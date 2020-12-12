using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Common.Tests
{
    /// <summary>
    /// Visiteur des méthodes
    /// </summary>
    public class MethodCallVisitor : ExpressionVisitor
    {
        private readonly Type _from;

        private readonly Type _to;

        /// <summary>
        /// Initialise une instance de <see cref="MethodCallVisitor"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public MethodCallVisitor(Type from, Type to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Permet de changer le comportement des méthodes
        /// </summary>
        /// <param name="node"> noeud des méthodees de l'expression</param>
        /// <returns>expression</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Method.DeclaringType == _from)
            {
                var typeArguments = node.Method.GetGenericArguments();
                var arguments = node.Arguments.Select(Visit).ToArray();
                return Expression.Call(_to, node.Method.Name, typeArguments, arguments);
            }
            return base.VisitMethodCall(node);
        }
    }
}
