using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.BibliothequePrix
{
    /// <summary>
    /// Référentiel de données pour les bibliothèques des prix.
    /// </summary>
    public class BudgetBibliothequePrixRepository : FredRepository<BudgetBibliothequePrixEnt>, IBudgetBibliothequePrixRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="BudgetBibliothequePrixRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit Of Work</param>
        public BudgetBibliothequePrixRepository(FredDbContext context)
          : base(context)
        { }

        public BudgetBibliothequePrixEnt GetBibliothequePrixByOrganisationIdAndDeviseId(int OrganisationId, int DeviseId)
        {
            return Context.BibliothequePrix
                .Where(bp => bp.OrganisationId == OrganisationId && bp.DeviseId == DeviseId)
                .Include(bp => bp.Items)
                    .ThenInclude(i => i.ItemValuesHisto)
                .FirstOrDefault();
        }
    }
}
