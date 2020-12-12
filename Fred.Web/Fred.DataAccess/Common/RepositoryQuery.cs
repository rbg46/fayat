using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Interfaces;

namespace Fred.DataAccess.Common
{

    /// <summary>
    ///   Classe permettant de requêter un repositoy
    ///   Les opréations possible sont: Filtrer, trier, Gérer les Includes, paginer
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité.</typeparam>
    public sealed class RepositoryQuery<TEntity> : IRepositoryQuery<TEntity> where TEntity : class
    {
        private readonly List<Expression<Func<TEntity, bool>>> filters;
        private readonly List<Expression<Func<TEntity, object>>> includeProperties;
        private readonly DbRepository<TEntity> _repository;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByQuerable;
        private int? page;
        private int? pageSize;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RepositoryQuery{TEntity}" />.
        /// </summary>
        /// <param name="repository">Le repository à requêter.</param>
        public RepositoryQuery(DbRepository<TEntity> repository)
        {
            this._repository = repository;
            this.includeProperties = new List<Expression<Func<TEntity, object>>>();
            this.filters = new List<Expression<Func<TEntity, bool>>>();
        }

        /// <summary>
        ///   Filtrer le repository.
        /// </summary>
        /// <param name="filter">Le filtre.</param>
        /// <returns>L'instance RepositoryQuery</returns>
        public IRepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            this.filters.Add(filter);
            return this;
        }

        /// <summary>
        ///   Trier une liste.
        /// </summary>
        /// <param name="orderBy">Les critères de tri.</param>
        /// <returns>L'instance RepositoryQuery</returns>
        public IRepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            this.orderByQuerable = orderBy;
            return this;
        }

        /// <summary>
        ///   Ajouter un include.
        /// </summary>
        /// <param name="expression">une expression de l'include.</param>
        /// <returns>L'instance RepositoryQuery</returns>
        public IRepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            this.includeProperties.Add(expression);
            return this;
        }

        /// <summary>
        ///   Gets the page.
        /// </summary>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">La taille de la page.</param>
        /// <returns>L'instance RepositoryQuery</returns>
        public IQueryable<TEntity> GetPage(int page, int pageSize)
        {
            int totalCount;
            return GetPage(page, pageSize, out totalCount);
        }

        /// <summary>
        ///   Gets the page.
        /// </summary>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">La taille de la page.</param>
        /// <param name="totalCount">le total de la requête.</param>
        /// <returns>L'instance RepositoryQuery</returns>
        public IQueryable<TEntity> GetPage(int page, int pageSize, out int totalCount)
        {
            this.page = page;
            this.pageSize = pageSize;
            totalCount = this._repository.Get(this.filters).Count();

            return this._repository.Get(this.filters, this.orderByQuerable, this.includeProperties, this.page, this.pageSize);
        }

        /// <summary>
        ///   Retourner la requête Queryable.
        /// </summary>
        /// <returns>La requête IQueryable</returns>
        public IQueryable<TEntity> Get()
        {
            return this._repository.Get(this.filters, this.orderByQuerable, this.includeProperties, this.page, this.pageSize);
        }

        public IQueryable<TEntity> Intersect(IQueryable<TEntity> repoIntersect)
        {
            return this.Intersect(repoIntersect);
        }

        public IQueryable<TEntity> Union(IQueryable<TEntity> repoUnion)
        {
            return this.Intersect(repoUnion);
        }
    }
}
