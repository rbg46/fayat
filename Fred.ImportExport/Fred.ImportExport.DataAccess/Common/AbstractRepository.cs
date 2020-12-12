using System;
using System.Data.Entity;
using System.Linq;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Database.ImportExport;
using NLog;

namespace Fred.ImportExport.DataAccess.Common
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly ImportExportContext context;

        protected AbstractRepository(ImportExportContext context)
        {
            this.context = context;

            try
            {
                dbSet = context.Set<T>();
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

        public void Update(T entity)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
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
    }
}
