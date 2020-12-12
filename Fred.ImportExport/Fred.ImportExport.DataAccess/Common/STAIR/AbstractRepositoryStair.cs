using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Database.ImportExport;
using NLog;

namespace Fred.ImportExport.DataAccess.Common
{
    /// <summary>
    /// Classe de base des repository
    /// </summary>
    /// <typeparam name="T">Type de repository</typeparam>
    public abstract class AbstractRepositoryStair<T> : IRepository<T> where T : class
    {
        private IDbSet<T> dbSet;
        private readonly StairContext stairContext;

        

        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        /// <param name="unitOfWorkStair">Unit of work</param>
        protected AbstractRepositoryStair(UnitOfWorkStair unitOfWorkStair)
        {
            try
            {
                stairContext = unitOfWorkStair.StairContext;
                dbSet = stairContext.Set<T>();
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogManager.GetLogger(GetType().FullName, typeof(FredRepositoryException)).Error(e);
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        ///   Obtient ou définit le DbSet.
        /// </summary>
        internal IDbSet<T> DbSet => dbSet ?? (dbSet = stairContext.Set<T>());

        /// <summary>
        /// Permet d'ajouter une nouvelle entité
        /// </summary>
        /// <param name="entity">L'entité à ajouter</param>
        public void Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
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
        /// Permet de mettre à jour une entité
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour</param>
        public void Update(T entity)
        {
            try
            {
                stairContext.Entry(entity).State = EntityState.Modified;
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
        /// Permet de supprimer une entité
        /// </summary>
        /// <param name="entity">Entité</param>
        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
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
        /// Permet de récupérer une entité
        /// </summary>
        /// <param name="id">L'identifiant de l'entité</param>
        /// <returns>Une entité</returns>
        public T GetById(int id)
        {
            try
            {
                return dbSet.Find(id);
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
        /// Permet de requêter  une entité
        /// </summary>
        /// <returns>Un IQueryable</returns>
        public IQueryable<T> Get()
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
        /// Permet de requêter  une entité
        /// </summary>
        /// <param name="includes">includes</param>
        /// <returns>Un IQueryable</returns>
        public IQueryable<T> Get(List<Expression<Func<T, object>>> includes)
        {
            try
            {
                IQueryable<T> query = dbSet;

                includes?.ForEach(i => { query = query.Include(i); });

                return query;

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
    }
}
