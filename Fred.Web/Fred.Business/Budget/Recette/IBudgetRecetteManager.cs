using Fred.Entities.Budget.Recette;

namespace Fred.Business.Budget.Recette
{
    /// <summary>
    /// Interface de gestion des recettes budgets
    /// </summary>
    public interface IBudgetRecetteManager : IManager<BudgetRecetteEnt>
    {
        /// <summary>
        /// Retourne la recette d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La recette d'un budget</returns>
        BudgetRecetteEnt GetByBudgetId(int budgetId);

        /// <summary>
        /// Supprime la recette ayant l'id donné
        /// </summary>
        /// <param name="recetteId">id de la recette a supprimer</param>
        void DeleteById(int recetteId);
    }
}
