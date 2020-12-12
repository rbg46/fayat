using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RessourcesRecommandees;
using Fred.EntityFramework;
using Fred.Web.Shared.Models.RessourceRecommandee;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RessourceRecommandee
{
    /// <summary>
    /// RessourceRecommandeeRepository Class
    /// </summary>
    public class RessourceRecommandeeRepository : FredRepository<RessourceRecommandeeEnt>, IRessourceRecommandeeRepository
    {
        #region Fields

        /// <summary>
        /// The database set
        /// </summary>
        private readonly DbSet<RessourceRecommandeeEnt> dbSet;

        #endregion

        public RessourceRecommandeeRepository(FredDbContext context)
            : base(context)
        {
            this.dbSet = this.Context.RessourceRecommandees;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>RessourceRecommandeeEnt</returns>
        public RessourceRecommandeeEnt Add(RessourceRecommandeeEnt entity)
        {
            var result = this.dbSet.Add(entity);
            var saveStatus = this.SaveChanges(result.Entity);
            if (saveStatus != -1)
            {
                return result.Entity;
            }
            return null;
        }

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>List of RessourceRecommandeeEnt</returns>
        public List<RessourceRecommandeeEnt> AddAll(List<RessourceRecommandeeEnt> items)
        {
            this.dbSet.AddRange(items);
            var saveStatus = this.SaveChanges();
            if (saveStatus != -1)
            {
                return items.ToList();
            }
            return null;
        }

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>
        /// List Of RessourceRecommandee
        /// </returns>
        public List<RessourceRecommandeeEnt> AddOrUpdate(List<RessourceRecommandeeEnt> entities)
        {
            // récupération des id de ressources recommandées par organisations (normalement une seule organisation)
            var organisationIds = entities.Select(x => x.OrganisationId).Distinct().ToList();
            var existingRessourceRecommandeeIds = this.Context.RessourceRecommandees
                                                    .Where(x => organisationIds.Contains(x.OrganisationId))
                                                    .Select(x => x.RessourceRecommandeeId)
                                                    .ToList();

            var listToDelete = entities.Where(x => x.IsRecommandee == false && existingRessourceRecommandeeIds.Contains(x.RessourceRecommandeeId)).ToList();
            var listToAdd = entities.Where(x => x.IsRecommandee == true && !existingRessourceRecommandeeIds.Contains(x.RessourceRecommandeeId)).ToList();

            using (var transaction = this.Context.Database.BeginTransaction())
            {
                var deleteResult = this.DeleteAll(listToDelete);
                var addResult = this.AddAll(listToAdd);
                transaction.Commit();
                if (deleteResult?.Count == listToDelete.Count && addResult?.Count == listToAdd.Count)
                {
                    return entities;
                }
                return null;
            }
        }

        /// <summary>
        /// Anies the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>bool</returns>
        public bool Any(Expression<Func<RessourceRecommandeeEnt, bool>> expression)
        {
            var result = this.dbSet.Any(expression);
            return result;
        }

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>List of RessourceRecommandeeEnt</returns>
        public List<RessourceRecommandeeEnt> DeleteAll(List<RessourceRecommandeeEnt> entities)
        {
            entities.ForEach(x =>
            {
                this.dbSet.Attach(x);
            });
            this.dbSet.RemoveRange(entities);
            var saveStatus = this.SaveChanges();
            if (saveStatus != -1)
            {
                return entities.ToList();
            }
            return null;
        }

        /// <summary>
        /// Finds the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// RessourceRecommandeeEnt
        /// </returns>
        public RessourceRecommandeeEnt Find(Expression<Func<RessourceRecommandeeEnt, bool>> expression)
        {
            var result = this.dbSet.AsNoTracking().FirstOrDefault(expression);
            return result;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>
        /// List of RessourceRecommandeeEnt
        /// </returns>
        public List<RessourceRecommandeeEnt> FindAll()
        {
            var result = this.dbSet.AsNoTracking().Select(x => x);
            return result.ToList();
        }

        /// <summary>
        /// Includes the specified include expressions.
        /// </summary>
        /// <param name="includeExpressions">The include expressions.</param>
        /// <returns>
        /// IQueryable of RessourceRecommandeeEnt
        /// </returns>
        public IQueryable<RessourceRecommandeeEnt> Include(params Expression<Func<RessourceRecommandeeEnt, object>>[] includeExpressions)
        {
            IQueryable<RessourceRecommandeeEnt> query = null;
            foreach (var includeExpression in includeExpressions)
            {
                query = this.dbSet.AsNoTracking().Include(includeExpression);
            }
            return query ?? this.dbSet;
        }

        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// IQueryable of RessourceRecommandeeEnt
        /// </returns>
        public IQueryable<RessourceRecommandeeEnt> Where(Expression<Func<RessourceRecommandeeEnt, bool>> predicate)
        {
            var resultEntities = this.dbSet.AsNoTracking().Where(predicate);
            return resultEntities;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>int</returns>
        private int SaveChanges(RessourceRecommandeeEnt entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry != null && entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }

            return this.Context.SaveChanges();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>int</returns>
        private int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        /// <summary>
        /// Récupère la liste des ressources recommandées correspondant aux référentiels étendus
        /// </summary>
        /// <param name="etablissementCIOrganisationId">Identifiant de l'organisation à laquelle l'établissement comptable du CI courant appartient</param>
        /// <returns>Une liste de ressources recommandées</returns>
        public List<RessourceRecommandeeFromEtablissementCIOrganisationModel> GetRessourceRecommandeeList(int etablissementCIOrganisationId)
        {
            return Context.RessourceRecommandees
                .Where(rr => rr.OrganisationId == etablissementCIOrganisationId)
                .Select(rr => new RessourceRecommandeeFromEtablissementCIOrganisationModel { ReferentielEtenduId = rr.ReferentielEtenduId })
                .ToList();
        }
    }
}
