using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget.BibliothequePrix;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using static Fred.Business.Budget.BudgetManager.Dtao.CopierBudgetDao;
using static Fred.Business.Budget.BudgetManager.Dtao.CopierBudgetDto;

namespace Fred.Business.Budget.BudgetManager
{
    public class CopierBudgetSourceToCible : ICopierBudgetSourceToCible
    {
        private readonly IUnitOfWork uow;
        private readonly IBudgetRepository budgetRepository;
        private readonly IBudgetSousDetailRepository budgetSousDetailRepository;
        private readonly IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository;
        private readonly IRepository<BudgetCopyHistoEnt> budgetCopyHistoRepository;
        private readonly ICIRepository ciRepository;
        private readonly ITacheManager tacheManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IBudgetBibliothequePrixManager budgetBibliothequePrixManager;
        private readonly IBudgetT4Repository budgetT4Repository;
        private readonly ITacheRepository tacheRepository;
        private readonly DateTime utcNow;

        private Result result;
        private int utilisateurId;
        private BudgetEnt budgetCible;

        private BudgetSourceDao budgetSource;
        private List<BibliothequePrixItemDao> bibliothequePrixItems;
        private bool composantesDuBudgetSource;
        private Lazy<List<BudgetCibleTacheDao>> budgetCibleTache3s;
        private Lazy<List<BudgetCibleTacheDao>> budgetCibleTache4s;

        public CopierBudgetSourceToCible(
            IUnitOfWork uow,
            ITacheRepository tacheRepository,
            IBudgetRepository budgetRepository,
            IBudgetSousDetailRepository budgetSousDetailRepository,
            IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository,
            ICIRepository ciRepository,
            ITacheManager tacheManager,
            IUtilisateurManager utilisateurManager,
            IBudgetBibliothequePrixManager budgetBibliothequePrixManager,
            IBudgetT4Repository budgetT4Repository,
            IRepository<BudgetCopyHistoEnt> budgetCopyHistoRepository)
        {
            this.tacheRepository = tacheRepository;
            this.budgetRepository = budgetRepository;
            this.budgetSousDetailRepository = budgetSousDetailRepository;
            this.budgetBibliothequePrixItemRepository = budgetBibliothequePrixItemRepository;
            this.ciRepository = ciRepository;
            this.tacheManager = tacheManager;
            this.utilisateurManager = utilisateurManager;
            this.budgetBibliothequePrixManager = budgetBibliothequePrixManager;
            this.budgetT4Repository = budgetT4Repository;
            this.uow = uow;
            this.budgetCopyHistoRepository = budgetCopyHistoRepository;
            utcNow = DateTime.UtcNow;
        }

        public async Task<Result> CopierAsync(Request request)
        {
            budgetCible = null;
            budgetSource = null;
            bibliothequePrixItems = null;
            budgetCibleTache3s = new Lazy<List<BudgetCibleTacheDao>>(() =>
            {
                return tacheManager.GetTaches(budgetCible.CiId, 3, false, false, new BudgetCibleTacheDao().Selector);
            });
            budgetCibleTache4s = new Lazy<List<BudgetCibleTacheDao>>(() =>
            {
                return tacheManager.GetTaches(budgetCible.CiId, 4, false, false, new BudgetCibleTacheDao().Selector);
            });
            result = new Result();
            var memeCi = request.BudgetSourceCiId == request.BudgetCibleCiId;

            // Si le CI source est différent du CI cible alors il faut que les plans de tâches soient identiques (T1 à T3)
            if (!memeCi)
            {
                var planDeTacheIdentiques = CheckPlanDeTacheIdentiques(request.BudgetCibleCiId, request.BudgetSourceCiId);
                if (!planDeTacheIdentiques)
                {
                    result.Erreur = FeatureBudgetDetail.CopierBudget_Erreur_PlanDeTacheDifferent;
                    return result;
                }
            }

            // Charge le budget source
            utilisateurId = utilisateurManager.GetContextUtilisateurId();
            budgetSource = budgetRepository.GetBudgetVisiblePourUserSurCi(
                utilisateurId,
                request.BudgetSourceCiId,
                request.DeviseId,
                request.BudgetSourceRevision,
                new BudgetSourceDao().Selector);
            if (budgetSource == null)
            {
                result.Erreur = FeatureBudgetDetail.CopierBudget_Error_BudgetSourceInexistantOuInvisible;
                return result;
            }

            // Charge la bibliothèque
            if (request.BibliothequePrixOrganisationId.HasValue)
            {
                var bibliothequePrixExists = budgetBibliothequePrixManager.Exists(request.BibliothequePrixOrganisationId.Value, request.DeviseId);
                if (bibliothequePrixExists)
                {
                    bibliothequePrixItems = budgetBibliothequePrixItemRepository.GetAllBibliothequePrixItemForOrgaCi(
                        request.BibliothequePrixOrganisationId.Value,
                        request.DeviseId,
                        new BibliothequePrixItemDao().Selector);
                }
            }

            // Charge le budget cible
            budgetCible = await budgetRepository.GetTargetBudgetForCopyAsync(request.BudgetCibleId);

            if (budgetCible == null)
            {
                result.Erreur = FeatureBudgetDetail.CopierBudget_Error_BudgetCibleInexistant;
                return result;
            }

            // Copie les BudgetT4
            composantesDuBudgetSource = bibliothequePrixItems != null ? false : request.ComposantesDuBudgetSource.HasValue && request.ComposantesDuBudgetSource.Value;
            if (!CopyBudgetT4s(request))
            {
                return result;
            }

            // Recalcule le budget et met à jour l'historique si nécessaire
            if (result.Tache4IdCopies.Count > 0)
            {
                RecalculateBudgetCible();
                UpdateBudgetCibleHisto(request);
            }

            List<BudgetT4Ent> tachesWithVueT4 = budgetCible.BudgetT4s.Where(t4 => t4.VueSD == 0).ToList();

            // Enregistre
            uow.Save();

            //Sauvegarde de la liste des budgets T4 avec la vue T4;
            //Obligation de forcer l'update a 0, parce que l'insert entity framework core ne les détectent pas.

            //insert avec vue SD à 0:
            /*
             * INSERT INTO [dbo].[FRED_BUDGET_T4] ([BudgetId], [Commentaire], [MontantT4], [PU], [QuantiteARealiser], [QuantiteDeBase], [T3Id], [T4Id], [TypeAvancement], [UniteId])
                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9);
                SELECT [BudgetT4Id], [IsReadOnly], [VueSD]
                FROM [dbo].[FRED_BUDGET_T4]
                WHERE @@ROWCOUNT = 1 AND [BudgetT4Id] = scope_identity();
             */
            //insert avec vue SD à 1:
            /*
             * INSERT INTO [dbo].[FRED_BUDGET_T4] ([BudgetId], [Commentaire], [MontantT4], [PU], [QuantiteARealiser], [QuantiteDeBase], [T3Id], [T4Id], [TypeAvancement], [UniteId], [VueSD])
                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10);
                SELECT [BudgetT4Id], [IsReadOnly]
                FROM [dbo].[FRED_BUDGET_T4]
                WHERE @@ROWCOUNT = 1 AND [BudgetT4Id] = scope_identity();
             */
            await budgetT4Repository.UpdateVueSDToZero(tachesWithVueT4);

            return result;
        }

        /// <summary>
        /// Compare deux plans de tâche (T1 à T3).
        /// </summary>
        /// <param name="ciId1">Identifiant d'un CI.</param>
        /// <param name="ciId2">Identifiant d'un autre CI.</param>
        /// <returns>True si les plans de tâches sont identiques, sinon false.</returns>
        public bool CheckPlanDeTacheIdentiques(int ciId1, int ciId2)
        {
            if (ciId1 == ciId2)
            {
                return true;
            }

            var taches1 = tacheManager.GetTaches(ciId1, false, t => new
                {
                    t.Code,
                    t.Libelle,
                    t.Niveau,
                    t.Active,
                    ParentCode = t.Parent.Code
                })
                .OrderBy(t => t.Code)
                .ToList();

            var taches2 = tacheManager.GetTaches(ciId2, false, t => new
                {
                    t.Code,
                    t.Libelle,
                    t.Niveau,
                    t.Active,
                    ParentCode = t.Parent.Code
                })
                .OrderBy(t => t.Code)
                .ToList();

            if (taches1.Count != taches2.Count)
            {
                return false;
            }

            for (var i = 0; i < taches1.Count; i++)
            {
                var t1 = taches1[i];
                var t2 = taches2[i];
                if (t1.Code != t2.Code
                    || t1.Libelle != t2.Libelle
                    || t1.Niveau != t2.Niveau
                    || t1.Active != t2.Active
                    || t1.ParentCode != t2.ParentCode)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CopyBudgetT4s(Request request)
        {
            // Copier uniquement sur les lignes vides indique que la copie ne se fera que si le BudgetT4 cible ne possède pas de sous-détail
            // Copier intégralement effectue la copie dans tous les cas
            var budgetCibleBudgetT4s = new List<BudgetT4Ent>(budgetCible.BudgetT4s);
            foreach (var budgetT4Source in budgetSource.BudgetT4s)
            {
                var budgetT4Cible = budgetCibleBudgetT4s.FirstOrDefault(bt4Cible => bt4Cible.T4.Code == budgetT4Source.T4.Code);
                if (budgetT4Cible != null)
                {
                    // Les budgets cible et source possèdent tous les 2 un BudgetT4 avec le même code tâche 4
                    if (request.OnlyLignesVides && budgetT4Cible.BudgetSousDetails.Count > 0)
                    {
                        continue;
                    }
                    CopyBudgetT4(budgetT4Cible, budgetT4Source);
                }
                else
                {
                    // Le budget cible ne possède pas de BudgetT4 avec le même code tâche 4
                    // Récupération de la tâche 3 parente dans le budget cible
                    var budgetT4CibleT3Parent = budgetCibleTache3s.Value.FirstOrDefault(a => a.Code == budgetT4Source.T3Code);
                    if (budgetT4CibleT3Parent == null)
                    {
                        // Les plans de tâche cible et source sont sensés être identique
                        // Si on arrive là c'est que le plan de tâche du budget cible a changé
                        result.Erreur = FeatureBudgetDetail.CopierBudget_Erreur_PlanDeTacheDifferent;
                        return false;
                    }

                    // Création du budget T4 cible
                    budgetT4Cible = new BudgetT4Ent
                    {
                        BudgetId = budgetCible.BudgetId,
                        T3Id = budgetT4CibleT3Parent.TacheId,
                        BudgetSousDetails = new List<BudgetSousDetailEnt>()
                    };

                    // Soit la tâche 4 correspondante existe dans le plan de tâche du budget cible mais n'est pas
                    //  référencée, dans ce cas on l'utilise, soit elle n'existe pas et il faut la créer
                    var budgetCibleTache4 = budgetCibleTache4s.Value.FirstOrDefault(t4 => t4.Code == budgetT4Source.T4.Code);
                    if (budgetCibleTache4 == null)
                    {
                        // Création de la tâche 4 correspondante qui n'existe pas
                        var tache4 = new TacheEnt
                        {
                            Code = budgetT4Source.T4.Code,
                            Libelle = budgetT4Source.T4.Libelle,
                            TacheParDefaut = false,
                            CiId = budgetCible.CiId,
                            ParentId = budgetT4CibleT3Parent.TacheId,
                            Niveau = 4,
                            Active = budgetT4Source.T4.Active,
                            TacheType = budgetT4Source.T4.TacheType,
                            DateCreation = utcNow,
                            AuteurCreationId = utilisateurId
                        };
                        tacheRepository.Insert(tache4);
                        budgetT4Cible.T4 = tache4;
                    }
                    else
                    {
                        // Utilisation de la tâche 4 correspondante
                        budgetT4Cible.T4Id = budgetCibleTache4.TacheId;
                    }

                    budgetCible.BudgetT4s.Add(budgetT4Cible);
                    CopyBudgetT4(budgetT4Cible, budgetT4Source);
                }
            }
            return true;
        }

        private void CopyBudgetT4(
            BudgetT4Ent budgetT4Cible,
            BudgetSourceBudgetT4Dao budgetT4Source)
        {
            // Note : PU et MontantT4 vont être recalculés plus tard
            // NPI : IsReadonly ?
            result.Tache4IdCopies.Add(budgetT4Cible.T4Id);
            budgetT4Cible.UniteId = budgetT4Source.UniteId;
            budgetT4Cible.QuantiteARealiser = budgetT4Source.QuantiteARealiser;
            budgetT4Cible.Commentaire = budgetT4Source.Commentaire;
            budgetT4Cible.TypeAvancement = budgetT4Source.TypeAvancement;
            budgetT4Cible.QuantiteDeBase = budgetT4Source.QuantiteDeBase;
            budgetT4Cible.VueSD = budgetT4Source.VueSD;
            CopyBudgetSousDetails(budgetT4Cible, budgetT4Source);
        }

        private void CopyBudgetSousDetails(
            BudgetT4Ent budgetT4Cible,
            BudgetSourceBudgetT4Dao budgetT4Source)
        {
            // Supprime les sous-détails existants de la cible
            var budgetSousDetailsCible = new List<BudgetSousDetailEnt>(budgetT4Cible.BudgetSousDetails);
            foreach (var budgetSousDetailCible in budgetSousDetailsCible)
            {
                budgetSousDetailRepository.Delete(budgetSousDetailCible);
            }

            // Ajoute les sous-détails source dans la cible
            foreach (var budgetSousDetailsSource in budgetT4Source.BudgetSousDetails)
            {
                var sousDetailCible = new BudgetSousDetailEnt
                {
                    BudgetT4Id = budgetT4Cible.BudgetT4Id,
                    RessourceId = budgetSousDetailsSource.RessourceId,
                    Quantite = budgetSousDetailsSource.Quantite,
                    QuantiteFormule = budgetSousDetailsSource.QuantiteFormule,
                    Montant = budgetSousDetailsSource.Montant,
                    QuantiteSD = budgetSousDetailsSource.QuantiteSD,
                    QuantiteSDFormule = budgetSousDetailsSource.QuantiteSDFormule,
                    Commentaire = budgetSousDetailsSource.Commentaire,
                };

                SetSousDetailCiblePuEtUniteId(sousDetailCible, budgetSousDetailsSource);
                budgetT4Cible.BudgetSousDetails.Add(sousDetailCible);
            }
        }

        private void SetSousDetailCiblePuEtUniteId(
            BudgetSousDetailEnt sousDetailCible,
            BudgetSourceSousDetailDao budgetSousDetailsSource)
        {
            if (bibliothequePrixItems != null)
            {
                var item = bibliothequePrixItems.SingleOrDefault(i => i.RessourceId == budgetSousDetailsSource.RessourceId);
                sousDetailCible.PU = item?.Prix;
                sousDetailCible.UniteId = item?.UniteId;
            }
            else if (composantesDuBudgetSource)
            {
                sousDetailCible.PU = budgetSousDetailsSource.PU;
                sousDetailCible.UniteId = budgetSousDetailsSource.UniteId;
            }
            else
            {
                sousDetailCible.PU = 0;
                sousDetailCible.UniteId = null;
            }
        }

        private void RecalculateBudgetCible()
        {
            foreach (var budgetT4 in budgetCible.BudgetT4s)
            {
                if (result.Tache4IdCopies.Contains(budgetT4.T4Id))
                {
                    budgetT4.PU = null;
                    budgetT4.MontantT4 = null;
                    if (budgetT4.VueSD == 1)
                    {
                        RecalculateBudgetT4VueSD(budgetT4);
                    }
                    else
                    {
                        RecalculateBudgetT4VueT4(budgetT4);
                    }
                }
            }
        }

        private void RecalculateBudgetT4VueSD(BudgetT4Ent budgetT4)
        {
            foreach (var sousDetail in budgetT4.BudgetSousDetails)
            {
                if (sousDetail.QuantiteSD.HasValue && sousDetail.PU.HasValue && budgetT4.QuantiteDeBase != null && budgetT4.QuantiteDeBase != 0)
                {
                    var puT4 = sousDetail.PU.Value * sousDetail.QuantiteSD / budgetT4.QuantiteDeBase;
                    AddPU(budgetT4, puT4);
                    if (budgetT4.QuantiteARealiser != null || budgetT4.QuantiteARealiser != 0)
                    {
                        AddMontant(budgetT4, puT4 * budgetT4.QuantiteARealiser);
                    }
                }
            }
        }

        private void RecalculateBudgetT4VueT4(BudgetT4Ent budgetT4)
        {
            foreach (var sousDetail in budgetT4.BudgetSousDetails)
            {
                if (sousDetail.Quantite.HasValue && sousDetail.PU.HasValue)
                {
                    var montantT4 = sousDetail.Quantite.Value * sousDetail.PU.Value;
                    AddMontant(budgetT4, montantT4);
                    if (budgetT4.QuantiteARealiser != null || budgetT4.QuantiteARealiser != 0)
                    {
                        AddPU(budgetT4, montantT4 / budgetT4.QuantiteARealiser);
                    }
                }
            }
        }

        private void AddPU(BudgetT4Ent budgetT4, decimal? pu)
        {
            budgetT4.PU = budgetT4.PU.HasValue
                ? budgetT4.PU.Value + pu
                : pu;
        }

        private void AddMontant(BudgetT4Ent budgetT4, decimal? montant)
        {
            budgetT4.MontantT4 = budgetT4.MontantT4.HasValue
                ? budgetT4.MontantT4.Value + montant
                : montant;
        }

        private void UpdateBudgetCibleHisto(Request request)
        {
            var budgetHistoCopy = budgetCopyHistoRepository.Get()
                .FirstOrDefault(bci => bci.BudgetId == budgetCible.BudgetId);

            int? bibliothequePrixSourceCIId = null;
            if (request.BibliothequePrixOrganisationId.HasValue)
            {
                bibliothequePrixSourceCIId = ciRepository.Get()
                    .Where(ci => ci.Organisation.OrganisationId == request.BibliothequePrixOrganisationId.Value)
                    .Select(ci => ci.CiId)
                    .FirstOrDefault();
            }

            if (budgetHistoCopy == null)
            {
                budgetHistoCopy = new BudgetCopyHistoEnt { BudgetId = budgetCible.BudgetId };
            }

            budgetHistoCopy.BudgetSourceCIId = request.BudgetSourceCiId;
            budgetHistoCopy.BudgetSourceVersion = request.BudgetSourceRevision;
            budgetHistoCopy.BibliothequePrixSourceCIId = bibliothequePrixSourceCIId;
            budgetHistoCopy.DateCopy = utcNow;

            budgetCopyHistoRepository.InsertOrUpdate(x => new { x.BudgetCopyHistoId }, new List<BudgetCopyHistoEnt> { budgetHistoCopy });
        }
    }
}
