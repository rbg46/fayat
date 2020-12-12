using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Gestionnaire du workflow du budget.
    /// </summary>
    public class BudgetWorkflowManager : Manager<BudgetWorkflowEnt, IBudgetWorkflowRepository>, IBudgetWorkflowManager
    {
        public BudgetWorkflowManager(IUnitOfWork uow, IBudgetWorkflowRepository budgetWorkflowRepository)
          : base(uow, budgetWorkflowRepository)
        {
        }

        /// <inheritdoc />
        public void Add(BudgetEnt budget, int etatCibleId, int utilisateurId, string commentaire, bool creation = false)
        {
            var workflow = new BudgetWorkflowEnt()
            {
                AuteurId = utilisateurId,
                BudgetId = budget.BudgetId,
                Commentaire = commentaire,
                Date = DateTime.Now,
                EtatCibleId = etatCibleId
            };
            if (!creation)
            {
                workflow.EtatInitialId = budget.BudgetEtatId;
            }
            Repository.Insert(workflow);
        }

        /// <inheritdoc/>
        public void DeleteByBudgetId(int budgetId)
        {
            var allWorkflow = Repository.Get().Where(w => w.BudgetId == budgetId);
            foreach (var w in allWorkflow)
            {
                Repository.DeleteById(w.BudgetWorkflowId);
            }

            Save();
        }

        /// <inheritdoc />
        public IEnumerable<BudgetWorkflowEnt> GetList(int budgetId)
        {
            return this.Repository.GetList(budgetId);
        }

        /// <summary>
        /// Retourne la derniere date de verrouillage d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La date du dernier workflow de verrouillage d'un budget</returns>
        public DateTime? GetLastLockWorkflowDate(int budgetId)
        {
            var workflow = this.Repository.GetLastLockWorkflow(budgetId);
            return workflow != null ? (DateTime?)workflow.Date : null;
        }
    }
}
