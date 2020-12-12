using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Fred.Business
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> firstExpr,
                                                            Expression<Func<T, bool>> secondExpr)
        {
            var invokedExpr = Expression.Invoke(secondExpr, firstExpr.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(firstExpr.Body, invokedExpr), firstExpr.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> firstExpr,
                                                             Expression<Func<T, bool>> secondExpr)
        {
            var invokedExpr = Expression.Invoke(secondExpr, firstExpr.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(firstExpr.Body, invokedExpr), firstExpr.Parameters);
        }
    }
}
