namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface du gestionnaire du contexte de la base de donnée.
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité gérée</typeparam>
    public interface IFredRepository<TEntity> : IRepository<TEntity>
      where TEntity : class
    {
        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void CheckAccessToEntity(TEntity entity, int userId);

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void CheckAccessToEntity(int id, int userId);
    }
}