using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Recette;
using Fred.Framework;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Recette
{
    /// <summary>
    /// Implementation de l'interface IBudgetRecetteRepository
    /// </summary>
    public class BudgetRecetteRepository : FredRepository<BudgetRecetteEnt>, IBudgetRecetteRepository
    {
        /// <summary>
        /// Constructeur par défault
        /// </summary>
        /// <param name="logMgr">manager de log</param>
        public BudgetRecetteRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne la recette d'un budget
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>La recette d'un budget</returns>
        public BudgetRecetteEnt GetByBudgetId(int budgetId)
        {
            return Context.BudgetRecettes.FirstOrDefault(r => r.BudgetRecetteId == budgetId);
        }
    }
}
