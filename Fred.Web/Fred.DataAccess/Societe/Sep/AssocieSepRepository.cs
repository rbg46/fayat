using Fred.DataAccess.Common;
using Fred.Entities;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using Fred.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.DataAccess.Societe
{
    /// <summary>
    /// Repository Associé SEP
    /// </summary>
    public class AssocieSepRepository : FredRepository<AssocieSepEnt>, IAssocieSepRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="AssocieSepRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="context">Inject context</param>
        public AssocieSepRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<IList<AssocieSepEnt>> GetAssocieSepAsync(int societeId)
        {
            return await context.AssocieSeps
                .Where(a => a.SocieteId == societeId)
                .Include(a => a.TypeParticipationSep)
                .Include(a => a.SocieteAssociee)
                .ToListAsync();
        }

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
        public List<AssocieSepEnt> Search(List<Expression<Func<AssocieSepEnt, bool>>> filters,
                                                Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null,
                                                List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = false)
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
