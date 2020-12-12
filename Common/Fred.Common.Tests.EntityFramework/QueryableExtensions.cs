using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fred.Common.Tests.EntityFramework
{
    /// <summary>
    /// Helper pour les <see cref="IQueryable"/> 
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Permet de subtituer les méthodes DbFunctions
        /// </summary>
        /// <typeparam name="T">type de la query data</typeparam>
        /// <param name="value">query à modifier</param>
        /// <param name="useInsteadType">type à utilisé à la place</param>
        /// <returns>query substitué</returns>
        public static IQueryable<T> RewriteDbFunctions<T>(this IQueryable<T> value, Type useInsteadType)
        {
            return value.MethodsRewrite(typeof(DbFunctions), useInsteadType);
        }

        /// <summary>
        /// Permet de réécrire les functions
        /// </summary>
        /// <typeparam name="T">type de la query data</typeparam>
        /// <param name="value">query à modifier</param>
        /// <param name="from">type à remplacer</param>
        /// <param name="to">type à utilisé à la place</param>
        /// <returns>query substitué</returns>
        public static IQueryable<T> MethodsRewrite<T>(this IQueryable<T> value, Type from, Type to)
        {
            var visitor = new MethodCallVisitor(from, to);
            var provider = new QueryProviderProxy(value.Provider, visitor);
            return new RewriteQueryable<T>(value, provider);

        }
    }
}
