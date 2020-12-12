using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget;
using System.Collections.Generic;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface du budget sous détail.
    /// </summary>
    public interface IBudgetSousDetailManager : IManager<BudgetSousDetailEnt>
    {
        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        IEnumerable<BudgetSousDetailEnt> GetByBudgetT4Id(int budgetT4Id);


        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        IEnumerable<BudgetSousDetailEnt> GetByBudgetT4IdIncludeRessource(int budgetT4Id);

        /// <summary>
        /// Insère un sous-détail dans le contexte
        /// </summary>
        /// <param name="sousDetail">Le sous-détail à insérer</param>
        void InsereSousDetail(BudgetSousDetailEnt sousDetail);

        /// <summary>
        /// Met à jour un sous-détail dans le contexte
        /// </summary>
        /// <param name="sousDetail">Le sous-détail à insérer</param>
        void UpdateSousDetail(BudgetSousDetailEnt sousDetail);

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du détail.</param>
        /// <param name="itemsChanged">Elements du sous-détail ajoutés ou modifiés.</param>
        /// <param name="itemsDeletedId">Identifiants des élements du sous-détail supprimés.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        List<BudgetSousDetailSave.ItemCreatedModel> Save(int budgetT4Id, List<BudgetSousDetailSave.ItemModel> itemsChanged, List<int> itemsDeletedId);
    }
}
