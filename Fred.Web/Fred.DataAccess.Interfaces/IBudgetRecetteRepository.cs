
using Fred.Entities.Budget.Recette;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Acces aux données des recettes
    /// </summary>
    public interface IBudgetRecetteRepository : IFredRepository<BudgetRecetteEnt>
    {
        /// <summary>
        /// Retourne la recette d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La recette d'un budget</returns>
        BudgetRecetteEnt GetByBudgetId(int budgetId);
    }
}
