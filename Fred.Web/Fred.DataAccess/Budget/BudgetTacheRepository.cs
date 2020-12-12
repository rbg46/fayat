using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    /// Référentiel de données pour budget T4.
    /// </summary>
    public class BudgetTacheRepository : FredRepository<BudgetTacheEnt>, IBudgetTacheRepository
    {
        public BudgetTacheRepository(FredDbContext context)
          : base(context)
        { }

        /// <inheritdoc />
        public BudgetTacheEnt GetByBudgetIdAndTacheId(int budgetId, int tacheId)
        {
            return this.Context.BudgetTaches.FirstOrDefault(x => x.BudgetId == budgetId && x.TacheId == tacheId);
        }

        /// <summary>
        /// Récupère les informations sur les tâches pour un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Les informations sur les tâches.</returns>
        public IEnumerable<BudgetTacheEnt> Get(int budgetId)
        {
            return Context.BudgetTaches.Where(bt => bt.BudgetId == budgetId);
        }
    }
}
