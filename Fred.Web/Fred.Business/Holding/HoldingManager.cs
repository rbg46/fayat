using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Holding;
namespace Fred.Business.Holding
{
    /// <summary>
    ///   Gestionnaire des sociétés.
    /// </summary>
    public class HoldingManager : Manager<HoldingEnt, IHoldingRepository>, IHoldingManager
    {
        public HoldingManager(IUnitOfWork uow, IHoldingRepository holdingRepository)
          : base(uow, holdingRepository)
        {

        }

        /// <summary>
        ///   Retourne la liste des sociétés.
        /// </summary>
        /// <returns>Liste des sociétés.</returns>
        public IEnumerable<HoldingEnt> GetHoldings()
        {
            return Repository.GetHoldings();
        }

        /// <summary>
        ///   Retourne le société dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="holdingId">Identifiant du société à retrouver.</param>
        /// <returns>Le société retrouvée, sinon nulle.</returns>
        public HoldingEnt GetHolding(int holdingId)
        {
            return Repository.GetHolding(holdingId);
        }

        /// <summary>
        /// Retourne la holding portant l'organisation Id unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <returns>la holding retrouvée, sinon null.</returns>
        public HoldingEnt GetHoldingByOrganisationId(int organisationId)
        {
            return Repository.GetHoldingByOrganisationId(organisationId);
        }
    }
}