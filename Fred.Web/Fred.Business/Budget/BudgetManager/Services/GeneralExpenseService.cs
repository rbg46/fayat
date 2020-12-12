using System.Collections.Generic;
using System.Linq;
using Fred.Business.Budget.Helpers;
using Fred.Business.Budget.Recette;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Business.Budget.BudgetManager.Services
{
    public class GeneralExpenseService : IGeneralExpenseService
    {
        private readonly IBudgetManager budgetManager;
        private readonly IAvancementRecetteLoader avancementRecetteLoader;

        public GeneralExpenseService(IBudgetManager budgetManager, IAvancementRecetteLoader avancementRecetteLoader)
        {
            this.budgetManager = budgetManager;
            this.avancementRecetteLoader = avancementRecetteLoader;
        }
        public List<GeneralExpense> GetGeneralExpenses(List<int> ciIds, int periode)
        {
            var result = new List<GeneralExpense>();

            if (ciIds == null)
            {
                return result;
            }

            foreach (int ciId in ciIds)
            {
                int budgetId = budgetManager.GetIdBudgetEnApplication(ciId);
                AvancementRecetteLoadModel avancementRecetteLoadModel = avancementRecetteLoader.Load(budgetId, periode);

                var previousPeriod = PeriodeHelper.GetPreviousPeriod(periode).Value;
                AvancementRecetteLoadModel avancementRecetteLoadModelPrevious = avancementRecetteLoader.Load(budgetId, previousPeriod);

                var generalExpense = new GeneralExpense();
                generalExpense.Prod = GetProd(avancementRecetteLoadModel);
                generalExpense.Pourcentage = GetPourcentage(avancementRecetteLoadModel);
                generalExpense.Budget = GetBudget(avancementRecetteLoadModel); // calcul avec prod
                generalExpense.RecetteCumul = GetRecetteCumul(avancementRecetteLoadModel);
                generalExpense.RecetteCumulPrevious = GetRecetteCumul(avancementRecetteLoadModelPrevious);
                generalExpense.Recette = GetRecette(generalExpense.RecetteCumul, generalExpense.RecetteCumulPrevious);
                generalExpense.Pfa = GetPfa(avancementRecetteLoadModel);
                result.Add(generalExpense);
            }

            return result;
        }

        private decimal GetRecette(decimal recetteCumul, decimal recetteCumulPrevious)
        {
            return recetteCumul - recetteCumulPrevious;
        }

        private decimal GetPourcentage(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            decimal result = 0;
            if (TotalRecetteIsNullOrEqualZero(avancementRecetteLoadModel))
            {
                return result;// evite la division par zero
            }

            var prod = GetProd(avancementRecetteLoadModel); // totalRecette = Prod

            result = (((avancementRecetteLoadModel.TauxFraisGeneraux / 100) * prod) + avancementRecetteLoadModel.AjustementFraisGeneraux) / prod;

            return result;
        }

        private decimal GetBudget(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            var prod = GetProd(avancementRecetteLoadModel); // totalRecette = Prod

            return (avancementRecetteLoadModel.TauxFraisGeneraux * prod / 100) + avancementRecetteLoadModel.AjustementFraisGeneraux;
        }

        private decimal GetRecetteCumul(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            // TotalAvancementFacture = ProdCumul
            return ((avancementRecetteLoadModel.AvancementTauxFraisGeneraux / 100) * avancementRecetteLoadModel.TotalAvancementFacture) + avancementRecetteLoadModel.AvancementAjustementFraisGeneraux;
        }
        private decimal GetPfa(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            // TotalPFA = ProdPFA

            return (avancementRecetteLoadModel.TauxFraisGenerauxPFA / 100) * avancementRecetteLoadModel.TotalPFA + avancementRecetteLoadModel.AjustementFraisGenerauxPFA;
        }

        private decimal GetProd(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            if (avancementRecetteLoadModel.BudgetRecette == null || avancementRecetteLoadModel.BudgetRecette.TotalRecette == null)
            {
                return 0;
            }
            return avancementRecetteLoadModel.BudgetRecette.TotalRecette.Value;
        }

        private static bool TotalRecetteIsNullOrEqualZero(AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            return avancementRecetteLoadModel.BudgetRecette == null || avancementRecetteLoadModel.BudgetRecette.TotalRecette == null || avancementRecetteLoadModel.BudgetRecette.TotalRecette == 0;
        }


        public GeneralExpense GetTotalGeneralExpenses(List<GeneralExpense> generalExpenses)
        {
            var generalExpense = new GeneralExpense();
            generalExpense.Pourcentage = GetPourcentage(generalExpenses);
            generalExpense.Budget = GetBudget(generalExpenses); // calcul avec prod
            generalExpense.RecetteCumul = GetRecetteCumul(generalExpenses);
            generalExpense.RecetteCumulPrevious = GetRecetteCumulPrevious(generalExpenses);
            generalExpense.Recette = GetRecette(generalExpenses);
            generalExpense.Pfa = GetPfa(generalExpenses);
            return generalExpense;
        }

        private decimal GetPfa(List<GeneralExpense> generalExpenses)
        {
            return generalExpenses.Sum(x => x.Pfa);
        }

        private decimal GetRecette(List<GeneralExpense> generalExpenses)
        {
            return GetRecetteCumul(generalExpenses) - GetRecetteCumulPrevious(generalExpenses);
        }

        private decimal GetRecetteCumulPrevious(List<GeneralExpense> generalExpenses)
        {
            return generalExpenses.Sum(x => x.RecetteCumulPrevious);
        }

        private decimal GetRecetteCumul(List<GeneralExpense> generalExpenses)
        {
            return generalExpenses.Sum(x => x.RecetteCumul);
        }

        private decimal GetPourcentage(List<GeneralExpense> generalExpenses)
        {
            var sumOfProd = generalExpenses.Sum(x => x.Prod);
            if (sumOfProd == 0)
            {
                return 0;
            }
            return GetBudget(generalExpenses) / generalExpenses.Sum(x => x.Prod);
        }

        private decimal GetBudget(List<GeneralExpense> generalExpenses)
        {
            return generalExpenses.Sum(x => x.Budget);
        }
    }
}
