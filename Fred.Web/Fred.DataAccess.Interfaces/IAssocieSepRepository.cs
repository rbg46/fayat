using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface repo Societe Associé SEP
    /// </summary>
    public interface IAssocieSepRepository : IFredRepository<AssocieSepEnt>
    {
        /// <summary>
        ///   Retourner la requête de récupération des associés SEP
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<AssocieSepEnt> Search(List<Expression<Func<AssocieSepEnt, bool>>> filters,
                                                Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null,
                                                List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = false);

        /// <summary>
        /// Get SocieteAssocie list by societe id
        /// </summary>
        /// <param name="societeId">societe id</param>
        /// <returns>AssocieSepEnt list</returns>
        Task<IList<AssocieSepEnt>> GetAssocieSepAsync(int societeId);
    }
}
