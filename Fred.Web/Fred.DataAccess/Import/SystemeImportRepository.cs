using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Import
{
    /// <summary>
    /// Référentiel de données pour les systèmes d'import.
    /// </summary>
    public class SystemeImportRepository : FredRepository<SystemeImportEnt>, ISystemeImportRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SystemeImportRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public SystemeImportRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}
