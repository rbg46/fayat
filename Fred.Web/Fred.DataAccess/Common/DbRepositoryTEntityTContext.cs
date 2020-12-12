// File name must match first type name
// Il n'est pas possible de mettre le même nom de classe, c'est une classe générique héritée et le nom de fichier est déjà utilisée pour la classe de base.
// En fait, il vaudrait mieux renommer la classe...
#pragma warning disable SA1649
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Common
{
    /// <summary>
    ///   Repository de base Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité à gérer</typeparam>
    /// <typeparam name="TContext">LE DbContext</typeparam>
    public abstract class DbRepository<TEntity, TContext> : DbRepository<TEntity>
      where TEntity : class
      where TContext : DbContext,
      new()
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="DbRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="logMgr">Le manager des logs.</param>
        /// <param name="uow">Le unit of work</param>
        protected DbRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}

#pragma warning restore SA1649