using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fred.DataAccess.Common
{
    public class DbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;

        public FredDbContext Context { get; }

        public DbRepository(FredDbContext context)
        {
            Context = context;

            dbSet = Context.Set<TEntity>();
        }

        /// <summary>
        ///   Retourner la requête.
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Le tri.</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une requête</returns>
        internal IQueryable<TEntity> Get(
            List<Expression<Func<TEntity, bool>>> filters,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>>
            includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                // Add includes as path because it's not possible to use select in includes in ef core.
                // It's ThenInclude which is used instead but it's not possible to create an abstraction from it.
                // Source : https://stackoverflow.com/questions/47052419/how-to-pass-lambda-include-with-multiple-levels-in-entity-framework-core/47063432#47063432
                includeProperties?.ForEach(i => { query = query.Include(i.AsPath()); });

                filters?.ForEach(i => { query = query.Where(i); });

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (page != null && pageSize != null)
                {
                    query = query
                      .Skip((page.Value - 1) * pageSize.Value)
                      .Take(pageSize.Value);
                }

                return query;
            }
            catch (Exception ex)
            {
                throw new FredRepositoryException(ex.Message, ex);
            }
        }

        private EntityEntry<TEntity> EnsureAttach(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            return entry;
        }

        /// <summary>
        /// Récupérer une entité (détachée) par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de l'entité.</param>
        /// <returns>L'entité correspondante</returns>
        public virtual TEntity GetById(object id)
        {
            TEntity entity = dbSet.Find(id);

            // Détacher l'entité
            Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }


        /// <summary>
        ///   Rechercher une entité par identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité.</param>
        /// <returns>
        ///   L'entité correspondante
        /// </returns>
        public virtual TEntity FindById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task<TEntity> FindByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        ///   Charger des propriétés liées de l'entité.
        /// </summary>
        /// <param name="entity">l'entité à compléter.</param>
        /// <param name="expressions">Expressions des propriétés à inclure.</param>
        public virtual void PerformEagerLoading(TEntity entity, params Expression<Func<TEntity, object>>[] expressions)
        {
            if (expressions == null)
            {
                return;
            }

            foreach (var expression in expressions)
            {
                MemberExpression member = (MemberExpression)expression.Body;
                MemberEntry memberEntry = Context.Entry(entity).Member(member.Member.Name);
                CollectionEntry collectionEntry = memberEntry as CollectionEntry;
                if (collectionEntry != null)
                {
                    collectionEntry.Load();
                }
                else
                {
                    EnsureAttach((TEntity)memberEntry.EntityEntry.Entity);
                    ((ReferenceEntry)memberEntry).Load();
                }
            }
        }

        /// <summary>
        ///   Mettre à jour une entité.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public virtual void Update(TEntity entity)
        {
            var entry = EnsureAttach(entity);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        ///   Supprimer une entité.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à supprimer.</param>
        public virtual void DeleteById(object id)
        {
            TEntity entity = dbSet.Find(id);
            Delete(entity);
        }

        /// <summary>
        ///   Supprimer une entité.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        /// <param name="onlyIfAttashed">Supprime uniquement si l'entité est déjà attachée.</param>
        public virtual void Delete(TEntity entity, bool onlyIfAttashed = false)
        {
            var entry = Context.Entry(entity);
            if (onlyIfAttashed && entry.State == EntityState.Detached)
            {
                return;
            }
            EnsureAttach(entity);
            dbSet.Remove(entity);
        }

        /// <summary>
        ///   Insérer une nouvelle entité.
        /// </summary>
        /// <param name="entity">L'entité à insérer.</param>
        /// <returns>L'entité ajoutée.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return dbSet.Add(entity).Entity;
        }

        public void InsertOrUpdate(Func<TEntity, object> identifierExpression, IEnumerable<TEntity> entities)
        {
            if (!entities.Any() || identifierExpression == null)
                return;

            PropertyInfo[] properties = GetIdentifierProperties();

            foreach (TEntity entity in entities)
            {
                IQueryable<TEntity> query = dbSet.AsNoTracking();

                foreach (PropertyInfo property in properties)
                {
                    PropertyInfo propInfo = typeof(TEntity).GetProperty(property.Name);
                    query = query.Where(GetPropertyPredicate(entity, propInfo));
                }

                if (!query.Any())
                {
                    dbSet.Add(entity);
                    continue;
                }

                dbSet.Update(entity);
            }

            PropertyInfo[] GetIdentifierProperties() => identifierExpression
                    .Invoke(entities.FirstOrDefault())
                    .GetType()
                    .GetProperties();

            Expression<Func<TEntity, bool>> GetPropertyPredicate(TEntity entity, PropertyInfo propInfo)
            {
                var arg = Expression.Parameter(typeof(TEntity), "x");
                var expProperty = Expression.Property(arg, propInfo.Name);

                var expPropValue = Expression.Constant(propInfo.GetValue(entity));

                var equal = Expression.Equal(expProperty, expPropValue);
                var conv = Expression.Convert(equal, typeof(bool));
                return Expression.Lambda<Func<TEntity, bool>>(conv, new ParameterExpression[] { arg });
            }
        }

        /// <summary>
        ///   Retourner une requête.
        /// </summary>
        /// <returns>
        ///   Une requête
        /// </returns>
        public virtual IRepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper =
              new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        /// <summary>
        ///   Mettre à jour une entité.
        /// </summary>
        /// <param name="entity1">Entité 1 à détacher.</param>
        /// <param name="entity2">L'entité 2 à mettre à jour.</param>
        public virtual void Replace(TEntity entity1, TEntity entity2)
        {
            Context.Entry(entity1).State = EntityState.Detached;
            Update(entity2);
        }

        /// <summary>
        /// Permet de requêter  une entité
        /// </summary>
        /// <returns>Un IQueryable</returns>
        public virtual IQueryable<TEntity> Get()
        {
            try
            {
                return dbSet;
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// ATTENTION : ne fonctionne que pour les entités possédant une clé primaire technique unique
        /// Détache l'entité donnée du contexte Entity 
        /// </summary>
        /// <param name="entityId">Identifiant du rapport</param>
        public void RemoveAttachedEntityFromContext(int entityId)
        {
            var entity = FindById(entityId);
            if (entity != null)
            {
                // the entity you want to update is already attached, we need to detach it and attach the updated entity instead
                Context.Entry(entity).State = EntityState.Detached;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TResult> SelectOneColumn<TResult>(Expression<Func<TEntity, TResult>> column, Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where)
                .Select(column);
        }

        /// <summary>
        /// Mise à jour d'une liste de champs pour une entité choisie
        /// </summary>
        /// <param name="entity">Entité</param>
        /// <param name="fieldsToUpdate">Champs à mettre à jour</param>
        public virtual void Update(TEntity entity, List<Expression<Func<TEntity, object>>> fieldsToUpdate)
        {
            EnsureAttach(entity);
            fieldsToUpdate?.ForEach(x => Context.Entry(entity).Property(x).IsModified = true);
        }

        /// <inheritdoc/>
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        public void InsertRange(List<TEntity> entities)
        {
            Context.AddRange(entities);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            Context.UpdateRange(entities);
        }
    }
}
