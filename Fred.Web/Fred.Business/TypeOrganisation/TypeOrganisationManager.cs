using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;

namespace Fred.Business.TypeOrganisation
{
    /// <summary>
    /// Manager des TypeOrganisationEnt
    /// </summary>
    public class TypeOrganisationManager : Manager<TypeOrganisationEnt, ITypeOrganisationRepository>, ITypeOrganisationManager
    {
        public TypeOrganisationManager(IUnitOfWork uow, ITypeOrganisationRepository typeOrganisationRepository)
          : base(uow, typeOrganisationRepository)
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

            return this.Repository.Search(filters, orderBy, includeProperties, page, pageSize);

        }

        /// <summary>
        /// Retourne la liste des types d'organisation
        /// </summary>
        /// <returns>Liste de type</returns>
        public List<TypeOrganisationEnt> GetAll()
        {
            var filters = new List<Expression<Func<TypeOrganisationEnt, bool>>>
            {
                x => true
            };
            return this.Search(filters);

        }
    }
}
