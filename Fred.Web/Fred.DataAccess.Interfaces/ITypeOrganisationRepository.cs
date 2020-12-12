using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Organisation;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repo des TypeOrganisationEnt
    /// </summary>
    public interface ITypeOrganisationRepository : IRepository<TypeOrganisationEnt>
    {
        /// <summary>
        /// Recherche des TypeOrganisationEnt selon les filtres definis
        /// </summary>
        /// <param name="filters">les filtres</param>
        /// <param name="orderBy">les orderby</param>
        /// <param name="includeProperties">les includes</param>
        /// <param name="page">la page corrante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Liste de ci</returns>
        List<TypeOrganisationEnt> Search(List<Expression<Func<TypeOrganisationEnt, bool>>> filters,
                                             Func<IQueryable<TypeOrganisationEnt>, IOrderedQueryable<TypeOrganisationEnt>> orderBy = null,
                                             List<Expression<Func<TypeOrganisationEnt, object>>> includeProperties = null,
                                             int? page = null,
                                             int? pageSize = null,
                                             bool asNoTracking = true);
    }
}
