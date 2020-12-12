using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Budget.BibliothequePrix
{
    /// <summary>
    /// Référentiel de données pour l'historique des éléments de bibliothèques des prix.
    /// </summary>
    public class BudgetBibliothequePrixItemValuesHistoRepository : FredRepository<BudgetBibliothequePrixItemValuesHistoEnt>, IBudgetBibliothequePrixItemValuesHistoRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="BudgetBibliothequePrixItemValuesHistoRepository" />. 
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit Of Work</param>
        public BudgetBibliothequePrixItemValuesHistoRepository(FredDbContext context)
            : base(context)
        {
        }
    }
}
