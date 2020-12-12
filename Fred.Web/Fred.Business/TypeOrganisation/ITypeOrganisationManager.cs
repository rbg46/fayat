using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Organisation;

namespace Fred.Business.TypeOrganisation
{
    /// <summary>
    /// Manager des TypeOrganisationEnt
    /// </summary>
    public interface ITypeOrganisationManager : IManager<TypeOrganisationEnt>
    {
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
        List<TypeOrganisationEnt> Search(List<Expression<Func<TypeOrganisationEnt, bool>>> filters,
                                            Func<IQueryable<TypeOrganisationEnt>, IOrderedQueryable<TypeOrganisationEnt>> orderBy = null,
                                            List<Expression<Func<TypeOrganisationEnt, object>>> includeProperties = null,
                                            int? page = null,
                                            int? pageSize = null,
                                            bool asNoTracking = true);

        /// <summary>
        /// Retourne la liste des types d'organisation
        /// </summary>
        /// <returns>Liste de type</returns>bool asNoTracking = true);
        List<TypeOrganisationEnt> GetAll();

    }
}
