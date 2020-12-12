using Fred.Business.Budget.Helpers.Extensions;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Referential.Tache;
using Fred.DataAccess.Referential.Common;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Cette classe permet de grouper des éléments en fonction d'un axe analytique
    /// </summary>
    public class AxeTreeGrouper
    {
        private readonly AxeTreeBuilder builder;
        private readonly AxePrincipal axePrincipal;
        private readonly ITacheSearchHelper taskSearchHelper;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="builder">Instance d'un AxeTreeBuilder qui utilisera ces fonctions</param>
        /// <param name="axePrincipal">axe principal choisi par l'utilisateur</param>
        public AxeTreeGrouper(AxeTreeBuilder builder, AxePrincipal axePrincipal, ITacheSearchHelper taskSearchHelper)
        {
            this.builder = builder;
            this.axePrincipal = axePrincipal;
            this.taskSearchHelper = taskSearchHelper;
        }

        /// <summary>
        /// Groupe par T1
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant la même tache T1</returns>
        public List<AxeTreeModel> GroupByT1(AxeTreeDataSource subSource)
        {
            var tacheRows = subSource.Valeurs.Where(item => item.Tache3.Parent.ParentId == null).ToList();
            if (tacheRows.Any())
            {
                var tachesEcart = subSource.Valeurs.Where(item =>
                    item.Tache3.Parent.ParentId != null && taskSearchHelper.IsTacheEcart(item.Tache3.Parent.Parent)).ToList();

                if (tachesEcart.Any())
                {
                    var t1Ecart = tachesEcart.Select(item => item.Tache3.Parent.Parent).FirstOrDefault();

                    foreach (var axeTreeDataSourceRow in tacheRows)
                    {
                        axeTreeDataSourceRow.Tache3.Parent.Parent = t1Ecart;
                        axeTreeDataSourceRow.Tache3.Parent.ParentId = t1Ecart.TacheId;
                    }
                }
            }

            var groupedByT1 = subSource.Valeurs.GroupBy(x => x.Tache3.Parent.ParentId)
                    .Select(w => new
                    {
                        TacheId = w.Key,
                        Tache = w.First().Tache3.Parent.Parent,
                        AxeTreeCells = w.ToList()
                    });

            var result = new List<AxeTreeModel>();

            foreach (var t1 in groupedByT1)
            {
                var t1Axe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.T1,
                    Libelle = GetLibelleAxeFromTache(t1.Tache),
                    Code = t1.Tache.Code,
                    Source = t1.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(t1.AxeTreeCells, AxeTypes.T1)
                };

                result.Add(t1Axe);
            }
            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        /// <summary>
        /// Groupe par T2
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant la même tache T2</returns>
        public List<AxeTreeModel> GroupByT2(AxeTreeDataSource subSource)
        {
            var groupedByT2 = subSource.Valeurs.GroupBy(x => x.Tache3.ParentId)
                        .Select(w => new
                        {
                            TacheId = w.Key,
                            Tache = w.First().Tache3.Parent,
                            AxeTreeCells = w.ToList()
                        });

            var result = new List<AxeTreeModel>();

            foreach (var t2 in groupedByT2)
            {
                var t2Axe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.T2,
                    Libelle = GetLibelleAxeFromTache(t2.Tache),
                    Code = t2.Tache.Code,
                    Source = t2.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(t2.AxeTreeCells, AxeTypes.T2)
                };

                result.Add(t2Axe);
            }
            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        /// <summary>
        /// Groupe par T3
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant la même tache T3</returns>
        public List<AxeTreeModel> GroupByT3(AxeTreeDataSource subSource)
        {
            var groupedByT3 = subSource.Valeurs.GroupBy(x => x.Tache3.TacheId)
                     .Select(w => new
                     {
                         TacheId = w.Key,
                         Tache = w.First().Tache3,
                         AxeTreeCells = w.ToList()
                     });

            var result = new List<AxeTreeModel>();
            foreach (var t3 in groupedByT3)
            {
                var t3Axe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.T3,
                    Libelle = GetLibelleAxeFromTache(t3.Tache),
                    Code = t3.Tache.Code,
                    TacheId = t3.Tache.TacheId,
                    Source = t3.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(t3.AxeTreeCells, AxeTypes.T3)
                };

                if (axePrincipal == AxePrincipal.RessourceTache)
                {
                    //Si on est dans ce cas là, alors cette T3 est l'enfant d'une ressource et donc toutes
                    //les lignes qu'il y a dans Source on la même ressource
                    var ressource = t3Axe.Source.First().Ressource;
                    if (ressource != null) t3Axe.RessourceId = ressource.RessourceId;
                }
                result.Add(t3Axe);
            }

            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        /// <summary>
        /// Groupe par Chapitre
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant le même chapitre</returns>
        public List<AxeTreeModel> GroupByChapitre(AxeTreeDataSource subSource)
        {
            if (subSource == null)
            {
                return null;
            }

            var groupedByChapitre = subSource.Valeurs.GroupBy(x => x.Ressource?.SousChapitre.ChapitreId ?? x.Tache3.TacheId)
                .Select(w => new
                {
                    RessourceId = w.Key,
                    Chapitre = w.First().Ressource?.SousChapitre.Chapitre,
                    AxeTreeCells = w.ToList()
                });

            var result = new List<AxeTreeModel>();
            foreach (var chapitre in groupedByChapitre)
            {
                var chapitreAxe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.Chapitre,
                    Libelle = chapitre.Chapitre?.Libelle,
                    Code = chapitre.Chapitre?.Code,
                    Source = chapitre.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(chapitre.AxeTreeCells, AxeTypes.Chapitre)
                };

                result.Add(chapitreAxe);
            }

            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        /// <summary>
        /// Groupe par Sous Chapitre
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant le même Sous Chapitre</returns>
        public List<AxeTreeModel> GroupBySousChapitre(AxeTreeDataSource subSource)
        {
            if (subSource == null)
            {
                return null;
            }

            var groupedBySousChapitre = subSource.Valeurs.GroupBy(x => x.Ressource?.SousChapitreId ?? x.Tache3.TacheId)
                .Select(w => new
                {
                    RessourceId = w.Key,
                    SousChapitre = w.First().Ressource?.SousChapitre,
                    AxeTreeCells = w.ToList()
                });

            var result = new List<AxeTreeModel>();
            foreach (var sousChapitre in groupedBySousChapitre)
            {
                var sousChapitreAxe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.SousChapitre,
                    Libelle = sousChapitre.SousChapitre?.Libelle,
                    Code = sousChapitre.SousChapitre?.Code,
                    Source = sousChapitre.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(sousChapitre.AxeTreeCells, AxeTypes.SousChapitre)
                };
                result.Add(sousChapitreAxe);
            }

            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        /// <summary>
        /// Groupe par Ressource
        /// </summary>
        /// <param name="subSource">un ensemble de lignes</param>
        /// <returns>l'ensemble de lignes ayant la même Ressource</returns>
        public List<AxeTreeModel> GroupByRessource(AxeTreeDataSource subSource)
        {
            var groupedByRessource = subSource.Valeurs.GroupBy(x => x.Ressource?.RessourceId ?? x.Tache3.TacheId)
                          .Select(w => new
                          {
                              RessourceId = w.Key,
                              Ressource = w.First().Ressource,
                              AxeTreeCells = w.ToList()
                          });

            var result = new List<AxeTreeModel>();
            foreach (var ressource in groupedByRessource)
            {
                var ressourceAxe = new AxeTreeModel()
                {
                    AxeType = AxeTypes.Ressource,
                    Libelle = ressource.Ressource?.Libelle,
                    Code = ressource.Ressource?.Code,
                    RessourceId = ressource.RessourceId,
                    Source = ressource.AxeTreeCells,
                    Valeurs = builder.ComputeCellValues(ressource.AxeTreeCells, AxeTypes.Ressource)
                };

                if (axePrincipal == AxePrincipal.TacheRessource)
                {
                    //Si on est dans ce cas là, alors cette ressource est l'enfant d'un T3 et donc toutes
                    //les lignes qu'il y a dans Source on la même T3
                    ressourceAxe.TacheId = ressourceAxe.Source.First().Tache3.TacheId;
                }

                result.Add(ressourceAxe);
            }
            result = result.OrderBy(res => res.Libelle).ToList();
            return result;
        }

        private static string GetLibelleAxeFromTache(TacheEnt tache)
        {
            return tache.Libelle;
        }
    }
}
