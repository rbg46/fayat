using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.PermissionFonctionnalite;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.PermissionFonctionnalite
{
    /// <summary>
    /// PermissionFonctionnalite Repository 
    /// </summary>
    public class PermissionFonctionnaliteRepository : FredRepository<PermissionFonctionnaliteEnt>, IPermissionFonctionnaliteRepository
    {

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logMgr">logMgr</param>
        /// <param name="uow">uow</param>
        public PermissionFonctionnaliteRepository(FredDbContext context)
          : base(context)
        {

        }
    }
}
