using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fred.Common.Tests.EntityFramework
{
    public class FakeDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>
        where TEntity : class, new()
    {
        private readonly PropertyInfo primaryKey;

        private readonly ICollection<TEntity> items;

        private readonly IQueryable<TEntity> query;

        public FakeDbSet()
        {
            primaryKey = typeof(TEntity).GetProperties()[0];
            items = new List<TEntity>();
            query = items.AsQueryable().RewriteDbFunctions(typeof(FakeDbFunctions));
        }

        public override EntityEntry<TEntity> Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            items.Add(entity);
            return null;
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            entities.Select(Add);
        }

        public override EntityEntry<TEntity> Attach(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var item = items.SingleOrDefault(i =>
                primaryKey.GetValue(entity).Equals(primaryKey.GetValue(i))
            );
            if (item == null)
                items.Add(entity);
            return null;
        }

        public override TEntity Find(params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException("keyValues");
            if (keyValues.Any(k => k == null))
                throw new ArgumentOutOfRangeException("keyValues");

            return items.SingleOrDefault(i =>
                keyValues.Contains(primaryKey.GetValue(i))
            );
        }

        public override Task<TEntity> FindAsync(params object[] keyValues)
        {
            return Task.FromResult(Find(keyValues));
        }

        public override Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return Task.FromResult(Find(keyValues));
        }

        public override EntityEntry<TEntity> Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (primaryKey.GetValue(entity) == null)
                throw new ArgumentOutOfRangeException("entity");
            var item = items.SingleOrDefault(i =>
                primaryKey.GetValue(entity).Equals(primaryKey.GetValue(i))
            );
            if (item != null)
                items.Remove(item);
            return null;
        }

        public override void RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            entities.Select(Remove);
        }

        public Type ElementType
        {
            get { return query.ElementType; }
        }

        public Expression Expression
        {
            get { return query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return query.Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return query.GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return query.GetEnumerator();
        }
    }
}
