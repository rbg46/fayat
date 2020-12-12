using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Import
{
    /// <summary>
    /// Référentiel de données pour les transco d'import.
    /// </summary>
    public class TranscoImportRepository : FredRepository<TranscoImportEnt>, ITranscoImportRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="TranscoImportRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public TranscoImportRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}
