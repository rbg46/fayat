using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Entities.Search
{
    /// <summary>
    ///   Classe permettant de gérer les tris
    /// </summary>
    /// <typeparam name="TSource">1er ordre</typeparam>
    /// <typeparam name="TKey">2eme ordre</typeparam>
    public class Orderer<TSource, TKey> : IOrderer<TSource>
    {
        private readonly bool asc;
        private readonly Expression<Func<TSource, TKey>> orderExpression;
        private readonly List<Expression<Func<TSource, TKey>>> orderExpressionList;

        public Orderer(Expression<Func<TSource, TKey>> orderExpression, bool asc)
        {
            this.orderExpression = orderExpression;
            this.asc = asc;
        }

        public Orderer(List<Expression<Func<TSource, TKey>>> orderExpressionList, bool asc)
        {
            this.orderExpressionList = orderExpressionList;
            this.asc = asc;
        }

        public IOrderedQueryable<TSource> ApplyOrderBy(IQueryable<TSource> source)
        {
            if (asc)
            {
                return CreateOrderByExpr(source, Queryable.OrderBy, Queryable.ThenBy);
            }

            return CreateOrderByExpr(source, Queryable.OrderByDescending, Queryable.ThenByDescending);
        }

        private IOrderedQueryable<TSource> CreateOrderByExpr(
            IQueryable<TSource> source,
            Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>> orderByFunc,
            Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>> thenByFunc)
        {
            if (orderExpressionList != null)
            {
                return CreateOrderByExprOnList();
            }

            return CreateOrderByExprOnSingleProperty();

            IOrderedQueryable<TSource> CreateOrderByExprOnList()
            {
                IOrderedQueryable<TSource> query = orderByFunc(source, orderExpressionList.First());

                return orderExpressionList.Skip(1).Aggregate(query, thenByFunc);
            }

            IOrderedQueryable<TSource> CreateOrderByExprOnSingleProperty() => orderByFunc(source, orderExpression);
        }

        public IOrderedEnumerable<TSource> ApplyOrderBy(IEnumerable<TSource> source)
        {
            if (asc)
            {
                return source.OrderBy(orderExpression.Compile());
            }

            return source.OrderByDescending(orderExpression.Compile());
        }

        public IOrderedQueryable<TSource> ApplyThenBy(IOrderedQueryable<TSource> source)
        {
            if (asc)
            {
                return CreateThenByExpr(source, Queryable.ThenBy);
            }

            return CreateThenByExpr(source, Queryable.ThenByDescending);
        }

        private IOrderedQueryable<TSource> CreateThenByExpr(
            IOrderedQueryable<TSource> source,
            Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>> thenByFunc)
        {
            if (orderExpressionList != null)
            {
                return CreateThenByExprOnList();
            }

            return CreateThenByExprOnSingleProperty();

            IOrderedQueryable<TSource> CreateThenByExprOnList() => orderExpressionList.Aggregate(source, thenByFunc);

            IOrderedQueryable<TSource> CreateThenByExprOnSingleProperty() => thenByFunc(source, orderExpression);
        }
    }
}
