using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Common.Tests
{
    /// <summary>
    /// Proxy pour le query provider.
    /// </summary>
    public class QueryProviderProxy : IQueryProvider
    {
        /// <summary>
        /// query provider.
        /// </summary>
        public IQueryProvider Provider { get; }

        /// <summary>
        /// Visiteur des méthodes
        /// </summary>
        public ExpressionVisitor Rewriter { get; }

        /// <summary>
        /// Initialise une instance de <see cref="QueryProviderProxy"/>.
        /// </summary>
        /// <param name="provider">query provider</param>
        /// <param name="rewriter">Visiteur des méthodes</param>
        public QueryProviderProxy(IQueryProvider provider, ExpressionVisitor rewriter)
        {
            if (rewriter == null)
                throw new ArgumentNullException(nameof(rewriter));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            Provider = provider;
            Rewriter = rewriter;
        }

        /// <summary>
        /// Initialise une instance de la query substituée
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>query substituée</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            var query = Provider.CreateQuery(expression);
            return (IQueryable)Activator.CreateInstance(
                typeof(RewriteQueryable<>).MakeGenericType(query.ElementType),
                query, this);
        }

        /// <summary>
        /// Initialise une instance de la query substituée
        /// </summary>
        /// <typeparam name="TElement">type de donnée</typeparam>
        /// <param name="expression">query expression</param>
        /// <returns>query substituée</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            var query = Provider.CreateQuery<TElement>(expression);
            return new RewriteQueryable<TElement>(query, this);
        }

        /// <summary>
        /// Execute la substitution
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>expression substituée</returns>
        public object Execute(Expression expression)
        {
            return Provider.Execute(Rewrite(expression));
        }

        /// <summary>
        /// Execute la substitution
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>expression sub
        public TResult Execute<TResult>(Expression expression)
        {
            return Provider.Execute<TResult>(Rewrite(expression));
        }

        /// <summary>
        /// remplace l'expression.
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>expression substituée</returns>

        protected virtual Expression Rewrite(Expression expression)
        {
            return Rewriter.Visit(expression);
        }

        /// <summary>
        /// initialise une instance de <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="expression">query expression.</param>
        /// <returns>query substitué</returns>
        public virtual IQueryable RewriteQuery(Expression expression)
        {
            return Provider.CreateQuery(Rewrite(expression));
        }

        /// <summary>
        /// initialise une instance de <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="expression">query expression.</param>
        /// <returns>query substitué</returns>
        public virtual IQueryable<TElement> RewriteQuery<TElement>(Expression expression)
        {
            return Provider.CreateQuery<TElement>(Rewrite(expression));
        }
    }
}
