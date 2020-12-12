using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Directory;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Referential.CodeDeplacement.ExterneDirectory
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class ExterneDirectoryRepository : FredRepository<ExternalDirectoryEnt>, IExterneDirectoryRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ExterneDirectoryRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public ExterneDirectoryRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}