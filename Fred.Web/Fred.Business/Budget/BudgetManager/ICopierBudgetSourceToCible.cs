using System.Threading.Tasks;
using Fred.Business.Budget.BudgetManager.Dtao;

namespace Fred.Business.Budget.BudgetManager
{
    public interface ICopierBudgetSourceToCible
    {
        Task<CopierBudgetDto.Result> CopierAsync(CopierBudgetDto.Request request);

        bool CheckPlanDeTacheIdentiques(int ciId1, int ciId2);

    }
}