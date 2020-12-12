using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Log;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Log
{
    /// <summary>
    ///   Représente un référentiel de données pour les logs.
    /// </summary>
    public class NLogRepository : FredRepository<NLogEnt>, INLogRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="NLogRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public NLogRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}
