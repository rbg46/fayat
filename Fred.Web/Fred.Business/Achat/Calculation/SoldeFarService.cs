using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;

namespace Fred.Business.Achat.Calculation
{
    public class SoldeFarService : ISoldeFarService
    {
        private readonly ISoldeFarRepository soldeFarRepository;

        public SoldeFarService(ISoldeFarRepository soldeFarRepository)
        {
            this.soldeFarRepository = soldeFarRepository;
        }

        public List<int> SelectReceptionsWithSoldeFar(List<int> receptionsMustBeFiltered, DateTime? dateFrom, DateTime? dateTo)
        {
            var soldeFarResponses = soldeFarRepository.GetSoldeFar(x => receptionsMustBeFiltered.Contains(x.DepenseId), dateFrom, dateTo);
            var soldeFarResponsesDifferentToZero = soldeFarResponses.Where(x => !x.SoldeFar.Equals(0)).ToDictionary(x => x.ReceptionId);
            var result = receptionsMustBeFiltered.Where(x => soldeFarResponsesDifferentToZero.ContainsKey(x)).ToList();
            return result;
        }

        public decimal CalculateSoldeFarTotal(List<int> receptionsIdsMustBeCalculated, DateTime? dateFrom, DateTime? dateTo)
        {
            var soldeFarResponses = soldeFarRepository.GetSoldeFar(x => receptionsIdsMustBeCalculated.Contains(x.DepenseId), dateFrom, dateTo);
            var result = soldeFarResponses.Sum(x => x.SoldeFar);
            return result;
        }

        public void GetAndMapSoldeFar(List<DepenseAchatEnt> receptionsMustBeMappedWithSoldeFar, DateTime? dateDebut, DateTime? dateFin)
        {
            if (receptionsMustBeMappedWithSoldeFar != null)
            {
                var receptionsIds = receptionsMustBeMappedWithSoldeFar.Select(x => x.DepenseId).Distinct().ToList();

                var soldeFarsResponses = this.soldeFarRepository.GetSoldeFar(x => receptionsIds.Contains(x.DepenseId), dateDebut, dateFin);

                // faire un dictionnaire accroit considerablement les performances de la boucle surtout s'il y a bq d'elements.
                var soldeFarsResponsesDictionnary = soldeFarsResponses.ToDictionary(x => x.ReceptionId);

                foreach (var receptionToMap in receptionsMustBeMappedWithSoldeFar)
                {
                    var resultOfRequest = soldeFarsResponsesDictionnary[receptionToMap.DepenseId];

                    receptionToMap.SoldeFar = resultOfRequest.SoldeFar;
                }
            }
        }

    }
}
