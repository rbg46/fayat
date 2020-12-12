using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;

namespace Fred.Business.Achat.Calculation
{
    public class MontantHtService : IMontantHtService
    {
        private readonly IMontantHtRepository montantHtRepository;

        public MontantHtService(IMontantHtRepository montantHtRepository)
        {
            this.montantHtRepository = montantHtRepository;
        }

        /// <summary>
        /// Calcule le mont HT receptionne des commandes
        /// </summary>
        /// <param name="commandesToMustBeCalculatedAndMapped">Commandes dont on doit calcule le Montant HT receptiionnes</param>       
        public void GetAndMapMontantHT(List<CommandeEnt> commandesToMustBeCalculatedAndMapped)
        {
            if (commandesToMustBeCalculatedAndMapped != null)
            {
                var commandeIds = commandesToMustBeCalculatedAndMapped.Select(x => x.CommandeId).Distinct().ToList();

                var commandeMontantHTResponses = this.montantHtRepository.GetMontantHT(x => commandeIds.Contains(x.CommandeId));

                // faire un dictionnaire accroit considerablement les performances de la boucle surtout s'il y a bq d'elements.
                var commandeMontantHTDictionnary = commandeMontantHTResponses.ToDictionary(x => x.CommandeId);

                foreach (var commandeToMap in commandesToMustBeCalculatedAndMapped)
                {
                    var resultOfRequest = commandeMontantHTDictionnary[commandeToMap.CommandeId];

                    commandeToMap.MontantHT = resultOfRequest.MontantHT;
                }
            }
        }

        public decimal CalculateMontantHtTotal(List<int> receptionsIdsMustBeCalculated)
        {
            if (receptionsIdsMustBeCalculated == null)
                throw new System.ArgumentNullException(nameof(receptionsIdsMustBeCalculated));

            var depenseAchatMontants = this.montantHtRepository.GetMontantHT(x => receptionsIdsMustBeCalculated.Contains(x.DepenseId));
            var result = depenseAchatMontants.Sum(x => x.MontantHt);
            return result;
        }
    }
}
