using System.Collections.Generic;
using System.Linq;
using Fred.Business.DatesClotureComptable.Reception.Models;
using Fred.Business.DatesClotureComptable.Services;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    public class DateTransfertFarProviderService : IDateTransfertFarProviderService
    {
        private readonly IDatesProviderForReceptionService datesProviderForReceptionService;

        public DateTransfertFarProviderService(IDatesProviderForReceptionService datesProviderForReceptionService)
        {
            this.datesProviderForReceptionService = datesProviderForReceptionService;
        }

        /// <summary>
        /// Met les date Transfert Far dans les receptions, elle est issue des datecloturecomptables
        /// </summary>
        /// <param name="receptions">receptions</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        public void SetDateTransfertFarOfReceptions(List<DepenseAchatEnt> receptions, int year, int month)
        {
            var receptionBlockedResponses = GetDateTransfertFars(receptions, year, month);

            foreach (var reception in receptions)
            {
                if (reception.CiId.HasValue)
                {
                    var dateTransfertFar = receptionBlockedResponses.First(x => x.Resquest.CiId == reception.CiId
                                                    && x.Resquest.Year == year
                                                    && x.Resquest.Month == month);

                    reception.DateTransfertFar = dateTransfertFar.DateTransfertFar;
                }
            }
        }

        private List<GetDateTransfertFarResponse> GetDateTransfertFars(List<DepenseAchatEnt> receptions, int year, int month)
        {
            var requests = new List<GetDateTransfertFarResquest>();
            foreach (var reception in receptions)
            {
                requests.Add(new GetDateTransfertFarResquest()
                {
                    CiId = reception.CiId.Value,
                    Year = year,
                    Month = month
                });
            }
            return datesProviderForReceptionService.GetDateTransfertFars(requests);
        }
    }
}
