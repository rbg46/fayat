using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface du budget T4.
    /// </summary>
    public interface IBudgetT4Manager : IManager<BudgetT4Ent>
    {
        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="loadSousDetails">Indique si on veut charger les sous-détails liés aux budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        IEnumerable<BudgetT4Ent> GetByBudgetId(int budgetId, bool loadSousDetails = false);

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
        /// Récupère un budget T4.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="tacheId">Identifiant de la tâche.</param>
        /// <returns>Un budget T4.</returns>
        BudgetT4Ent GetByTacheIdAndBudgetId(int budgetId, int tacheId);

        List<BudgetT4Ent> GetByTacheIdsAndBudgetId(int budgetId, IEnumerable<int> tacheIds);

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du budget.</param>
        /// <returns>Les budgets T4 du budget.</returns>
        BudgetT4Ent GetByIdWithSousDetailAndAvancement(int budgetT4Id);

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="taches4Model">Les tâches de niveau 4 à enregistrer.</param>
        /// <param name="budgetT4sDeleted">Liste des identifiants des budget T4 à supprimer</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        List<BudgetDetailSave.BudgetT4CreatedModel> Save(int budgetId, List<BudgetDetailSave.Tache4Model> taches4Model, List<int> budgetT4sDeleted);

        /// <summary>
        /// Ajoute un budgetT4 au contexte
        /// </summary>
        /// <param name="budgetT4">Budget T4.</param>
        void Add(BudgetT4Ent budgetT4);

        /// <summary>
        /// Met à jour un budgetT4 au contexte
        /// </summary>
        /// <param name="budgetT4">Budget T4.</param>
        void Update(BudgetT4Ent budgetT4);

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache4Model">La tâche de niveau 4 à enregistrer.</param>
        /// <param name="created">Indique si le budget T4 a été créé.</param>
        /// <returns>Le budget T4.</returns>
        BudgetT4Ent Save(int budgetId, BudgetSousDetailSave.Tache4Model tache4Model, out bool created);

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        List<IGrouping<TacheEnt, BudgetT4Ent>> GetT4GroupByT3ByBudgetId(int budgetId);

        /// <summary>
        /// Retourne une liste des T4s
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4</returns>
        List<BudgetT4Ent> GetT4ByBudgetId(int budgetId);

        /// <summary>
        /// Indique si un budget est révisé.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <returns>True si le budget est révisé, sinon false.</returns>
        bool IsBudgetRevise(int budgetId);
    }
}
