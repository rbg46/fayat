using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Gestionnaire du budget Etat.
    /// </summary>
    public class BudgetEtatManager : Manager<BudgetEtatEnt, IBudgetEtatRepository>, IBudgetEtatManager
    {
        public BudgetEtatManager(IUnitOfWork uow, IBudgetEtatRepository budgetEtatRepository)
          : base(uow, budgetEtatRepository)
        {
        }

        /// <inheritdoc />
        public BudgetEtatEnt GetByCode(string code)
        {
            return Repository.GetByCode(code);
        }
    }
}
