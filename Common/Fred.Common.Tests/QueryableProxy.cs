using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Common.Tests
{
    /// <summary>
    /// Proxy pour une query
    /// </summary>
    public abstract class QueryableProxy : IOrderedQueryable
    {
        /// <summary>
        /// query
        /// </summary>
        public IQueryable Query { get; }

        /// <summary>
        /// provider substitué
        /// </summary>
        public QueryProviderProxy Provider { get; }

        /// <summary>
        /// Initialise une instance de <see cref="QueryableProxy"/>
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="provider">provider substitué</param>
        protected QueryableProxy(IQueryable query, QueryProviderProxy provider)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            Query = query;
            Provider = provider;
        }

        /// <summary>
        /// Obtient l'Enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // rewrite on enumeration
            return Provider.RewriteQuery(Expression).GetEnumerator();
        }

        /// <summary>
        /// Obtient l'Enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator GetEnumerator()
        {       
            return GetEnumerator();
        }

        /// <summary>
        /// Obtient le Type de classe
        /// </summary>
        public Type ElementType => Query.ElementType;

        /// <summary>
        /// Obtient l'expression
        /// </summary>
        public Expression Expression => Query.Expression;

        /// <summary>
        /// Obtient le provider substitué
        /// </summary>
        IQueryProvider IQueryable.Provider => Provider;

    }

    /// <summary>
    /// Proxy pour une query
    /// </summary>
    public class RewriteQueryable<T> : QueryableProxy, IOrderedQueryable<T>
    {

        /// <summary>
        /// Initialise une instance de <see cref="QueryableProxy"/>
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="provider">provider substitué</param>
        public RewriteQueryable(IQueryable query, QueryProviderProxy provider)
            : base(query, provider)
        {

        }

        /// <summary>
        /// Obtient l'Enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            // rewrite on enumeration
            return Provider.RewriteQuery<T>(Expression).GetEnumerator();
        }
    }
}
