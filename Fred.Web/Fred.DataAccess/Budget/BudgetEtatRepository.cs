using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Framework;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    ///   Référentiel de données pour les barèmes exploitation CI.
    /// </summary>
    public class BudgetEtatRepository : FredRepository<BudgetEtatEnt>, IBudgetEtatRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="BudgetEtatRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit Of Work</param>
        public BudgetEtatRepository(FredDbContext context)
          : base(context)
        { }

        /// <inheritdoc />
        public BudgetEtatEnt GetByCode(string code)
        {
            return this.Context.BudgetEtats.FirstOrDefault(e => e.Code == code);
        }
    }
}
