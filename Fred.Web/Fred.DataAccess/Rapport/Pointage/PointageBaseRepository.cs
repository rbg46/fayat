using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   Référentiel abstrait de données pour les pointages.
    /// </summary>
    /// <typeparam name="TEntity">Entité de type PointageBase.</typeparam>
    public abstract class PointageBaseRepository<TEntity> : FredRepository<TEntity>, IPointageBaseRepository<TEntity> where TEntity : PointageBase
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe PointageBaseRepository.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of Work</param>
        protected PointageBaseRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}