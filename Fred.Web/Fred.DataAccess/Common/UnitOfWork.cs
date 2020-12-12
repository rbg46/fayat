using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EntityBase;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fred.DataAccess.Common
{
    /// <summary>
    ///   Le pattern Unit of work
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private Hashtable repositories;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="UnitOfWork" />.
        /// </summary>
        /// <param name="context">le contexte.</param>
        public UnitOfWork(FredDbContext context, ISecurityManager securityManager)
        {
            this.securityManager = securityManager;

            Context = context;
        }

        private readonly ISecurityManager securityManager;

        /// <summary>
        ///   L''accés couche data
        /// </summary>
        public FredDbContext Context { get; }

        /// <summary>
        ///   Sauvegarder les modificaions en cours.
        /// </summary>
        public void Save()
        {
            try
            {
                // Tips : Use this code to debug change in the context
                ////#if DEBUG
                ////var added = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                ////var modified = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                ////var deleted = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
                ////#endif

                Context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                TransformDbUpdateExceptionToFredRepositoryException(ex);
            }
        }

        /// <summary>
        ///   Sauvegarder les modificaions en cours de manière async.
        /// </summary>
        public async Task SaveAsync()
        {
            try
            {
                var userId = securityManager.GetUtilisateurId();
                var dateTimeNow = DateTime.UtcNow;
                var entires = Context.ChangeTracker.Entries();

                HandleAddedEntities(userId, dateTimeNow, entires);
                HandleModifiedEntities(userId, dateTimeNow, entires);
                HandleDeletedEntities(userId, dateTimeNow, entires);

                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                TransformDbUpdateExceptionToFredRepositoryException(ex);
            }
        }

        private static void HandleAddedEntities(int userId, DateTime dateTimeNow, System.Collections.Generic.IEnumerable<EntityEntry> entires)
        {
            var addedList = entires
                .Where(e => e.State == EntityState.Added && (e.Entity is ICreatable || e.Entity is IAuditableEntity || e.Entity is IDeletable))
                .Select(e => e.Entity)
                .ToList();

            foreach (var added in addedList)
            {
                if (added is ICreatable)
                {
                    var entity = added as ICreatable;

                    entity.AuteurCreationId = entity.AuteurCreationId == default ? userId : entity.AuteurCreationId;
                    entity.DateCreation = entity.DateCreation == default ? dateTimeNow : entity.DateCreation;
                }
                else if (added is IAuditableEntity)
                {
                    var entity = added as IAuditableEntity;

                    entity.AuteurCreationId = entity.AuteurCreationId ?? userId;
                    entity.DateCreation = entity.DateCreation ?? dateTimeNow;
                }
            }
        }

        private static void HandleModifiedEntities(int userId, DateTime dateTimeNow, System.Collections.Generic.IEnumerable<EntityEntry> entires)
        {
            var modifiedList = entires.Where(e => e.State == EntityState.Modified && e.Entity is IAuditableEntity)
                .Select(e => e.Entity)
                .ToList();

            foreach (var modified in modifiedList)
            {
                var entity = modified as IAuditableEntity;

                entity.AuteurModificationId = entity.AuteurModificationId ?? userId;
                entity.DateModification = entity.DateModification ?? dateTimeNow;
            }
        }

        private static void HandleDeletedEntities(int userId, DateTime dateTimeNow, System.Collections.Generic.IEnumerable<EntityEntry> entires)
        {
            var deletedList = entires.Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable)
                .Select(e => e.Entity)
                .ToList();

            foreach (var deleted in deletedList)
            {
                var entity = deleted as IDeletable;

                entity.AuteurSuppressionId = entity.AuteurSuppressionId ?? userId;
                entity.DateSuppression = entity.DateSuppression ?? dateTimeNow;
            }
        }

        private static void TransformDbUpdateExceptionToFredRepositoryException(DbUpdateException ex)
        {
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendLine($"DbUpdateException error details - {ex?.InnerException?.InnerException?.Message}");

            //Récupération des DbEntityEntry
            foreach (EntityEntry entry in ex.Entries)
            {
                errorMessage.AppendLine($"Entity of type {entry.Entity.GetType().Name} in state {entry.State} could not be updated");
            }

            throw new FredRepositoryException(errorMessage.ToString(), ex);
        }

        /// <summary>
        /// Sauvegarde des modifications via une transaction.
        /// </summary>
        public void SaveWithTransaction()
        {
            using (IDbContextTransaction dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Save();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> pour libérer les ressources non managées et managées.; <c>false</c> pour libérer
        ///   uniquement les ressources non managées.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                Context?.Dispose();
            }

            this.disposed = true;
        }
    }
}
