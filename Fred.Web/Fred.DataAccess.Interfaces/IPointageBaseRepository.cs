
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les pointages.
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  /// <seealso cref="Fred.DataAccess.Common.IRepository{TEntity}" />
  public interface IPointageBaseRepository<TEntity> : IRepository<TEntity> where TEntity : PointageBase
  {
  }
}