using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Entities.Search
{
    /// <summary>
    /// Classe abstraite utilisé pour les recherches
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité sur laquelle nous faisons la recherche</typeparam>
    [Serializable]
    public abstract class AbstractSearchEnt<TEntity> : AbstractSearch where TEntity : class
    {
        /// <summary>
        /// Permet de récupérer le prédicat de recherche.
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
        public abstract Expression<Func<TEntity, bool>> GetPredicateWhere();

        /// <summary>
        /// Appliquer le tri (utilisateur et/ou interne) au résultat.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Entité triée</returns>
        public IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query)
        {
            var orderBy = GetUserOrderBy();
            var orderedQuery = orderBy.ApplyOrderBy(query);
            var defaultOrderBy = GetDefaultOrderBy();
            if (defaultOrderBy != null)
            {
                orderedQuery = defaultOrderBy.ApplyThenBy(orderedQuery);
            }
            return orderedQuery;
        }

        /// <summary>
        /// Retourner le tri par défaut (tri interne).
        /// </summary>
        /// <returns>Le tri</returns>
        protected abstract IOrderer<TEntity> GetDefaultOrderBy();

        /// <summary>
        /// Retourner le tri défini par l'utilisateur.
        /// </summary>
        /// <returns>Le tri</returns>
        protected abstract IOrderer<TEntity> GetUserOrderBy();
    }
}
