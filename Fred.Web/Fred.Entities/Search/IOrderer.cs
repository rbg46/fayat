using System.Collections.Generic;
using System.Linq;

namespace Fred.Entities.Search
{
    /// <summary>
    ///   Interface permettant de gérer les tris
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    public interface IOrderer<T>
    {
        /// <summary>
        ///   Appliquer le tri.
        /// </summary>
        /// <param name="source">La source de données.</param>
        /// <returns>Collection triée</returns>
        IOrderedQueryable<T> ApplyOrderBy(IQueryable<T> source);

        /// <summary>
        ///   Appliquer le tri sur une collection déjà triée.
        /// </summary>
        /// <param name="source">La source de données.</param>
        /// <returns>Collection triée</returns>
        IOrderedQueryable<T> ApplyThenBy(IOrderedQueryable<T> source);

        /// <summary>
        ///   Appliquer le tri.
        /// </summary>
        /// <param name="source">La source de données.</param>
        /// <returns>Collection triée</returns>
        IOrderedEnumerable<T> ApplyOrderBy(IEnumerable<T> source);
    }
}
