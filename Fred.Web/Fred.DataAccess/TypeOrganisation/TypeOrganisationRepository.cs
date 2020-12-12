using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.TypeOrganisation
{
    /// <summary>
    /// Repo des TypeOrganisationEnt
    /// </summary>
    public class TypeOrganisationRepository : FredRepository<TypeOrganisationEnt>, ITypeOrganisationRepository
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logMgr">logMgr</param>
        public TypeOrganisationRepository(FredDbContext context)
        : base(context)
        {
        }

        /// <summary> 
        /// Fait une recherche sur les entités TypeOrganisationEnt
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="orderBy">orderBy</param>
        /// <param name="includeProperties">includeProperties</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>les type d'organisations</returns>
        public List<TypeOrganisationEnt> Search(List<Expression<Func<TypeOrganisationEnt, bool>>> filters,
                                             Func<IQueryable<TypeOrganisationEnt>, IOrderedQueryable<TypeOrganisationEnt>> orderBy = null,
                                             List<Expression<Func<TypeOrganisationEnt, object>>> includeProperties = null,
                                             int? page = null,
                                             int? pageSize = null,
                                             bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }
            else
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).ToList();
            }

        }

    }
}
