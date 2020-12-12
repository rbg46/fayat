using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Holding;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Holding
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class HoldingRepository : FredRepository<HoldingEnt>, IHoldingRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="HoldingRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public HoldingRepository(FredDbContext context)
          : base(context)
        {
        }


        /// <summary>
        /// Retourne la liste des holdings.
        /// </summary>
        /// <returns>Renvoie la liste des holdings.</returns>
        public IEnumerable<HoldingEnt> GetHoldings()
        {
            return Context.Holdings.ToList();
        }

        /// <summary>
        /// Retourne la holding portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="holdingId">Identifiant de la holding à retrouver.</param>
        /// <returns>la holding retrouvée, sinon null.</returns>
        public HoldingEnt GetHolding(int holdingId)
        {
            return Context.Holdings.FirstOrDefault(h => h.HoldingId == holdingId);
        }

        /// <summary>
        /// Retourne la holding portant l'organisation Id unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <returns>la holding retrouvée, sinon null.</returns>
        public HoldingEnt GetHoldingByOrganisationId(int organisationId)
        {
            return Context.Holdings.Include("Organisation").FirstOrDefault(h => h.Organisation.OrganisationId == organisationId);
        }
    }
}