using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;

namespace Fred.Business
{
    public interface IManager<TEntity> : IManager where TEntity : class
    {
        TEntity FindById(int id);

        Task<TEntity> FindByIdAsync(int id);

        void BusinessValidation<TOtherEntity>(TOtherEntity entity, IValidator<TOtherEntity> validator) where TOtherEntity : class;

        void CheckAccessToEntity(TEntity entity);

        void Save();

        void PerformEagerLoading(TEntity entity, params Expression<Func<TEntity, object>>[] expressions);
    }
}
