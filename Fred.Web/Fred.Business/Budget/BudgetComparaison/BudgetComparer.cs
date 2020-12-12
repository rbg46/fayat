using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Business.Budget.BudgetComparaison.Dto;
using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison;
using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison.Result;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Dao;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Web.Shared.App_LocalResources;
using MoreLinq;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Permet de comparer des budgets.
    /// </summary>
    public class BudgetComparer : SimpleManagerFeature, IBudgetComparer
    {
        #region Membres

        private readonly IBudgetComparaisonReferentialService referentialService;
        private readonly IBudgetRepository budgetRepository;
        private readonly IUniteRepository uniteRepository;
        private readonly IBudgetSousDetailRepository budgetSousDetailRepository;
        private readonly ICIRepository ciRepository;

        private RequestDto request;
        private ResultDto result;
        private List<SousDetailDao> sousDetail1s;
        private List<SousDetailDao> sousDetail2s;
        private IEnumerable<SousDetailDao> sousDetails;
        private List<AxeInfoDao> taches;
        private List<AxeInfoDao> chapitres;
        private List<AxeInfoDao> sousChapitres;
        private List<AxeInfoDao> ressources;
        private IList<SkeletonNode> skeletonTree;
        private List<Func<SousDetailDao, int>> axeAnalytiqueGroupByFuncs;

        #endregion
        #region Constructeur

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="referentialService">Service d'accès aux référentiels.</param>
        /// <param name="budgetRepository">Référentiel de données pour les budgets.</param>
        /// <param name="uniteRepository">Référentiel de données pour les unités.</param>
        /// <param name="budgetSousDetailRepository">Référentiel de données pour les sous-détails budget.</param>
        /// <param name="ciRepository">Référentiel de données pour les CI.</param>
        public BudgetComparer(
            IBudgetComparaisonReferentialService referentialService,
            IBudgetRepository budgetRepository,
            IUniteRepository uniteRepository,
            IBudgetSousDetailRepository budgetSousDetailRepository,
            ICIRepository ciRepository)
        {
            this.budgetRepository = budgetRepository;
            this.referentialService = referentialService;
            this.uniteRepository = uniteRepository;
            this.budgetSousDetailRepository = budgetSousDetailRepository;
            this.ciRepository = ciRepository;
        }

        #endregion
        #region Properties

        /// <summary>
        /// L'identifiant du CI.
        /// </summary>
        public int CiId { get; private set; }

        #endregion
        #region Comparaison

        /// <summary>
        /// Compare les budgets.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>Le résultat de la comparaison.</returns>
        public ResultDto Compare(RequestDto request)
        {
            this.request = request;
            result = new ResultDto();

            if (!CheckRequest())
            {
                return result;
            }

            // Récupère les sous-détails à comparer
            sousDetail1s = budgetSousDetailRepository.GetSousDetailPourBudgetComparaison(request.BudgetId1);
            sousDetail2s = budgetSousDetailRepository.GetSousDetailPourBudgetComparaison(request.BudgetId2);
            if (sousDetail1s.Count > 0 || sousDetail2s.Count > 0)
            {
                sousDetails = sousDetail1s.Union(sousDetail2s);

                FillDevises();
                FillCommonUnites();

                // Crée l'arbre
                CreateTree();

                // Calcul les écarts
                if (result.Tree.Nodes.Any())
                {
                    foreach (var node in result.Tree.Nodes)
                    {
                        ComputeEcarts(node);
                    }
                    ComputeEcartTotal();
                }
            }

            SetUserInformation();
            return result;
        }

        /// <summary>
        /// Crée l'arbre de comparaison.
        /// </summary>
        private void CreateTree()
        {
            FillAxeAnalytiqueGroupByFuncs();
            CreateSkeletonTree();
            FillReferentielEtPlanDeTache();
            CreateTreeFromSkeleton();
        }

        #endregion
        #region Squelette de l'arbre

        /// <summary>
        /// Crée le squelette de l'arbre de comparaison.
        /// </summary>
        private void CreateSkeletonTree()
        {
            skeletonTree = sousDetails
                .GroupBy(sd => 0)
                .Select(g0 => new SkeletonMainNode
                {
                    Nodes = GetSkeletonNodes(g0, 0)
                })
                .First().Nodes;
        }

        /// <summary>
        /// Crée le squelette (récursif).
        /// </summary>
        /// <param name="group">Le groupe en cours.</param>
        /// <param name="nodeIndex">L'index dans la hiérarchie.</param>
        /// <returns>Les noeuds du groupe.</returns>
        private IList<SkeletonNode> GetSkeletonNodes(IGrouping<int, SousDetailDao> group, int nodeIndex)
        {
            var subGroups = group.GroupBy(axeAnalytiqueGroupByFuncs[nodeIndex]);

            if (nodeIndex < axeAnalytiqueGroupByFuncs.Count - 1)
            {
                return subGroups
                    .Select(subGroup => new SkeletonMainNode
                    {
                        Id = subGroup.Key,
                        Nodes = GetSkeletonNodes(subGroup, nodeIndex + 1)
                    })
                    .ToArray();
            }
            else
            {
                return subGroups
                    .Select(subGroup => new SkeletonLastNode
                    {
                        Id = subGroup.Key,
                        Budget1 = subGroup
                            .Where(sd => sd.BudgetId == request.BudgetId1)
                            .Select(sd => GetSkeletonLastGroup(sd))
                            .ToList(),
                        Budget2 = subGroup
                            .Where(sd => sd.BudgetId == request.BudgetId2)
                            .Select(sd => GetSkeletonLastGroup(sd))
                            .ToList()
                    })
                    .ToArray();
            }
        }

        /// <summary>
        /// Retourne un groupe pour les derniers noeuds qui contiennent les sous-détails.
        /// </summary>
        /// <param name="sousDetail">Le sous-détail concerné.</param>
        /// <returns>Le groupe.</returns>
        private GroupDto GetSkeletonLastGroup(SousDetailDao sousDetail)
        {
            return new GroupDto
            {
                Quantite = sousDetail.Quantite,
                UniteIds = new List<int?> { sousDetail.UniteId },
                PrixUnitaire = sousDetail.PrixUnitaire,
                Montant = sousDetail.Montant
            };
        }

        #endregion
        #region Référentiel et plan de tâche

        /// <summary>
        /// Récupère les informations du référentiel et du plan de tâche.
        /// </summary>
        private void FillReferentielEtPlanDeTache()
        {
            var axes = new Dictionary<AxeType, IEnumerable<SkeletonNode>>(request.Axes.Count);
            var nodes = skeletonTree;
            foreach (var axeType in request.Axes)
            {
                axes.Add(axeType, nodes);
                nodes = nodes.OfType<SkeletonMainNode>().SelectMany(n => n.Nodes).ToList();
            }

            var tacheIds = GetReferentielEtPlanDeTacheIds(axes, kvp => kvp.Key == AxeType.Tache1 || kvp.Key == AxeType.Tache2 || kvp.Key == AxeType.Tache3 || kvp.Key == AxeType.Tache4);
            if (tacheIds.Any())
            {
                taches = referentialService.TacheRepository.GetPourBudgetComparaison(tacheIds);
            }

            var chapitreIds = GetReferentielEtPlanDeTacheIds(axes, kvp => kvp.Key == AxeType.Chapitre);
            if (chapitreIds.Any())
            {
                chapitres = referentialService.ChapitreRepository.GetPourBudgetComparaison(chapitreIds);
            }

            var sousChapitreIds = GetReferentielEtPlanDeTacheIds(axes, kvp => kvp.Key == AxeType.SousChapitre);
            if (sousChapitreIds.Any())
            {
                sousChapitres = referentialService.SousChapitreRepository.GetPourBudgetComparaison(sousChapitreIds);
            }

            var ressourceIds = GetReferentielEtPlanDeTacheIds(axes, kvp => kvp.Key == AxeType.Ressource);
            if (ressourceIds.Any())
            {
                ressources = referentialService.RessourceRepository.GetPourBudgetComparaison(ressourceIds);
            }
        }

        /// <summary>
        /// Récupère les identifiants à utiliser pour le chargement du référentiel et du plan de tâche.
        /// </summary>
        /// <param name="axes">Dictionnaire de type d'axe / noeuds correspondants</param>
        /// <param name="predicate">Les axes concernés.</param>
        /// <returns>Les identifiants à utiliser pour le chargement du référentiel et du plan de tâche.</returns>
        private IEnumerable<int> GetReferentielEtPlanDeTacheIds(Dictionary<AxeType, IEnumerable<SkeletonNode>> axes, Func<KeyValuePair<AxeType, IEnumerable<SkeletonNode>>, bool> predicate)
        {
            return axes
                .Where(predicate)
                .SelectMany(kvp => kvp.Value)
                .Select(n => n.Id)
                .Distinct();
        }

        /// <summary>
        /// Récupère le code et le libellé d'un noeud.
        /// </summary>
        /// <param name="id">L'identifiant du noeud.</param>
        /// <param name="axeType">Le type d'axe du noeud.</param>
        /// <returns>Le code et le libellé du noeud.</returns>
        private CodeLibelleDao GetCodeAndLibelle(int id, AxeType axeType)
        {
            switch (axeType)
            {
                case AxeType.Tache1:
                case AxeType.Tache2:
                case AxeType.Tache3:
                case AxeType.Tache4:
                    return taches.First(t => t.Id == id);
                case AxeType.Chapitre:
                    return chapitres.First(t => t.Id == id);
                case AxeType.SousChapitre:
                    return sousChapitres.First(t => t.Id == id);
                case AxeType.Ressource:
                    return ressources.First(t => t.Id == id);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
        #region Arbre final

        /// <summary>
        /// Crée l'arbre à partir du squelette.
        /// </summary>
        private void CreateTreeFromSkeleton()
        {
            foreach (var skeletonNode in skeletonTree)
            {
                var treetNode = new NodeDto();
                result.Tree.Nodes.Add(treetNode);
                CreateTreeFromSkeleton(skeletonNode, treetNode, 0);
            }

            result.Tree.Nodes = result.Tree.Nodes.OrderBy(n => n.Code).ToList();
        }

        /// <summary>
        /// Crée l'arbre à partir du squelette (récursif).
        /// </summary>
        /// <param name="skeletonNode">Le noeud courant du squelette.</param>
        /// <param name="treeNode">Le noeud courant de l'arbre final.</param>
        /// <param name="nodeIndex">L'index dans la hiérarchie.</param>
        private void CreateTreeFromSkeleton(SkeletonNode skeletonNode, NodeDto treeNode, int nodeIndex)
        {
            var codeLibelle = GetCodeAndLibelle(skeletonNode.Id, request.Axes[nodeIndex]);
            treeNode.Code = codeLibelle.Code;
            treeNode.Libelle = codeLibelle.Libelle;

            var mainNode = skeletonNode as SkeletonMainNode;
            if (mainNode != null)
            {
                foreach (var skeletonSubNode in mainNode.Nodes)
                {
                    var resultSubNode = new NodeDto();
                    treeNode.Nodes.Add(resultSubNode);
                    CreateTreeFromSkeleton(skeletonSubNode, resultSubNode, nodeIndex + 1);
                }
                treeNode.Nodes = treeNode.Nodes.OrderBy(n => n.Code).ToList();

                UpdateGroup(treeNode.Budget1, treeNode.Nodes.Select(sg => sg.Budget1));
                UpdateGroup(treeNode.Budget2, treeNode.Nodes.Select(sg => sg.Budget2));
                return;
            }

            var lastNode = skeletonNode as SkeletonLastNode;
            if (lastNode != null)
            {
                UpdateGroup(treeNode.Budget1, lastNode.Budget1);
                UpdateGroup(treeNode.Budget2, lastNode.Budget2);
            }
        }

        /// <summary>
        /// Met à jour un groupe en fonction des composantes de ses enfants.
        /// </summary>
        /// <param name="group">Le groupe concerné.</param>
        /// <param name="subGroups">Les enfants concernés.</param>
        private void UpdateGroup(GroupDto group, IEnumerable<GroupDto> subGroups)
        {
            var subGroupsForMontant = subGroups.Where(sg => sg.Montant != null);
            if (subGroupsForMontant.Any())
            {
                bool hasPuAndQuantite = subGroupsForMontant.Where(sg => sg.Quantite != 0 && sg.PrixUnitaire != 0).Any();

                group.Quantite = subGroupsForMontant.Sum(sg => sg.Quantite);
                group.Montant = hasPuAndQuantite ? subGroupsForMontant.Sum(sg => sg.Montant) : 0;
                group.PrixUnitaire = group.Montant.HasValue && group.Quantite.HasValue && group.Quantite.Value != 0
                    ? group.Montant.Value / group.Quantite.Value
                    : 0;
            }
            group.UniteIds = GetGroupUniteIds(subGroups);
        }

        #endregion
        #region Ecarts

        /// <summary>
        /// Calcule les écarts sur un noeud.
        /// </summary>
        /// <param name="node">Le noeud concerné.</param>
        private void ComputeEcarts(NodeDto node)
        {
            node.Ecart.Quantite = GetEcart(node.Budget1.Quantite, node.Budget2.Quantite);
            node.Ecart.UniteIds = GetEcartUniteIds(node);
            node.Ecart.PrixUnitaire = GetEcart(node.Budget1.PrixUnitaire, node.Budget2.PrixUnitaire);
            node.Ecart.Montant = GetEcart(node.Budget1.Montant, node.Budget2.Montant);
            foreach (var subNode in node.Nodes)
            {
                ComputeEcarts(subNode);
            }
        }

        /// <summary>
        /// Retourne l'écart entre deux valeurs.
        /// </summary>
        /// <param name="value1">La première valeur.</param>
        /// <param name="value2">La seconde valeur.</param>
        /// <returns>L'écart.</returns>
        private decimal? GetEcart(decimal? value1, decimal? value2)
        {
            if (value1.HasValue)
            {
                if (value2.HasValue)
                {
                    var ecart = value1 - value2;
                    return ecart == 0 ? null : ecart;
                }
                else
                {
                    return value1;
                }
            }
            else
            {
                return value2.HasValue ? -value2 : null;
            }
        }

        /// <summary>
        /// Calcule l'écart total.
        /// </summary>
        private void ComputeEcartTotal()
        {
            var ecarts = result.Tree.Nodes.Select(n => n.Ecart).ToList();
            result.EcartTotal.Quantite = ecarts.Sum(e => e.Quantite);
            result.EcartTotal.UniteIds = ecarts.SelectMany(e => e.UniteIds).Distinct().ToList();
            result.EcartTotal.Montant = ecarts.Sum(e => e.Montant);
            result.EcartTotal.PrixUnitaire = result.EcartTotal.Montant.HasValue && result.EcartTotal.Quantite.HasValue && result.EcartTotal.Quantite.Value != 0
                ? result.EcartTotal.Montant.Value / result.EcartTotal.Quantite.Value
                : (decimal?)null;
        }

        #endregion
        #region Unités

        /// <summary>
        /// Récupère les unités communes aux deux budgets à comparer.
        /// </summary>
        private void FillCommonUnites()
        {
            var uniteIds = sousDetails
                .Where(sd => sd.UniteId != null)
                .Distinct()
                .Select(sd => sd.UniteId ?? 0);

            result.Unites = uniteRepository.GetUnitesPourBudgetComparaison(uniteIds)
                .Select(u => new UniteDto
                {
                    UniteId = u.UniteId,
                    Code = u.Code,
                    Libelle = u.Libelle
                })
                .ToList();
        }

        /// <summary>
        /// Récupère les identifiants des unités pour un groupe.
        /// </summary>
        /// <param name="subGroups">Les sous-groupes à utiliser.</param>
        /// <returns>Les identifiants des unités pour le groupe.</returns>
        private List<int?> GetGroupUniteIds(IEnumerable<GroupDto> subGroups)
        {
            return subGroups
                .SelectMany(sg => sg.UniteIds)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Récupère les identifiants des unités pour l'écart.
        /// </summary>
        /// <param name="node">Le noeud concerné.</param>
        /// <returns>Les identifiants des unités pour l'écart.</returns>
        private List<int?> GetEcartUniteIds(NodeDto node)
        {
            var ret = node.Budget1.UniteIds.Union(node.Budget2.UniteIds)
                .Except(node.Budget1.UniteIds.Intersect(node.Budget2.UniteIds))
                .ToList();
            return ret;
        }

        #endregion
        #region Autre

        /// <summary>
        /// Récupère les devises du CI des budgets.
        /// </summary>
        private void FillDevises()
        {
            result.Devises = ciRepository.GetDevisesPourBudgetComparaison(CiId)
                .Select(d => new DeviseDto
                {
                    DeviseId = d.DeviseId,
                    Symbole = d.Symbole,
                    IsoCode = d.IsoCode,
                    Libelle = d.Libelle
                })
                .ToList();
        }

        /// <summary>
        /// Rempli la liste des fonctions de grouping à utiliser pour le tri.
        /// </summary>
        private void FillAxeAnalytiqueGroupByFuncs()
        {
            axeAnalytiqueGroupByFuncs = new List<Func<SousDetailDao, int>>();
            foreach (var axeType in request.Axes)
            {
                axeAnalytiqueGroupByFuncs.Add(GetAxeAnalytiqueGroupByFunc(axeType));
            }
        }

        /// <summary>
        /// Retourne la fonction de grouping à utiliser en fonction du type d'axe analytique.
        /// </summary>
        /// <param name="axeType">Le type d'axe analytique.</param>
        /// <returns>La fonction de grouping à utiliser.</returns>
        private Func<SousDetailDao, int> GetAxeAnalytiqueGroupByFunc(AxeType axeType)
        {
            switch (axeType)
            {
                case AxeType.Tache1:
                    return sd => sd.Tache1Id;
                case AxeType.Tache2:
                    return sd => sd.Tache2Id;
                case AxeType.Tache3:
                    return sd => sd.Tache3Id;
                case AxeType.Tache4:
                    return sd => sd.Tache4Id;
                case AxeType.Chapitre:
                    return sd => sd.ChapitreId;
                case AxeType.SousChapitre:
                    return sd => sd.SousChapitreId;
                case AxeType.Ressource:
                    return sd => sd.RessourceId;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Vérifie que la requête est correcte et enregistre l'erreur le cas échéant.
        /// </summary>
        /// <returns>True si la requête est correcte, sinon false.</returns>
        private bool CheckRequest()
        {
            var ci1 = budgetRepository.GetBudget(request.BudgetId1, b => new { b.CiId, b.DeviseId });
            var ci2 = budgetRepository.GetBudget(request.BudgetId2, b => new { b.CiId, b.DeviseId });
            if (ci1 == null)
            {
                result.Erreur = string.Format(FeatureBudgetComparaison.BudgetComparaison_Erreur_BudgetNonTrouve, 1);
            }
            else if (ci2 == null)
            {
                result.Erreur = string.Format(FeatureBudgetComparaison.BudgetComparaison_Erreur_BudgetNonTrouve, 2);
            }
            else if (ci1.CiId != ci2.CiId)
            {
                result.Erreur = FeatureBudgetComparaison.BudgetComparaison_Erreur_BudgetsCiDifferents;
            }
            else if (ci1.DeviseId != ci2.DeviseId)
            {
                result.Erreur = FeatureBudgetComparaison.BudgetComparaison_Erreur_BudgetsDevisesDifferentes;
            }
            else
            {
                CiId = ci1.CiId;
                result.DeviseId = ci1.DeviseId;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ajoute les informations à destination de l'utilisateur.
        /// </summary>
        private void SetUserInformation()
        {
            if (sousDetail1s.Count == 0 && sousDetail2s.Count == 0)
            {
                result.Information = FeatureBudgetComparaison.BudgetComparaison_Information_Budgets_Vide;
            }
            else if (sousDetail1s.Count == 0)
            {
                result.Information = FeatureBudgetComparaison.BudgetComparaison_Information_Budget1_Vide;
            }
            else if (sousDetail2s.Count == 0)
            {
                result.Information = FeatureBudgetComparaison.BudgetComparaison_Information_Budget2_Vide;
            }
            else
            {
                var budgetsDifferents = result.Tree.Nodes
                    .Select(n => n.Ecart)
                    .Any(e => e.Quantite != null || e.UniteIds.Count > 0 || e.PrixUnitaire != null || e.Montant != null);
                if (!budgetsDifferents)
                {
                    result.Information = FeatureBudgetComparaison.BudgetComparaison_Information_Budgets_Identique;
                }
            }
        }

        #endregion
        #region Classes

        /// <summary>
        /// Représente la base d'un noeud du squelette de l'arbre de comparaison.
        /// </summary>
        [DebuggerDisplay("Id = {Id}")]
        private abstract class SkeletonNode
        {
            /// <summary>
            /// L'identifiant du noeud. Peut représenter l'identifiant d'un référentiel ou d'une tâche.
            /// </summary>
            public int Id { get; set; }
        }

        /// <summary>
        /// Représente un noeud principal du squelette de l'arbre de comparaison.
        /// </summary>
        private class SkeletonMainNode : SkeletonNode
        {
            /// <summary>
            /// Les sous noeuds.
            /// </summary>
            public IList<SkeletonNode> Nodes { get; set; }
        }

        /// <summary>
        /// Représente un dernier noeud du squelette de l'arbre de comparaison.
        /// </summary>
        private class SkeletonLastNode : SkeletonNode
        {
            /// <summary>
            /// Le groupe du budget 1.
            /// </summary>
            public List<GroupDto> Budget1 { get; set; }

            /// <summary>
            /// Le groupe du budget 2.
            /// </summary>
            public List<GroupDto> Budget2 { get; set; }
        }

        #endregion
    }
}
