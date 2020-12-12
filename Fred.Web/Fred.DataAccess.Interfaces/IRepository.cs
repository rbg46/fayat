using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.DataAccess.Interfaces
{
    public interface IRepository
    {
        void DeleteById(object id);

        void RemoveAttachedEntityFromContext(int entityId);
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        TEntity FindById(object id);

        Task<TEntity> FindByIdAsync(object id);

        TEntity Insert(TEntity entity);
        void InsertRange(List<TEntity> entities);

        void InsertOrUpdate(Func<TEntity, object> identifierExpression, IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Update(TEntity entity, List<Expression<Func<TEntity, object>>> fieldsToUpdate);
        void UpdateRange(List<TEntity> entities);

        void Delete(TEntity entity, bool onlyIfAttashed = false);

        IRepositoryQuery<TEntity> Query();

        void PerformEagerLoading(TEntity entity, params Expression<Func<TEntity, object>>[] expressions);

        void Replace(TEntity entity1, TEntity entity2);

        IQueryable<TEntity> Get();

        /// <summary>
        /// Récupère les valeurs de la colonne passée en paramètre, en appliquant le filtre passé dans le paramètre where
        /// </summary>
        /// <remarks>
        /// il est tout à fait possible d'ajouter après l'appel à la fonction un First() ou un FirstOrDefault() si besoin
        /// </remarks>
        /// <example>
        /// CiRepository.SelectOne(ci => ci.Organisation.OrganisationId, ci => !ci.DateFermeture.HasValue || ci.CiId == ciId);
        /// BudgetRepository.SelectOne(b => b.PeriodeDebut, b => b.BudgetId == budgetId).FirstOrDefault();
        /// </example>
        /// <typeparam name="TResult">Ce type est déduit par le compilateur à partir du type de la propriété passée</typeparam>
        /// <param name="column">Colonne à récupérer</param>
        /// <param name="where">conditions permettant de délimiter les valeurs à récupérer</param>
        /// <returns>Une liste potentiellement vide mais jamais null contenant toutes les valeurs de cette colonne pour la condition donnée</returns>
        IEnumerable<TResult> SelectOneColumn<TResult>(Expression<Func<TEntity, TResult>> column, Expression<Func<TEntity, bool>> where);

        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}
