using System;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Personnel;

namespace Fred.DataAccess.Interfaces
{
    public interface IRepositoryQuery<TEntity> where TEntity : class
    {
        IRepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> GetPage(int page, int pageSize);
        IQueryable<TEntity> GetPage(int page, int pageSize, out int totalCount);
        IRepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression);
        IRepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryable<TEntity> Intersect(IQueryable<TEntity> repoIntersect);
        IQueryable<TEntity> Union(IQueryable<TEntity> repoUnion);
    }
}
