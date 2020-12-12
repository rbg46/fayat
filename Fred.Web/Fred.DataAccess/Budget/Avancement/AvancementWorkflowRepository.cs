using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;
using Fred.Framework;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Avancement
{
    /// <summary>
    ///   Référentiel de données pour les workflows de l'avancement.
    /// </summary>
    public class AvancementWorkflowRepository : FredRepository<AvancementWorkflowEnt>, IAvancementWorkflowRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="AvancementWorkflowRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit Of Work</param>
        public AvancementWorkflowRepository(FredDbContext context)
          : base(context)
        { }

        /// <inheritdoc />
        public IEnumerable<AvancementWorkflowEnt> GetList(int avancementId)
        {
            return this.Context.AvancementWorkflows.Where(w => w.AvancementId == avancementId).ToList();
        }
    }
}
