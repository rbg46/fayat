
using System.Collections.Generic;
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Référentiel de données pour les tâches budget.
    /// </summary>
    public interface IBudgetTacheRepository : IRepository<BudgetTacheEnt>
    {
        /// <summary>
        /// Récupère un budget tache.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tacheId">Identifiant de la tâche</param>
        /// <returns>Le budget tache ou null s'il n'existe pas.</returns>
        BudgetTacheEnt GetByBudgetIdAndTacheId(int budgetId, int tacheId);

        /// <summary>
        /// Récupère les informations sur les tâches pour un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Les informations sur les tâches.</returns>
        IEnumerable<BudgetTacheEnt> Get(int budgetId);
    }
}
