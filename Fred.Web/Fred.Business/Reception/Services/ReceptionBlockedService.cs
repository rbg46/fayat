using System.Collections.Generic;
using System.Linq;
using Fred.Business.DatesClotureComptable.Reception.Models;
using Fred.Business.DatesClotureComptable.Services;
using Fred.Entities.Depense;
namespace Fred.Business.Reception.Services
{
    public class ReceptionBlockedService : IReceptionBlockedService
    {
        private readonly IDatesProviderForReceptionService dateClotureForReceptionService;

        public ReceptionBlockedService(IDatesProviderForReceptionService dateClotureForReceptionService)
        {
            this.dateClotureForReceptionService = dateClotureForReceptionService;
        }

        /// <summary>
        ///   Détermine s'il y a au moins une réception dont la date est comprise dans une période bloquée en réception
        /// </summary>
        /// <param name="receptions">Liste des réceptions</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>
        public bool CheckAnyReceptionsIsBlocked(List<DepenseAchatEnt> receptions)
        {
            var receptionBlockedResponses = GetIsBlockedInReceptionData(receptions);

            var receptionBlockeds = GetReceptions(receptionBlockedResponses, receptions, blocked: true);

            return receptionBlockeds.Count > 0;
        }


        /// <summary>
        /// Met a jours la dateComptable d'une liste de receptions.
        /// Si la ci est cloturé, alors c'est le prochain mois dipsonible qui est pris.
        /// </summary>
        /// <param name="receptions">receptions a mettre a jour</param>
        public void SetDateComtapleOfReceptions(List<DepenseAchatEnt> receptions)
        {
            var receptionBlockedResponses = GetIsBlockedInReceptionData(receptions);

            var receptionBlockeds = GetReceptions(receptionBlockedResponses, receptions, blocked: true);

            var nextDatesInfos = GetNextDatesUnblockedInReception(receptionBlockeds);

            var receptionNotBlockeds = GetReceptions(receptionBlockedResponses, receptions, blocked: false);

            foreach (var receptionBlocked in receptionBlockeds)
            {
                receptionBlocked.DateComptable = nextDatesInfos.First(x => x.Resquest.CiId == receptionBlocked.CiId.Value && x.Resquest.StartDate == receptionBlocked.Date.Value).NextDate;
            }

            foreach (var receptionNotBlocked in receptionNotBlockeds)
            {
                receptionNotBlocked.DateComptable = receptionNotBlocked.Date;
            }
        }


        private List<DepenseAchatEnt> GetReceptions(List<ReceptionBlockedResponse> receptionBlockedResponses, List<DepenseAchatEnt> receptions, bool blocked)
        {
            var results = new List<DepenseAchatEnt>();
            foreach (var reception in receptions)
            {
                if (receptionBlockedResponses.Any(x => x.Resquest.CiId == reception.CiId
                                                   && x.Resquest.Year == reception.Date.Value.Year
                                                   && x.Resquest.Month == reception.Date.Value.Month
                                                   && x.IsBlockedInReception == blocked))
                {
                    results.Add(reception);
                }

            }
            return results;
        }

        private List<ReceptionBlockedResponse> GetIsBlockedInReceptionData(List<DepenseAchatEnt> receptions)
        {
            var requests = new List<ReceptionBlockedResquest>();
            foreach (var reception in receptions)
            {
                requests.Add(new ReceptionBlockedResquest()
                {
                    CiId = reception.CiId.Value,
                    Year = reception.Date.Value.Year,
                    Month = reception.Date.Value.Month
                });
            }
            return dateClotureForReceptionService.IsBlockedInReception(requests);
        }

        private List<NextDateUnblockedInReceptionResponse> GetNextDatesUnblockedInReception(List<DepenseAchatEnt> receptions)
        {
            var requests = new List<NextDateUnblockedInReceptionResquest>();
            foreach (var reception in receptions)
            {
                requests.Add(new NextDateUnblockedInReceptionResquest()
                {
                    CiId = reception.CiId.Value,
                    StartDate = reception.Date.Value
                });
            }
            return dateClotureForReceptionService.GetNextDatesUnblockedInReception(requests);
        }

    }
}
