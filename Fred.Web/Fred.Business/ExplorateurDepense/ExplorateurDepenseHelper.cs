using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Valorisation;
using Fred.Framework.Comparers;
using Fred.Framework.Extensions;
using Fred.Framework.Models;
using static Fred.Entities.Constantes;
using DepenseType = Fred.Entities.DepenseType;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Helper du manager explorateur de dépenses
    /// </summary>
    public class ExplorateurDepenseHelper
    {
        /// <summary>
        /// Enum axe analytique
        /// </summary>
        public enum AxeAnalytique
        {
            /// <summary>
            /// Axe tache vers ressource
            /// </summary>
            TacheRessource = 0,

            /// <summary>
            /// Axe ressource vers tache
            /// </summary>
            RessourceTache = 1
        }

        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertDepenseAchats(List<DepenseAchatEnt> depenseAchats, List<int?> idOfDepenseAchatWithFacture = null)
        {
            var result = new List<ExplorateurDepenseGeneriqueModel>();

            var depensesToConverts = depenseAchats.Where(x => !IsAdjustingEntry(x)).ToList();

            Dictionary<int, List<DepenseAchatEnt>> adjustingEntriesDictionnary = CreateAdjustingEntriesDictionnary(depenseAchats);

            foreach (DepenseAchatEnt depenseAchat in depensesToConverts)
            {
                ExplorateurDepenseGeneriqueModel model = ConvertDepenseAchat(depenseAchat, adjustingEntriesDictionnary, idOfDepenseAchatWithFacture);
                if (model != null)
                {
                    result.Add(model);
                }
            }

            return result;
        }

        private bool IsAdjustingEntry(DepenseAchatEnt depenseAchat)
        {
            var depenseType = (DepenseType)depenseAchat.DepenseType.Code;

            return depenseType == DepenseType.ExtourneFar;
        }

        private Dictionary<int, List<DepenseAchatEnt>> CreateAdjustingEntriesDictionnary(List<DepenseAchatEnt> depenseAchats)
        {
            return depenseAchats.Where(x => x.DepenseParentId.HasValue && x.DepenseParent != null &&
                                            !x.DepenseParent.DateSuppression.HasValue &&
                                            (DepenseType)x.DepenseType.Code == DepenseType.ExtourneFar
                                        )
                                .GroupBy(x => x.DepenseParentId.Value)
                                .ToDictionary(x => x.Key, x => x.ToList());
        }

        private ExplorateurDepenseGeneriqueModel ConvertDepenseAchat(DepenseAchatEnt depenseAchat,
                                                                     Dictionary<int, List<DepenseAchatEnt>> adjustingEntriesDictionnary,
                                                                     List<int?> idOfDepenseAchatWithFacture = null)
        {
            ExplorateurDepenseGeneriqueModel generiqueModel = InitializeConvertDepenseAchat(depenseAchat, idOfDepenseAchatWithFacture);
            if (!IsReception(depenseAchat))
                return generiqueModel;

            var result = ProcessAdjustingEntries(depenseAchat, adjustingEntriesDictionnary, generiqueModel);
            if (!result.Success)
            {
                return null;
            }
            return generiqueModel;

        }

        private bool IsReception(DepenseAchatEnt depenseAchat)
        {
            return (DepenseType)depenseAchat.DepenseType.Code == DepenseType.Reception;
        }


        private Result<string> ProcessAdjustingEntries(DepenseAchatEnt depenseAchat,
                                                        Dictionary<int, List<DepenseAchatEnt>> adjustingEntriesDictionnary,
                                                        ExplorateurDepenseGeneriqueModel generiqueModel)
        {
            List<DepenseAchatEnt> adjustingEntries = GetAdjustingEntriesForDepenseAchat(depenseAchat, adjustingEntriesDictionnary);
            if (!adjustingEntries.Any())
                return Result<string>.CreateSuccess("Ok");

            var result = AdjustReceptionAccountingInfo(depenseAchat, adjustingEntries, generiqueModel);
            if (!result.Success)
            {
                return Result<string>.CreateFailure("Ko");
            }

            FlagTaskAsNotReplaceable(generiqueModel);
            return Result<string>.CreateSuccess("Ok");
        }

        private List<DepenseAchatEnt> GetAdjustingEntriesForDepenseAchat(DepenseAchatEnt depenseAchat, Dictionary<int, List<DepenseAchatEnt>> adjustingEntriesDictionnary)
        {
            if (adjustingEntriesDictionnary.ContainsKey(depenseAchat.DepenseId))
            {
                return adjustingEntriesDictionnary[depenseAchat.DepenseId];
            }

            return new List<DepenseAchatEnt>();
        }

        private Result<string> AdjustReceptionAccountingInfo(DepenseAchatEnt depenseAchatEntity, List<DepenseAchatEnt> adjustingEntries, ExplorateurDepenseGeneriqueModel depenseAchatModel)
        {
            decimal finalAmount = (depenseAchatEntity.Quantite * depenseAchatEntity.PUHT) + adjustingEntries.Sum(x => x.Quantite * x.PUHT);
            if (finalAmount == 0)
                return Result<string>.CreateFailure("Ko");

            decimal finalQuantity = depenseAchatEntity.Quantite + adjustingEntries.Sum(x => x.Quantite);
            if (finalQuantity == 0)
                return Result<string>.CreateFailure("Ko");

            depenseAchatModel.MontantHT = finalAmount;
            depenseAchatModel.Quantite = finalQuantity;
            depenseAchatModel.PUHT = finalAmount / finalQuantity;

            return Result<string>.CreateSuccess("Ok");
        }

        private void FlagTaskAsNotReplaceable(ExplorateurDepenseGeneriqueModel depenseAchatModel) => depenseAchatModel.TacheRemplacable = false;


        /// <summary>
        /// Converti une DepenseAchatEnt en ExplorateurDepenseGenerique pour l'export
        /// </summary>
        /// <param name="depenseAchats">Liste des dépenses achat à convertir</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertForExportDepenseAchat(List<DepenseAchatEnt> depenseAchats)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();
            foreach (DepenseAchatEnt depAchat in depenseAchats)
            {
                ExplorateurDepenseGeneriqueModel expDep = ConvertForExportDepenseAchat(depAchat);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une ValorisationEnt en ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="valorisations">Liste des dépenses à convertir</param>        
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertValorisation(List<ValorisationEnt> valorisations, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();
            foreach (ValorisationEnt valo in valorisations)
            {
                ExplorateurDepenseGeneriqueModel expDep = ConvertValorisation(valo, periodeDebut, periodeFin, datesClotureComptables);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une ValorisationEnt en ExplorateurDepenseGenerique pour l'export
        /// </summary>
        /// <param name="valorisations">Liste des dépenses à convertir</param>        
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertForExportValorisation(List<ValorisationEnt> valorisations, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();
            foreach (ValorisationEnt valo in valorisations)
            {
                ExplorateurDepenseGeneriqueModel expDep = ConvertForExportValorisation(valo, periodeDebut, periodeFin, datesClotureComptables);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une OperationDiverseEnt en ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="operationDiverses">Liste des dépenses à convertir</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertOperationDiverse(List<OperationDiverseEnt> operationDiverses, bool useOdLibelleCourt, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();

            foreach (OperationDiverseEnt od in operationDiverses)
            {
                ExplorateurDepenseGeneriqueModel expDep = ConvertOperationDiverse(od, datesClotureComptables, useOdLibelleCourt);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une OperationDiverseEnt en ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="operationDiverses">Liste des dépenses à convertir</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> ConvertForExportOperationDiverse(List<OperationDiverseEnt> operationDiverses, bool useOdLibelleCourt, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();

            foreach (OperationDiverseEnt od in operationDiverses)
            {
                ExplorateurDepenseGeneriqueModel expDep = ConvertForExportOperationDiverse(od, datesClotureComptables, useOdLibelleCourt);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Création et récupération de l'arbre d'exploration des dépenses
        /// </summary>
        /// <param name="order">Ordre des axes</param>
        /// <param name="depenses">Liste des dépenses</param>
        /// <param name="axePrincipal">Liste d'axes 1</param>
        /// <param name="axeSecondaire">Liste d'axes 2</param>
        /// <returns>Arbre d'exploration des dépenses</returns>
        public List<ExplorateurAxe> GetTree(int order, IEnumerable<ExplorateurDepenseGeneriqueModel> depenses, string[] axePrincipal, string[] axeSecondaire)
        {
            List<ExplorateurAxe> tree1 = CreateTree(depenses, axePrincipal, 0, parent: null);
            List<ExplorateurAxe> result = tree1;

            if (axeSecondaire.Length > 0)
            {
                // Fusion des deux axes  
                result = Merging(order, tree1, depenses, axeSecondaire);
            }

            if (axePrincipal.Length == 1 && axePrincipal[0] == AnalysisAxis.Ressource && axeSecondaire.Length == 0)
            {
                result = result.OrderBy(x => x.CodeChapitreParent).ToList();
            }

            return result;
        }

        /// <summary>
        ///   Filtre par axe la liste des dépenses
        /// </summary>
        /// <param name="depenses">Liste des dépenses</param>
        /// <param name="axes">liste d'axe</param>
        /// <returns>Liste de dépenses filtrée par axe</returns>
        public List<ExplorateurDepenseGeneriqueModel> FilteringByAxes(List<ExplorateurDepenseGeneriqueModel> depenses, List<Axe> axes)
        {
            List<ExplorateurDepenseGeneriqueModel> results = new List<ExplorateurDepenseGeneriqueModel>();

            foreach (Axe axe in axes)
            {
                Dictionary<string, ExplorateurDepenseGeneriqueModel> currentResultDictionary = results.ToDictionary(x => x.Id);
                if (axe.Axe2 != null)
                {
                    var generiqueModelsFiltered = depenses.Where(FilterByAxeType(axe.Axe1)).Where(FilterByAxeType(axe.Axe2)).ToList();
                    var generiqueModelsNotInResult = generiqueModelsFiltered.Where(x => !GetGeneriqueModelIsAlreadyInResult(currentResultDictionary, x)).ToList();
                    results.AddRange(generiqueModelsNotInResult);
                }
                else
                {
                    var generiqueModelsFiltered = depenses.Where(FilterByAxeType(axe.Axe1)).ToList();
                    var generiqueModelsNotInResult = generiqueModelsFiltered.Where(x => !GetGeneriqueModelIsAlreadyInResult(currentResultDictionary, x)).ToList();
                    results.AddRange(generiqueModelsNotInResult);
                }
            }

            return results;
        }

        public Func<ExplorateurDepenseGeneriqueModel, bool> FilterByAxeType(ExplorateurAxe axe)
        {
            switch (axe.Type)
            {
                case AnalysisAxis.T1:
                    return x => x.Tache != null && x.Tache.Parent != null && x.Tache.Parent.Parent != null && x.Tache.Parent.Parent.Code == axe.Code;
                case AnalysisAxis.T2:
                    return x => x.Tache != null && x.Tache.Parent != null && x.Tache.Parent.Code == axe.Code;
                case AnalysisAxis.T3:
                    return x => x.Tache.Code == axe.Code;
                case AnalysisAxis.Chapitre:
                    return x => x.Ressource.SousChapitre.Chapitre.Code == axe.Code;
                case AnalysisAxis.SousChapitre:
                    return x => x.Ressource.SousChapitre.Code == axe.Code;
                case AnalysisAxis.Ressource:
                    return x => x.Ressource.Code == axe.Code;
                default:
                    return null;
            }

        }

        private bool GetGeneriqueModelIsAlreadyInResult(Dictionary<string, ExplorateurDepenseGeneriqueModel> currentResultDictionary,
                                                        ExplorateurDepenseGeneriqueModel currentGeneriqueModel)
        {
            if (currentResultDictionary.ContainsKey(currentGeneriqueModel.Id))
            {
                return true;
            }
            return false;
        }



        #region Fonctions privées

        /// <summary>
        /// Fusionne les deux arbres Ressources et Tâches
        /// </summary>
        /// <param name="order">Ordre des axes</param>
        /// <param name="tree1">Arbre 1</param>
        /// <param name="depenses">Liste des dépenses</param>
        /// <param name="axeSecondaire">Liste 2 axes analytiques</param>
        /// <returns>Arbre RessourceTache ou TacheRessource</returns>
        private List<ExplorateurAxe> Merging(int order, List<ExplorateurAxe> tree1, IEnumerable<ExplorateurDepenseGeneriqueModel> depenses, string[] axeSecondaire)
        {
            foreach (ExplorateurAxe level1 in tree1)
            {
                if (level1.SousExplorateurAxe != null)
                {
                    Merging(order, level1.SousExplorateurAxe.ToList(), depenses, axeSecondaire);
                }
                else
                {
                    if (order == AxeAnalytique.TacheRessource.ToIntValue())
                    {
                        level1.SousExplorateurAxe = CreateTree(depenses.Where(f => level1.AllT3Code.Contains(f.Tache?.Code)).ToList(), axeSecondaire, 0, parent: level1);
                    }
                    else if (order == AxeAnalytique.RessourceTache.ToIntValue())
                    {
                        level1.SousExplorateurAxe = CreateTree(depenses.Where(f => level1.AllRessourceCode.Contains(f.Ressource?.Code)).ToList(), axeSecondaire, 0, parent: level1);
                    }
                }
            }

            return tree1;
        }

        /// <summary>
        /// Créer l'arbre d'exploration
        /// </summary>
        /// <param name="depenses">liste des dépenses</param>
        /// <param name="axes">liste des axes choisis</param>
        /// <param name="i">index du tableau</param>
        /// <returns>Arbre d'exploration</returns>
        private List<ExplorateurAxe> CreateTree(IEnumerable<ExplorateurDepenseGeneriqueModel> depenses, string[] axes, int i, ExplorateurAxe parent)
        {
            List<ExplorateurAxe> explorateurAxes = new List<ExplorateurAxe>();

            if (i < axes.Length)
            {
                explorateurAxes = Grouping(depenses, axes[i]).ToList();

                foreach (ExplorateurAxe explorateurAxe in explorateurAxes)
                {
                    explorateurAxe.Parent = parent;
                    if (i + 1 < axes.Length)
                    {
                        explorateurAxe.SousExplorateurAxe = CreateTree(explorateurAxe.Depenses, axes, i + 1, parent: explorateurAxe);
                    }
                }
            }

            return explorateurAxes.OrderBy(x => x.Code, new CustomAlphanumericComparer()).ToList();
        }

        /// <summary>
        /// Regroupement des ExplorateurDepense selon l'axe
        /// </summary>
        /// <param name="depenses">Liste des dépenses</param>
        /// <param name="axe">Axe choisi</param>
        /// <returns>Liste d'explorateurAxe</returns>
        private IEnumerable<ExplorateurAxe> Grouping(IEnumerable<ExplorateurDepenseGeneriqueModel> depenses, string axe)
        {
            List<ExplorateurAxe> result = new List<ExplorateurAxe>();

            switch (axe)
            {
                case AnalysisAxis.T1:
                    {
                        var groupedByT1 = depenses.GroupBy(x => x.Tache?.Parent?.ParentId ?? -1).Select(w => new { TacheId = w.Key, Tache = w.FirstOrDefault()?.Tache?.Parent?.Parent, Items = w.ToList() }).ToList();

                        foreach (var d in groupedByT1)
                        {
                            ExplorateurAxe t1 = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.Tache.TacheId}".Trim(),
                                Code = d.Tache?.Code,
                                Libelle = d.Tache?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllT3Code = d.Items.Select(x => x.Tache?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(t1);
                        }

                        break;
                    }
                case AnalysisAxis.T2:
                    {
                        var groupedByT2 = depenses.GroupBy(x => x.Tache?.ParentId ?? -1).Select(w => new { TacheId = w.Key, Tache = w.FirstOrDefault()?.Tache?.Parent, Items = w.ToList() }).ToList();

                        foreach (var d in groupedByT2)
                        {
                            ExplorateurAxe t2 = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.Tache.TacheId}".Trim(),
                                Code = d.Tache?.Code,
                                Libelle = d.Tache?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllT3Code = d.Items.Select(x => x.Tache?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(t2);
                        }
                        break;
                    }
                case AnalysisAxis.T3:
                    {
                        var groupedByT3 = depenses.GroupBy(x => x.TacheId).Select(w => new { TacheId = w.Key, w.FirstOrDefault()?.Tache, Items = w.ToList() }).ToList();

                        foreach (var d in groupedByT3)
                        {
                            ExplorateurAxe t3 = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.Tache.TacheId}".Trim(),
                                Code = d.Tache?.Code,
                                Libelle = d.Tache?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllT3Code = d.Items.Select(x => x.Tache?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(t3);
                        }

                        break;
                    }
                case AnalysisAxis.Chapitre:
                    {
                        var groupedByChapitre = depenses.GroupBy(x => x.Ressource?.SousChapitre?.ChapitreId ?? -1).Select(w => new { ChapitreId = w.Key, w.FirstOrDefault()?.Ressource?.SousChapitre?.Chapitre, Items = w.ToList() }).ToList();

                        foreach (var d in groupedByChapitre)
                        {
                            ExplorateurAxe chapitre = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.ChapitreId}".Trim(),
                                Code = d.Chapitre?.Code,
                                Libelle = d.Chapitre?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllRessourceCode = d.Items.Select(x => x.Ressource?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(chapitre);
                        }

                        break;
                    }
                case AnalysisAxis.SousChapitre:
                    {
                        var groupedBySousChapitre = depenses.GroupBy(x => x.Ressource?.SousChapitreId ?? -1).Select(w => new { SousChapitreId = w.Key, w.FirstOrDefault()?.Ressource?.SousChapitre, Items = w.ToList() }).ToList();

                        foreach (var d in groupedBySousChapitre)
                        {
                            ExplorateurAxe sousChapitre = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.SousChapitreId}".Trim(),
                                Code = d.SousChapitre?.Code,
                                Libelle = d.SousChapitre?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllRessourceCode = d.Items.Select(x => x.Ressource?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(sousChapitre);
                        }

                        break;
                    }
                case AnalysisAxis.Ressource:
                    {
                        var groupedByRessource = depenses.GroupBy(x => x.RessourceId).Select(w => new { RessourceId = w.Key, w.FirstOrDefault()?.Ressource, Items = w.ToList() }).ToList();

                        foreach (var d in groupedByRessource)
                        {
                            ExplorateurAxe ressource = new ExplorateurAxe
                            {
                                Key = $"{axe} {d.RessourceId}".Trim(),
                                Code = d.Ressource?.Code,
                                Libelle = d.Ressource?.Libelle,
                                Type = axe,
                                MontantHT = d.Items.Sum(x => x.MontantHT),
                                Depenses = d.Items.ToList(),
                                AllRessourceCode = d.Items.Select(x => x.Ressource?.Code),
                                CodeChapitreParent = d.Items.Select(x => x.Ressource?.SousChapitre?.Chapitre?.Code).FirstOrDefault()
                            };
                            result.Add(ressource);
                        }
                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// Initialise un ExplorateurDepenseGenerique en fonction d'un depenseAchatEnt
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/></returns>
        private ExplorateurDepenseGeneriqueModel InitializeConvertDepenseAchat(DepenseAchatEnt depAchat, List<int?> idOfDepenseAchatWithFacture = null)
        {
            ExplorateurDepenseGeneriqueModel explorateurDepenseGenerique = new ExplorateurDepenseGeneriqueModel();
            AssociateExplorateurDepenseToDepenseAchat(explorateurDepenseGenerique, depAchat);
            explorateurDepenseGenerique.TypeDepense = SetDepenseType(depAchat);
            explorateurDepenseGenerique.SousTypeDepense = SetSousTypeDepense(depAchat);
            ProcessFacturation(explorateurDepenseGenerique, depAchat);

            if (explorateurDepenseGenerique.TypeDepense == Constantes.DepenseType.Reception)
            {
                explorateurDepenseGenerique.TacheRemplacable = (idOfDepenseAchatWithFacture == null || !idOfDepenseAchatWithFacture.Any()) ? explorateurDepenseGenerique.DepenseVisee
                    : !idOfDepenseAchatWithFacture.Contains(depAchat.DepenseId);
            }
            return explorateurDepenseGenerique;
        }

        /// <summary>
        /// Asssocie un ExplorateurDepenseGenerique à une DepenseAchatEnt
        /// </summary>
        /// <param name="explorateurDepenseGenerique"><see cref="ExplorateurDepenseGeneriqueModel"/></param>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="commentaire">Tableau de commentaire</param>
        private void AssociateExplorateurDepenseToDepenseAchat(ExplorateurDepenseGeneriqueModel explorateurDepenseGenerique, DepenseAchatEnt depAchat)
        {
            string[] defaultComments = { depAchat.NumeroBL, depAchat.Commentaire };

            explorateurDepenseGenerique.Ci = depAchat.CI;
            explorateurDepenseGenerique.Code = depAchat.CommandeLigne?.Commande?.Numero;
            explorateurDepenseGenerique.CommandeId = depAchat.CommandeLigne?.CommandeId;
            explorateurDepenseGenerique.Commentaire = string.Join(" - ", defaultComments.Where(x => !string.IsNullOrEmpty(x)));
            explorateurDepenseGenerique.RessourceId = depAchat.RessourceId ?? 0;
            explorateurDepenseGenerique.Ressource = depAchat.Ressource;
            explorateurDepenseGenerique.TacheId = depAchat.TacheId.Value;
            explorateurDepenseGenerique.Tache = depAchat.Tache;
            explorateurDepenseGenerique.NatureId = depAchat.Nature?.NatureId ?? 0;
            explorateurDepenseGenerique.Nature = depAchat.Nature;
            explorateurDepenseGenerique.UniteId = depAchat.UniteId ?? 0;
            explorateurDepenseGenerique.Unite = depAchat.Unite;
            explorateurDepenseGenerique.DeviseId = depAchat.DeviseId ?? 0;
            explorateurDepenseGenerique.Devise = depAchat.Devise;
            explorateurDepenseGenerique.PUHT = depAchat.PUHT;
            explorateurDepenseGenerique.MontantHT = depAchat.PUHT * depAchat.Quantite;
            explorateurDepenseGenerique.Libelle1 = depAchat.Fournisseur?.Libelle;
            explorateurDepenseGenerique.Libelle2 = depAchat.Libelle;
            explorateurDepenseGenerique.DateDepense = depAchat.Date.Value;
            explorateurDepenseGenerique.Periode = depAchat.DateComptable ?? DateTime.UtcNow;
            explorateurDepenseGenerique.FournisseurId = depAchat.FournisseurId;
            explorateurDepenseGenerique.Id = string.Concat(explorateurDepenseGenerique.TypeDepense, depAchat.DepenseId.ToString());
            explorateurDepenseGenerique.DateRapprochement = depAchat.DateOperation;
            explorateurDepenseGenerique.Quantite = depAchat.AfficherQuantite ? depAchat.Quantite : default(decimal?);
            explorateurDepenseGenerique.DepenseVisee = depAchat.DateVisaReception.HasValue;
            explorateurDepenseGenerique.DepenseId = depAchat.DepenseId;
            explorateurDepenseGenerique.GroupeRemplacementTacheId = depAchat.GroupeRemplacementTacheId ?? 0;
            explorateurDepenseGenerique.SoldeFar = depAchat.SoldeFar;
            explorateurDepenseGenerique.TacheRemplacable = true;
            explorateurDepenseGenerique.AgenceId = depAchat.CommandeLigne?.Commande.AgenceId;
            explorateurDepenseGenerique.MontantHtInitial = depAchat.MontantHtInitial;
            explorateurDepenseGenerique.IsEnergie = depAchat.CommandeLigne?.Commande.IsEnergie ?? false;
            explorateurDepenseGenerique.TypeRessourceId = depAchat.Ressource?.TypeRessourceId ?? 0;
        }

        /// <summary>
        /// Convertit une dépense achat vers ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="depAchat">Dépense achat</param>
        /// <returns>ExplorateurDepenseGenerique</returns>
        private ExplorateurDepenseGeneriqueModel ConvertForExportDepenseAchat(DepenseAchatEnt depAchat)
        {
            ExplorateurDepenseGeneriqueModel explorateurDepenseGenerique = InitializeConvertDepenseAchat(depAchat);
            explorateurDepenseGenerique.RemplacementTaches = depAchat.RemplacementTaches;
            return explorateurDepenseGenerique;
        }

        private void ProcessFacturation(ExplorateurDepenseGeneriqueModel expDep, DepenseAchatEnt depAchat)
        {
            FacturationEnt fact = null;

            if (depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.Facture.ToIntValue()
                    || depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue())
                {
                    fact = depAchat.FacturationsFacture.FirstOrDefault();
                }
                else if (depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue()
                         || depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue()
                         || depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    fact = depAchat.FacturationsFactureEcart.FirstOrDefault();

                    // RG_3657_011 : si 'PU A Afficher' = Non ET [EcartPU]<>0 dans la ligne Facturation telle que :
                    // [DepenseAchatFactureEcartId] = ID de la dépense(si cette ligne existe) : afficher[EcartPU] de la ligne de Facturation
                    if (!depAchat.AfficherPuHt && fact?.EcartPu.HasValue == true && fact?.EcartPu.Value != 0)
                    {
                        expDep.PUHT = fact?.EcartPu ?? default(decimal?);
                    }
                }
            }

            string[] numeroFacture = new string[] { fact?.NumeroFactureFMFI, fact?.NumeroFactureFournisseur };

            expDep.DateFacture = fact?.DatePieceSap;
            expDep.NumeroFacture = string.Join(" - ", numeroFacture.Where(x => !string.IsNullOrEmpty(x)));
            expDep.MontantFacture = fact?.MontantTotalHT;
        }

        /// <summary>
        /// Determine le type de dépense de la ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <returns>Le type de dépenses</returns>
        protected string SetDepenseType(DepenseAchatEnt depAchat)
        {
            if (depAchat != null && depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.Reception.ToIntValue())
                {
                    return Constantes.DepenseType.Reception;
                }
                else if (depAchat.DepenseType.Code == DepenseType.Facture.ToIntValue() || depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue() || depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue() || depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue() || depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    return Constantes.DepenseType.Facturation;
                }
                else if (depAchat.DepenseType.Code == DepenseType.AjustementFar.ToIntValue())
                {
                    return Constantes.DepenseType.AjustementFar;
                }
                else if (depAchat.DepenseType.Code == DepenseType.ExtourneFar.ToIntValue())
                {
                    return Constantes.DepenseType.ExtourneFar;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Determine le sous type de dépense de la ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <returns>Le sous type de dépenses</returns>
        protected string SetSousTypeDepense(DepenseAchatEnt depAchat)
        {
            if (depAchat != null && depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue())
                {
                    return Constantes.DepenseSousType.Ecart;
                }
                else if (depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue())
                {
                    return Constantes.DepenseSousType.NonCommandee;
                }
                else if (depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue() || depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    return Constantes.DepenseSousType.Avoir;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Convertit une ValorisationEnt vers ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="valo">Dépense valorisation</param>            
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>ExplorateurDepenseGenerique</returns>
        private ExplorateurDepenseGeneriqueModel ConvertValorisation(ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            ExplorateurDepenseGeneriqueModel expDep = new ExplorateurDepenseGeneriqueModel()
            {
                Ci = valo.CI,
                Code = valo.PersonnelId.HasValue ? valo.Personnel.Matricule : valo.Materiel.Code,
                RessourceId = valo.ReferentielEtendu?.RessourceId ?? 0,
                Ressource = valo.ReferentielEtendu.Ressource,
                TacheId = valo.TacheId,
                Tache = valo.Tache,
                NatureId = valo.ReferentielEtendu.NatureId ?? 0,
                Nature = valo.ReferentielEtendu.Nature,
                UniteId = valo.UniteId,
                Unite = valo.Unite,
                DeviseId = valo.DeviseId,
                Devise = valo.Devise,
                TypeDepense = Constantes.DepenseType.Valorisation,
                PUHT = valo.PUHT,
                Quantite = valo.Quantite,
                MontantHT = valo.Montant,
                Libelle1 = valo.PersonnelId.HasValue ? valo.Personnel.PrenomNom : valo.Materiel.Libelle,
                Libelle2 = valo.PersonnelId.HasValue ? valo.Personnel.Societe.Code : valo.Materiel.Societe.Code,
                DateDepense = valo.Date,
                Periode = valo.Date,
                DepenseId = valo.ValorisationId,
                GroupeRemplacementTacheId = valo.GroupeRemplacementTacheId ?? 0,
                Personnel = valo.Personnel,
                TypeRessourceId = valo.ReferentielEtendu?.Ressource?.TypeRessourceId ?? 0
            };

            expDep.Id = string.Concat(expDep.TypeDepense, valo.ValorisationId.ToString());

            //RG_3691_012
            // Remplaçable si elle appartient à une période qui a déjà 
            // été transférée en FAR ou clôturée pour leur CI      
            DatesClotureComptableEnt period = datesClotureComptables.Find(x =>
                expDep.Periode.Month == x.Mois
                && expDep.Periode.Year == x.Annee
                && (x.DateCloture.HasValue || x.DateTransfertFAR.HasValue));
            expDep.TacheRemplacable = period != null;
            expDep = SetExplorateurDepenseForInterim(expDep, valo, periodeDebut, periodeFin);

            // Ajout numéro rapport sur le second libellé
            if (!string.IsNullOrEmpty(expDep.Libelle2))
            {
                expDep.Libelle2 = string.Concat(expDep.Libelle2, " - ", ExplorateurDepenseResources.Rapport, " #", valo.RapportId);
            }

            return expDep;
        }

        /// <summary>
        /// Convertit une ValorisationEnt vers ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="valo">Dépense valorisation</param>            
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>ExplorateurDepenseGenerique</returns>
        private ExplorateurDepenseGeneriqueModel ConvertForExportValorisation(ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            ExplorateurDepenseGeneriqueModel expDep = ConvertValorisation(valo, periodeDebut, periodeFin, datesClotureComptables);
            expDep.RemplacementTaches = valo.RemplacementTaches;
            return expDep;
        }

        /// <summary>
        /// Sets the explorateur depense for interim.
        /// </summary>
        /// <param name="expDep">The exp dep.</param>
        /// <param name="valo">The valo.</param>        
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <returns>ExplorateurDepense</returns>
        private ExplorateurDepenseGeneriqueModel SetExplorateurDepenseForInterim(ExplorateurDepenseGeneriqueModel expDep, ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin)
        {
            List<string> societeCode = new List<string>();
            List<string> fournisseurCode = new List<string>();
            if (valo.Personnel != null)
            {
                List<ContratInterimaireEnt> contrats = valo.Personnel.ContratInterimaires.ToList();
                societeCode = GetSocieteCodeFromContrats(contrats, periodeDebut, periodeFin, expDep.Ci.CiId);
                fournisseurCode = GetFournisseurCodeFromContrats(contrats, periodeDebut, periodeFin);
            }

            if (valo.Materiel != null)
            {
                expDep.Libelle2 = valo.Materiel.Societe?.Code;
            }
            else if (valo.Personnel?.IsInterimaire == false)
            {
                expDep.Libelle2 = valo.Personnel.Societe?.Code;
            }
            else
            {
                expDep.Libelle2 = valo.Personnel?.Societe?.Code;
                societeCode.ForEach(x => expDep.Libelle2 += (string.Format("-{0}", x)));
            }

            if (valo.Personnel?.IsInterimaire == true)
            {
                fournisseurCode.ForEach(x => expDep.Commentaire += fournisseurCode.Last() == x ? string.Format("{0}", x) : string.Format("{0}-", x));
            }

            return expDep;
        }

        /// <summary>
        /// Gets the societe code from contrats.
        /// </summary>
        /// <param name="contratsList">The contrats list.</param>
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <param name="ciId">L'id du CI</param>
        /// <returns>List of society's code</returns>
        public List<string> GetSocieteCodeFromContrats(List<ContratInterimaireEnt> contratsList, DateTime? periodeDebut, DateTime? periodeFin, int ciId)
        {
            IEnumerable<ContratInterimaireEnt> contratWithSameCi = contratsList.Where(x => x.CiId == ciId).ToList();

            return contratWithSameCi.Where(x => x.DateFin.AddDays(x.Souplesse) <= periodeFin.Value
                    && periodeDebut != null ? x.DateDebut >= periodeDebut.Value : x.DateDebut != default(DateTime))
                    .Select(m => m.Societe?.Code).Distinct().ToList();
        }

        /// <summary>
        /// Gets the fournisseur code from contrats.
        /// </summary>
        /// <param name="contratsList">The contrats list.</param>
        /// <param name="periodeDebut">The periode debut.</param>
        /// <param name="periodeFin">The periode fin.</param>
        /// <returns>List of provider code</returns>
        public List<string> GetFournisseurCodeFromContrats(List<ContratInterimaireEnt> contratsList, DateTime? periodeDebut, DateTime? periodeFin)
        {
            return contratsList.Where(x =>
                    x.DateFin.AddDays(x.Souplesse) >= periodeFin.Value.AddDays(-14)
                    && periodeDebut != null ? x.DateDebut >= periodeDebut : x.DateDebut != default(DateTime))
                    .Select(m => m.Fournisseur?.Code)
                    .Distinct()
                    .ToList();
        }

        /// <summary>
        /// Convertit une OperationDiverseEnt vers ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="od">Dépense Opération diverse</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>ExplorateurDepenseGenerique</returns>
        private ExplorateurDepenseGeneriqueModel ConvertOperationDiverse(OperationDiverseEnt od, List<DatesClotureComptableEnt> datesClotureComptables, bool useOdLibelleCourt)
        {
            ExplorateurDepenseGeneriqueModel expDep = new ExplorateurDepenseGeneriqueModel
            {
                Ci = od.CI,
                Commentaire = od.Commentaire,
                TacheId = od.TacheId,
                Tache = od.Tache,
                UniteId = od.UniteId,
                Unite = od.Unite,
                DeviseId = od.DeviseId,
                Devise = od.Devise,
                TypeDepense = useOdLibelleCourt ? od.FamilleOperationDiverse.LibelleCourt ?? Constantes.DepenseType.OD : Constantes.DepenseType.OD,
                PUHT = od.PUHT,
                Quantite = od.Quantite,
                MontantHT = od.Montant,
                Libelle1 = od.Libelle,
                DateDepense = od.DateComptable.Value,
                Periode = od.DateComptable.Value,
                DepenseId = od.OperationDiverseId,
                RessourceId = od.RessourceId,
                Ressource = od.Ressource,
                GroupeRemplacementTacheId = od.GroupeRemplacementTacheId ?? 0,
                FamilleOperationDiverseId = od.FamilleOperationDiverseId,
                FamilleOperationDiverse = od.FamilleOperationDiverse,
                TypeRessourceId = od.Ressource?.TypeRessourceId ?? 0,
                TypeOd = Constantes.DepenseType.OD
            };

            string operationDiverseId = od.OperationDiverseId.ToString();

            if (useOdLibelleCourt)
            {
                expDep.Id = string.Concat(Constantes.DepenseType.OD, operationDiverseId);
            }
            else
            {
                expDep.Id = string.Concat(expDep.TypeDepense, operationDiverseId);
            }

            //RG_3691_012
            // Remplaçable si elle appartient à une période qui est clôturée pour leur CI        
            DatesClotureComptableEnt period = datesClotureComptables?.Find(x => expDep.Periode.Month == x.Mois && expDep.Periode.Year == x.Annee && x.DateCloture.HasValue);
            if (period != null)
            {
                expDep.TacheRemplacable = true;
            }

            return expDep;
        }

        /// <summary>
        /// Convertit une OperationDiverseEnt vers ExplorateurDepenseGenerique
        /// </summary>
        /// <param name="od">Dépense Opération diverse</param>
        /// <param name="datesClotureComptables">Période de cloture du CI associé</param>
        /// <returns>ExplorateurDepenseGenerique</returns>
        private ExplorateurDepenseGeneriqueModel ConvertForExportOperationDiverse(OperationDiverseEnt od, List<DatesClotureComptableEnt> datesClotureComptables, bool useOdLibelleCourt)
        {
            ExplorateurDepenseGeneriqueModel expDep = ConvertOperationDiverse(od, datesClotureComptables, useOdLibelleCourt);
            expDep.RemplacementTaches = od.RemplacementTaches;
            return expDep;
        }

        #endregion
    }
}
