using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Framework.Linq;
using Fred.Web.Shared.Models.Budget.Details;

namespace Fred.Business.Budget.Details
{
    /// <summary>
    /// Classe de methods d'extension pour BudgetDetailsExportExcelFeature
    /// </summary>
    public static class BudgetDetailExportExcelExtension
    {
        private static readonly int NiveauT1 = 1;
        private static readonly int NiveauT2 = 2;
        private static readonly int NiveauT3 = 3;
        private static readonly int NiveauT4 = 4;

        /// <summary>
        /// Permet d'ajouter les tâches T1
        /// </summary>
        /// <param name="result">liste Detail Export Excel</param>
        /// <param name="tacheEnts">Liste tâches T1</param>
        /// <param name="budgetT4s">Liste des budgets</param>
        /// <param name="disabledTasksDisplayed">flag affichage tâches inactives</param>
        /// <param name="niveauxVisibles">Liste des niveaux à affficher</param>
        public static void AddT1Nodes(this List<BudgetDetailsExportExcelEditableModel> result, List<TacheEnt> tacheEnts, IEnumerable<BudgetT4Ent> budgetT4s, bool disabledTasksDisplayed, List<int> niveauxVisibles)
        {
            foreach (var tacheEnt1 in tacheEnts.OrderBy(e => e.Code).ToList())
            {
                var montant = budgetT4s.Where(x => x.T3.Parent.Parent.TacheId == tacheEnt1.TacheId).Sum(x => x.MontantT4);
                if ((montant == 0) && !disabledTasksDisplayed && !tacheEnt1.Active)
                {
                    // on n'affiche pas les taches inactives
                    continue;
                }
                if (niveauxVisibles.Contains(NiveauT1))
                {
                    result.Add(new BudgetDetailsExportExcelEditableModel
                    {
                        CodeTache = $"T1-{tacheEnt1.Code}",
                        LibelleTache = tacheEnt1.Libelle,
                        MontantTache = montant
                    });
                }
                result.AddT2Nodes(budgetT4s, tacheEnt1, disabledTasksDisplayed, niveauxVisibles);
            }
        }
        /// <summary>
        /// Permet d'ajouter les tâches T2 à la tache T1
        /// </summary>
        /// <param name="result">liste Detail Export Excel</param>
        /// <param name="budgetT4s">liste des budgets</param>
        /// <param name="tacheEnt1">Tache T1</param>
        /// <param name="disabledTasksDisplayed">flag affichage tâches inactives</param>
        /// <param name="niveauxVisibles">Liste des niveaux à affficher</param>
        public static void AddT2Nodes(this List<BudgetDetailsExportExcelEditableModel> result, IEnumerable<BudgetT4Ent> budgetT4s, TacheEnt tacheEnt1, bool disabledTasksDisplayed, List<int> niveauxVisibles)
        {
            if (tacheEnt1.TachesEnfants != null)
            {
                foreach (var tacheEnt2 in tacheEnt1.TachesEnfants.OrderBy(e => e.Code).ToList())
                {
                    var montant = budgetT4s.Where(x => x.T3.Parent.TacheId == tacheEnt2.TacheId).Sum(x => x.MontantT4);
                    if ((montant == 0) && !disabledTasksDisplayed && !tacheEnt2.Active)
                    {
                        // on n'affiche pas les taches inactives
                        continue;
                    }
                    if (niveauxVisibles.Contains(NiveauT2))
                    {
                        result.Add(new BudgetDetailsExportExcelEditableModel
                        {
                            CodeTache = $"T2-{tacheEnt2.Code}",
                            LibelleTache = tacheEnt2.Libelle,
                            MontantTache = montant
                        });
                    }
                    result.AddT3Nodes(budgetT4s, tacheEnt2, disabledTasksDisplayed, niveauxVisibles);
                }
            }
        }

        /// <summary>
        /// PErmet d'ajouter les taches T3 à la tâche T2
        /// </summary>
        /// <param name="result">liste Detail Export Excel</param>
        /// <param name="budgetT4s">liste des budgets</param>
        /// <param name="tacheEnt2">Tâche T2</param>
        /// <param name="disabledTasksDisplayed">flag affichage tâches inactives</param>
        /// <param name="niveauxVisibles">Liste des niveaux à affficher</param>
        public static void AddT3Nodes(this List<BudgetDetailsExportExcelEditableModel> result, IEnumerable<BudgetT4Ent> budgetT4s, TacheEnt tacheEnt2, bool disabledTasksDisplayed, List<int> niveauxVisibles)
        {
            if (tacheEnt2.TachesEnfants != null)
            {
                foreach (var tacheEnt3 in tacheEnt2.TachesEnfants.OrderBy(e => e.Code).ToList())
                {
                    var montant = budgetT4s.Where(x => x.T3.TacheId == tacheEnt3.TacheId).Sum(x => x.MontantT4);
                    if ((montant == 0) && !disabledTasksDisplayed && !tacheEnt3.Active)
                    {
                        // on n'affiche pas les taches inactives
                        continue;
                    }
                    if (niveauxVisibles.Contains(NiveauT3))
                    {
                        result.Add(new BudgetDetailsExportExcelEditableModel
                        {
                            CodeTache = $"T3-{tacheEnt3.Code}",
                            LibelleTache = tacheEnt3.Libelle,
                            MontantTache = montant
                        });
                    }
                    result.AddT4Nodes(budgetT4s, tacheEnt3, disabledTasksDisplayed, niveauxVisibles);
                }
            }
        }
        /// <summary>
        /// PErmet d'ajouter les taches T4 à la tâche T3
        /// </summary>
        /// <param name="result">liste Detail Export Excel</param>
        /// <param name="budgetT4s">liste des budgets</param>
        /// <param name="tacheEnt3">Tâche T3</param>
        /// <param name="disabledTasksDisplayed">flag affichage tâches inactives</param>
        /// <param name="niveauxVisibles">Liste des niveaux à affficher</param>
        public static void AddT4Nodes(this List<BudgetDetailsExportExcelEditableModel> result, IEnumerable<BudgetT4Ent> budgetT4s, TacheEnt tacheEnt3, bool disabledTasksDisplayed, List<int> niveauxVisibles)
        {
            if (tacheEnt3.TachesEnfants != null)
            {
                foreach (var tacheEnt4 in tacheEnt3.TachesEnfants.OrderBy(e => e.Code).ToList())
                {
                    // recherche d'une tache de budget T4 correspondante 
                    var budgetT4 = budgetT4s.SingleOrDefault(x => x.T4Id == tacheEnt4.TacheId);
                    if (budgetT4 == null)
                    {
                        // Pas de T4 correspondante, passe au suivant
                        continue;
                    }

                    if ((budgetT4.MontantT4 == 0) && !disabledTasksDisplayed && !tacheEnt4.Active)
                    {
                        // on n'affiche pas les taches inactives
                        continue;
                    }

                    var tacheModel = new BudgetDetailsExportExcelEditableModel
                    {
                        CodeTache = $"T4-{tacheEnt4.Code}",
                        LibelleTache = tacheEnt4.Libelle,
                        UniteTache = budgetT4.Unite?.Code,
                        PUTache = budgetT4.PU,
                        QuantiteTache = budgetT4.QuantiteARealiser,
                        MontantTache = budgetT4.MontantT4,
                    };
                    if (niveauxVisibles.Contains(NiveauT4))
                    {
                        result.Add(tacheModel);
                        var vueSD = budgetT4.VueSD == 1;
                        budgetT4.BudgetSousDetails.ForEach(sd =>
                            result.Add(GetBudgetDetailsExportExcelEditableModel(sd, vueSD))
                        );
                    }
                }
            }
        }

        private static BudgetDetailsExportExcelEditableModel GetBudgetDetailsExportExcelEditableModel(BudgetSousDetailEnt sd, bool vueSD)
        {
            return new BudgetDetailsExportExcelEditableModel
            {
                Chapitre = sd.Ressource.SousChapitre.Chapitre.Libelle,
                Ressource = sd.Ressource.Libelle,
                Commentaire = sd.Commentaire,
                Unite = sd.Unite?.Code,
                QuantiteRessourceT4 = sd.Quantite,
                PURessourceT4 = sd.PU,
                MontantRessourceT4 = sd.PU * sd.Quantite,
                QuantiteRessourceSD = vueSD ? sd.QuantiteSD : null,
                PURessourceSD = vueSD ? sd.PU : null,
                MontantRessourceSD = vueSD ? (sd.QuantiteSD ?? 0) * sd.PU : null
            };
        }
    }
}
