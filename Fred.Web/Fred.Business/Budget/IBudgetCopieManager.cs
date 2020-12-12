using System.Threading.Tasks;
using Fred.Business.Budget.BudgetManager.Dtao;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    public interface IBudgetCopieManager : IManager<BudgetEnt>
    {
        Task<BudgetEnt> CopierBudgetDansMemeCiAsync(int budgetACopierId, int utilisateurConnecteId, bool useBibliothequeDesPrix);

        Task<CopierBudgetDto.Result> CopierBudgetAsync(CopierBudgetDto.Request request);
    }
}
