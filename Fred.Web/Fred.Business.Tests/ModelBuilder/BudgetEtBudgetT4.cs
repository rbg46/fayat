using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Budget;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;

namespace Fred.Business.Tests.ModelBuilder
{
    /// <summary>
    /// Classe proposant des fonctions de création d'entités pour les tests
    /// Faire très attention en modifiant les valeurs de ces entités car les tests peuvent reposer la dessus
    /// e.g Rajouter un sous détail à la fonction GetFakeT4();
    /// </summary>
    internal static class BudgetEtBudgetT4
    {
        /// <summary>
        /// Retourne un budget en application sur la version 1.0
        /// Ce budget ne contient AUCUN T4
        /// </summary>
        /// <returns></returns>
        public static BudgetEnt GetFakeBudgetEnApplication()
        {
            var fakeCi = GetFakeCi();
            var fakeBudgetEtatEnApplication = GetFakeBudgetEtatEnApplication();
            return new BudgetEnt
            {
                BudgetId = 1,
                Ci = fakeCi,
                CiId = fakeCi.CiId,
                BudgetEtat = fakeBudgetEtatEnApplication,
                BudgetEtatId = fakeBudgetEtatEnApplication.BudgetEtatId,
                PeriodeDebut = 201810,
                Version = "1.0"
            };
        }

        /// <summary>
        /// Faux budget T4 ne contenant qu'un seul sous détail, avec comme budget Id le FakeBudgetEnApplication
        /// </summary>
        public static BudgetT4Ent GetFakeBudgetT4()
        {
            var fakeT4 = GetFakeTache4();
            var fakeBudgetEnApplication = GetFakeBudgetEnApplication();
            var fakeRessource = GetFakeRessource();
            var fakeUnite = GetFakeUnite();

            var budgetT4 = new BudgetT4Ent()
            {
                QuantiteARealiser = 2,
                MontantT4 = 50,
                PU = 25,
                T4 = fakeT4,
                T4Id = fakeT4.TacheId,
                Budget = fakeBudgetEnApplication,
                BudgetId = fakeBudgetEnApplication.BudgetId,
                BudgetT4Id = 1,
                TypeAvancement = (int)TypeAvancementBudget.Pourcentage,
                Unite = fakeUnite,
                UniteId = fakeUnite.UniteId,
                BudgetSousDetails = new List<BudgetSousDetailEnt>()
                        {
                            new BudgetSousDetailEnt()
                            {
                                Quantite = 5,
                                PU = 10,
                                Montant = 50,
                                Ressource = fakeRessource,
                                RessourceId = fakeRessource.RessourceId,
                                Unite = fakeUnite,
                                UniteId = fakeUnite.UniteId,
                                BudgetSousDetailId = 1
                            }
                        }
            };

            budgetT4.BudgetSousDetails.First().BudgetT4 = budgetT4;
            budgetT4.BudgetSousDetails.First().BudgetT4Id = budgetT4.BudgetT4Id;


            return budgetT4;

        }
    }
}
