using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Framework.Extensions
{
    /// <summary>
    /// Methodes d'extension des IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Pagine une séquence.
        /// La séquence doit être triée avec un OrderBy avant d'utiliser cette fonction.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de la source.</typeparam>
        /// <param name="source">La source.</param>
        /// <param name="page">L'index de la page.</param>
        /// <param name="pageSize">La taille d'une page.</param>
        /// <returns>La séquence paginée.</returns>
        public static IEnumerable<TSource> Pagine<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Pagine une séquence.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de la source.</typeparam>
        /// <typeparam name="TKey">Type de la clé retournée par la fonction représentée par orderBy.</typeparam>
        /// <param name="source">La source.</param>
        /// <param name="orderBy">Le tri souhaité.</param>
        /// <param name="page">L'index de la page.</param>
        /// <param name="pageSize">La taille d'une page.</param>
        /// <returns>La séquence paginée.</returns>
        public static IEnumerable<TSource> Pagine<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> orderBy, int page, int pageSize)
        {
            return source
                .OrderBy(orderBy)
                .Pagine(page, pageSize);
        }

        /// <summary>
        /// Retourne une séquence qui contient des éléments dont certains champs contiennent une chaîne recherchée.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de la source.</typeparam>
        /// <typeparam name="TSearch">Le type qui représente les champs où rechercher.</typeparam>
        /// <param name="source">La source.</param>
        /// <param name="search">La chaîne à rechercher.</param>
        /// <param name="searchor">Indique les champs où rechercher.</param>
        /// <returns>La séquence qui contient les éléments recherchés.</returns>
        public static IEnumerable<TSource> Search<TSource, TSearch>(this IEnumerable<TSource> source, string search, Expression<Func<TSource, TSearch>> searchor)
        {
            return source.Where(searchor.GetSearchExpression(search).Compile());
        }
    }
}
