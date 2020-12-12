using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Entities.Budget;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Référentiel de données pour budget T4.
    /// </summary>
    public interface IBudgetT4Repository : IRepository<BudgetT4Ent>
    {
        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="loadSousDetails">Indique si on veut charger les sous-détails liés aux budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        IEnumerable<BudgetT4Ent> GetByBudgetId(int budgetId, bool loadSousDetails = false);

        /// <summary>
        /// Récupère un budget T4.
        /// </summary>
        /// <param name="tacheId">Identifiant de la tâche.</param>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>Un budget T4.</returns>
        BudgetT4Ent GetByTacheIdAndBudgetId(int tacheId, int budgetId);

        /// <summary>
        /// Récupère un T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tacheId">Identifiant de la tâche</param>
        /// <returns>Le T4 du budget ou null s'il n'existe pas.</returns>
        BudgetT4Ent Get(int budgetId, int tacheId);

        BudgetT4Ent GetByBudgetIdAndTacheIdWithSousDetails(int budgetId, int tacheId);

        List<BudgetT4Ent> GetByBudgetIdAndTacheIdsWithSousDetails(int budgetId, IEnumerable<int> tacheIds);

        /// <summary>
        /// Récupère les budgets T4 d'un budget créé avant .
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="date">Date</param>
        /// <param name="loadSousDetails">Indique si on veut charger les sous-détails liés aux budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        IEnumerable<BudgetT4Ent> GetByBudgetIdAndCreationDate(int budgetId, DateTime? date, bool loadSousDetails = false);

        /// <summary>
        /// Récupère la liste des budgets T4 enfant d'un T3 pour une version de budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache3Id">Identifiant de la tâche</param>
        /// <returns>Le T4 du budget ou null s'il n'existe pas.</returns>
        List<BudgetT4Ent> GetByBudgetIdAndTache3Id(int budgetId, int tache3Id);

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du budget.</param>
        /// <returns>Les budgets T4 du budget.</returns>
        BudgetT4Ent GetByIdWithSousDetailAndAvancement(int budgetT4Id);

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        List<IGrouping<TacheEnt, BudgetT4Ent>> GetT4GroupByT3ByBudgetId(int budgetId);

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        List<BudgetT4Ent> GetT4ByBudgetId(int budgetId);

        /// <summary>
        /// Indique si un budget est révisé.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <returns>True si le budget est révisé, sinon false.</returns>
        bool IsBudgetRevise(int budgetId);

        Task UpdateVueSDToZero(List<BudgetT4Ent> budgetT4WithVueT4);

        void UpdateT4(BudgetT4Ent budgetT4);
    }
}
