using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Framework;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    ///   Référentiel de données pour les barèmes exploitation CI.
    /// </summary>
    public class BudgetWorkflowRepository : FredRepository<BudgetWorkflowEnt>, IBudgetWorkflowRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="BudgetWorkflowRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit Of Work</param>
        public BudgetWorkflowRepository(FredDbContext context)
          : base(context)
        { }

        /// <inheritdoc />
        public IEnumerable<BudgetWorkflowEnt> GetList(int budgetId)
        {
            return this.Context.BudgetWorkflows.Where(w => w.BudgetId == budgetId).ToList();
        }

        /// <summary>
        /// Retourne la derniere entrée de verrouillage dans la table workflows concernant un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>le dernier workflow de verrouillage d'un budget</returns>
        public BudgetWorkflowEnt GetLastLockWorkflow(int budgetId)
        {
            return this.Context.BudgetWorkflows.Where(w => w.BudgetId == budgetId && (w.EtatCible.Code == EtatBudget.EnApplication)).OrderByDescending(w => w.Date).FirstOrDefault();
        }

    }
}
