using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface des états budget.
    /// </summary>
    public interface IBudgetEtatManager : IManager<BudgetEtatEnt>
    {
        /// <summary>
        /// Retourne l'état d'un budget
        /// </summary>
        /// <param name="code">Code de l'état</param>
        /// <returns>L'état d'un budget</returns>
        BudgetEtatEnt GetByCode(string code);
    }
}
