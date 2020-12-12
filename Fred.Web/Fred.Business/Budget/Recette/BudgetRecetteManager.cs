using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Recette;

namespace Fred.Business.Budget.Recette
{
    /// <summary>
    /// Implémentation du manager de recette
    /// </summary>
    public class BudgetRecetteManager : Manager<BudgetRecetteEnt, IBudgetRecetteRepository>, IBudgetRecetteManager
    {
        public BudgetRecetteManager(IUnitOfWork uow, IBudgetRecetteRepository budgetRecetteRepository)
          : base(uow, budgetRecetteRepository)
        {
        }

        /// <summary>
        /// Retourne la recette d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La recette d'un budget</returns>
        public BudgetRecetteEnt GetByBudgetId(int budgetId)
        {
            return Repository.GetByBudgetId(budgetId);
        }

        /// <summary>
        /// Supprime la recette ayant l'id donné
        /// </summary>
        /// <param name="recetteId">id de la recette a supprimer</param>
        public void DeleteById(int recetteId)
        {
            Repository.DeleteById(recetteId);
            Save();
        }
    }
}
