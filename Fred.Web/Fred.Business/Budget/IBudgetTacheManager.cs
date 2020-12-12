using System.Collections.Generic;
using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface d'une relation budget/tache
    /// </summary>
    public interface IBudgetTacheManager : IManager<BudgetTacheEnt>
    {
        /// <summary>
        /// Insère un budget/tache
        /// </summary>
        /// <param name="budgetTache">le budget/tache</param>
        void AddBudgetTache(BudgetTacheEnt budgetTache);

        /// <summary>
        /// Delete un budget/tache
        /// </summary>
        /// <param name="budgetTacheId">le budget/tache</param>
        void DeleteBudgetTache(int budgetTacheId);

        /// <summary>
        /// Récupère les budget/tache.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tacheId">Identifiant de la tâche</param>
        /// <returns>Les T4 du budget.</returns>
        BudgetTacheEnt GetByBudgetIdAndTacheId(int budgetId, int tacheId);

        /// <summary>
        /// Insère un budget/tache
        /// </summary>
        /// <param name="budgetTache">le budget/tache</param>
        /// <returns>Le budget/tache à jour</returns>
        BudgetTacheEnt UpdateBudgetTache(BudgetTacheEnt budgetTache);

        /// <summary>
        /// Récupère les informations sur les tâches pour un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Les informations sur les tâches.</returns>
        IEnumerable<BudgetTacheEnt> Get(int budgetId);

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache1To3sModel">Les tâches de niveau 1 à 3 à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        List<BudgetDetailSave.BudgetTacheCreatedModel> Save(int budgetId, List<BudgetDetailSave.Tache1To3Model> tache1To3sModel);
    }
}
