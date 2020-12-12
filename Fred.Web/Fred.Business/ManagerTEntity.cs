using FluentValidation;
using Fred.DataAccess.Interfaces;

namespace Fred.Business
{
    public abstract class Manager<TEntity> : Manager<TEntity, IRepository<TEntity>> where TEntity : class
    {
        protected Manager(IUnitOfWork uow, IRepository<TEntity> repository)
          : base(uow, repository)
        {
        }

        protected Manager(IUnitOfWork uow, IRepository<TEntity> repository, IValidator<TEntity> validator)
          : base(uow, repository, validator)
        {
        }
    }
}