using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui permet gere les api pour la page 'tableau des reception'
    /// </summary>
    public class ReceptionUpdateOrchestrator : IReceptionUpdateOrchestrator
    {
        private readonly IReceptionManager receptionManager;
        private readonly IVisableReceptionProviderService visableReceptionProviderService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="receptionManager">receptionManager</param>
        /// <param name="visableReceptionProviderService">visableReceptionProviderService</param>
        public ReceptionUpdateOrchestrator(IReceptionManager receptionManager,
                                              IVisableReceptionProviderService visableReceptionProviderService)
        {
            this.receptionManager = receptionManager;
            this.visableReceptionProviderService = visableReceptionProviderService;
        }


        /// <summary>
        /// Met a jour une liste de receptions
        /// </summary>
        /// <param name="frontReceptions">les reception front </param>       
        public void UpdateReceptions(List<DepenseAchatEnt> frontReceptions)
        {

            var frontReceptionsIds = frontReceptions.Select(r => r.DepenseId).ToList();
            // mise a jour des receptions qui sont visables seulement
            var visableReceptionsResponse = visableReceptionProviderService.GetReceptionsVisables(frontReceptionsIds);

            var visableReceptionIds = visableReceptionsResponse.ReceptionsVisables.Select(r => r.DepenseId).ToList();

            var frontReceptionVisablesToUpdates = frontReceptions.Where(r => visableReceptionIds.Contains(r.DepenseId)).ToList();

            // validation et sauvegarde
            receptionManager.UpdateAndSaveWithValidation(frontReceptionVisablesToUpdates);

        }


        /// <summary>
        /// Met a jour une reception
        /// </summary>
        /// <param name="frontReception">la reception front </param>
        /// <returns>la reception back</returns>
        public DepenseAchatEnt UpdateReception(DepenseAchatEnt frontReception)
        {
            receptionManager.UpdateAndSaveWithValidation(frontReception);

            var receptionsIds = new List<int>()
                {
                    frontReception.DepenseId
                };

            List<Expression<Func<DepenseAchatEnt, bool>>> filters = new List<Expression<Func<DepenseAchatEnt, bool>>>
                {
                    x => receptionsIds.Contains(x.DepenseId)
                };

            List<Expression<Func<DepenseAchatEnt, object>>> includePorperties = new List<Expression<Func<DepenseAchatEnt, object>>>
                {
                    x => x.Tache,
                    x => x.Unite,
                    x => x.Devise,
                    x => x.CI,
                    x => x.DepenseType,
                    x => x.PiecesJointesReception,
                    x => x.Ressource.ReferentielEtendus.Select(y => y.Nature)
                };

            var dbReceptions = receptionManager.Get(filters, null, includePorperties);

            return dbReceptions.First();
        }

    }
}
