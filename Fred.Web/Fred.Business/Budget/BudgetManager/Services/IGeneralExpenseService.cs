using System.Collections.Generic;

namespace Fred.Business.Budget.BudgetManager.Services
{
    public interface IGeneralExpenseService
    {
        List<GeneralExpense> GetGeneralExpenses(List<int> ciIds, int periode);
        GeneralExpense GetTotalGeneralExpenses(List<GeneralExpense> generalExpenses);
    }
}