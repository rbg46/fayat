using Fred.Entities.Budget;
using System;
using System.Collections.Generic;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface du workflow du budget
    /// </summary>
    public interface IBudgetWorkflowManager : IManager<BudgetWorkflowEnt>
    {
        /// <summary>
        /// Ajoute un nouveau workflow concernant un budget
        /// </summary>
        /// <param name="budget">budget</param>
        /// <param name="etatCibleId">Identifiant de l'état cible</param>
        /// <param name="utilisateurId">identifiant de l'utilisateur</param>
        /// <param name="commentaire">Commentaire</param>
        /// <param name="creation">Indique si le budget est en création</param>
        void Add(BudgetEnt budget, int etatCibleId, int utilisateurId, string commentaire, bool creation = false);

        /// <summary>
        /// Retourne la liste des workflows concernant un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La liste des workflows d'un budget</returns>
        IEnumerable<BudgetWorkflowEnt> GetList(int budgetId);

        /// <summary>
        /// Supprime tous les workflow associés à ce budget ID
        /// </summary>
        /// <param name="budgetId">Id du budget dont on supprime les workflows</param>
        void DeleteByBudgetId(int budgetId);

        /// <summary>
        /// Retourne la derniere date de verrouillage d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La date du dernier workflow de verrouillage d'un budget</returns>
        DateTime? GetLastLockWorkflowDate(int budgetId);
    }
}
