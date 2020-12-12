
using System.Collections.Generic;
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données des workflows du budget.
    /// </summary>
    public interface IBudgetWorkflowRepository : IRepository<BudgetWorkflowEnt>
    {
        /// <summary>
        /// Retourne la liste des workflows concernant un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La liste des workflows d'un budget</returns>
        IEnumerable<BudgetWorkflowEnt> GetList(int budgetId);


        /// <summary>
        /// Retourne la derniere entrée de verrouillage dans la table workflows concernant un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>le dernier workflow de verrouillage d'un budget</returns>
        BudgetWorkflowEnt GetLastLockWorkflow(int budgetId);
    }
}
