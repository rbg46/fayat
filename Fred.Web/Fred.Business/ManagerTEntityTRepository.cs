using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.DataAccess.Interfaces;
using Fred.Framework.Extensions;
using Fred.Framework.Tool;

namespace Fred.Business
{
    public abstract class Manager<TEntity, TRepository> : ManagersAccess, IManager<TEntity>
        where TEntity : class
        where TRepository : IRepository<TEntity>
    {
        private readonly IUnitOfWork uow;

        protected TRepository Repository { get; }
        protected IValidator<TEntity> Validator { get; }

        protected Manager(IUnitOfWork uow, TRepository repository)
        {
            this.uow = uow;
            Repository = repository;
        }

        protected Manager(IUnitOfWork uow, TRepository repository, IValidator<TEntity> validator)
            : this(uow, repository)
        {
            Validator = validator;
        }

        protected void ThrowBusinessValidationException(string properyName, string error)
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(properyName, error)
            });
        }

        public TEntity FindById(int id) => Repository.FindById(id);

        public async Task<TEntity> FindByIdAsync(int id) => await Repository.FindByIdAsync(id);

        public void BusinessValidation(TEntity entity) => BusinessValidation(entity, Validator);

        public void BusinessValidation<TOtherEntity>(TOtherEntity entity, IValidator<TOtherEntity> validator) where TOtherEntity : class
        {
            if (validator == null)
            {
                return;
            }

            ValidationResult result = validator.Validate(entity);

#if DEBUG
            {
                if (result.IsValid)
                {
                    Debug.WriteLine("Validation de " + typeof(TOtherEntity) + " : ok");
                }
                else
                {
                    StringBuilder msg = new StringBuilder();
                    msg.AppendLine("==== Validation Failure ===");
                    msg.Append("Validation de ").Append(typeof(TOtherEntity)).AppendLine(" : ko");
                    result.Errors.ForEach(failure => msg.AppendLine(failure.ErrorMessage));
                    msg.AppendLine(msg.ToString());
                    msg.AppendLine("Entity : ");
                    msg.AppendLine(DumpObject.GetObjectString(entity));
                    msg.AppendLine("===========================");
                    Debug.WriteLine(msg.ToString());
                }
                Debug.Flush();
            }
#endif

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        public virtual void CheckAccessToEntity(TEntity entity)
        {
        }

        public virtual void CheckAccessToEntity(TEntity entity, int userId)
        {
        }

        public void CheckAccessToEntity(int id) => CheckAccessToEntity(Repository.FindById(id));

        public void CheckAccessToEntity(int id, int userId) => CheckAccessToEntity(Repository.FindById(id), userId);

        public void Save() => uow.Save();

        public async Task SaveAsync() => await uow.SaveAsync();

        protected void SaveWithTransaction() => uow.SaveWithTransaction();

        public void PerformEagerLoading(TEntity entity, params Expression<Func<TEntity, object>>[] expressions) => Repository.PerformEagerLoading(entity, expressions);
    }
}
