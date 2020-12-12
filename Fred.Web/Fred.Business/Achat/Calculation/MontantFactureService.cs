using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;

namespace Fred.Business.Achat.Calculation
{
    public class MontantFactureService : IMontantFactureService
    {
        private readonly IMontantFactureRepository montantFactureRepository;

        public MontantFactureService(IMontantFactureRepository montantFactureRepository)
        {
            this.montantFactureRepository = montantFactureRepository;
        }

        public void GetAndMapMontantFacture(List<DepenseAchatEnt> receptionsMustBeMappedWithMontantFacture, DateTime? dateDebut, DateTime? dateFin)
        {
            if (receptionsMustBeMappedWithMontantFacture != null)
            {
                var receptionsIds = receptionsMustBeMappedWithMontantFacture.Select(x => x.DepenseId).Distinct().ToList();

                var montantFactureForReceptionsResponses = this.montantFactureRepository.GetMontantFactureForReceptions(x => receptionsIds.Contains(x.DepenseId), dateDebut, dateFin);

                // faire un dictionnaire accroit considerablement les performances de la boucle surtout s'il y a bq d'elements.
                var montantFactureForReceptionsResponsesDictionnary = montantFactureForReceptionsResponses.ToDictionary(x => x.ReceptionId);

                foreach (var receptionToMap in receptionsMustBeMappedWithMontantFacture)
                {
                    var resultOfRequest = montantFactureForReceptionsResponsesDictionnary[receptionToMap.DepenseId];

                    receptionToMap.MontantFacture = resultOfRequest.MontantFacture;
                }
            }
        }

    }
}
