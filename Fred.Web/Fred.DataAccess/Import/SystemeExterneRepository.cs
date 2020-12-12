using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Import
{
    /// <summary>
    /// Référentiel de données pour les systèmes externes.
    /// </summary>
    public class SystemeExterneRepository : FredRepository<SystemeExterneEnt>, ISystemeExterneRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SystemeExterneRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public SystemeExterneRepository(FredDbContext context)
          : base(context)
        { }
    }
}
