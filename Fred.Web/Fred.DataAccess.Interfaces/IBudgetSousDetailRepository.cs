using System.Collections.Generic;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Référentiel de données pour budget sous détail.
    /// </summary>
    public interface IBudgetSousDetailRepository : IRepository<BudgetSousDetailEnt>
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
        /// Retourne un sous-détail en fonction de son identifiant.
        /// </summary>
        /// <param name="budgetSousDetailId">Identifiant du sous-détail.</param>
        /// <returns>Le sous-détail ou null s'il n'existe pas.</returns>
        BudgetSousDetailEnt GetById(int budgetSousDetailId);

        /// <summary>
        /// Récupère les sous-détails pour la comparaison de budget.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget concerné.</param>
        /// <returns>Les sous-détails.</returns>
        List<SousDetailDao> GetSousDetailPourBudgetComparaison(int budgetId);

        void UpdateSousDetail(BudgetSousDetailEnt sousDetail);
    }
}
