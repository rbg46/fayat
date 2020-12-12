using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Framework.Extensions
{
    /// <summary>
    /// Methodes d'extension des IQueryable
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Filtre une séquence de valeurs selon des prédicats.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de la source.</typeparam>
        /// <param name="source">Source à filtrer.</param>
        /// <param name="predicates">Fonctions permettant de tester chaque élément par rapport à une condition.</param>
        /// <returns>IQueryable qui contient les éléments de la séquence d'entrée qui satisfont aux conditions spécifiées par predicate</returns>
        public static IQueryable<TSource> WhereMulti<TSource>(this IQueryable<TSource> source, params Expression<Func<TSource, bool>>[] predicates)
        {
            foreach (var predicate in predicates)
            {
                source = source.Where(predicate);
            }
            return source;
        }
    }
}
