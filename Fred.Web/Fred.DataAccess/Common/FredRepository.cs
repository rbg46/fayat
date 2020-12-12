using Fred.DataAccess.Interfaces;
using Fred.EntityFramework;

namespace Fred.DataAccess.Common
{
    /// <summary>
    ///   Gestionnaire du contexte de la base de donnée.
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité gérée</typeparam>
    public abstract class FredRepository<TEntity> : DbRepository<TEntity>, IFredRepository<TEntity>
      where TEntity : class
    {
        protected FredRepository(FredDbContext context)
            : base(context)
        {
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <exception cref="System.UnauthorizedAccessException">Informe la non-autorisation d'accéder</exception>
        public virtual void CheckAccessToEntity(TEntity entity, int userId)
        {
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void CheckAccessToEntity(int id, int userId)
        {
            CheckAccessToEntity(FindById(id), userId);
        }
    }
}