using System.Collections.Generic;
using System.Linq;
using Fred.Business.Achat.Calculation.Interfaces;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;

namespace Fred.Business.Achat.Calculation
{
    /// <summary>
    /// Calule me 
    /// </summary>
    public class MontantHtReceptionneService : IMontantHtReceptionneService
    {
        private readonly IMontantHtReceptionneeRepository montantHtReceptionneeRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="montantHtReceptionneeRepository">montantHtReceptionneeRepository</param>
        public MontantHtReceptionneService(IMontantHtReceptionneeRepository montantHtReceptionneeRepository)
        {
            this.montantHtReceptionneeRepository = montantHtReceptionneeRepository;
        }

        /// <summary>
        /// Calcule le mont HT receptionne des commandes
        /// </summary>
        /// <param name="commandesToMustBeCalculatedAndMapped">Commandes dont on doit calcule le Montant HT receptiionnes</param>       
        public void GetAndMapMontantHTReceptionne(List<CommandeEnt> commandesToMustBeCalculatedAndMapped)
        {
            if (commandesToMustBeCalculatedAndMapped != null)
            {
                var commandeIds = commandesToMustBeCalculatedAndMapped.Select(x => x.CommandeId).Distinct().ToList();

                var commandeMontantHTReceptionnesResponse = this.montantHtReceptionneeRepository.GetMontantHTReceptionne(x => commandeIds.Contains(x.CommandeId));

                // faire un dictionnaire accroit considerablement les performances de la boucle surtout s'il y a bq d'elements.
                var commandeMontantHTReceptionnesDictionnary = commandeMontantHTReceptionnesResponse.ToDictionary(x => x.CommandeId);

                foreach (var commandeToMap in commandesToMustBeCalculatedAndMapped)
                {
                    var resultOfRequest = commandeMontantHTReceptionnesDictionnary[commandeToMap.CommandeId];

                    commandeToMap.MontantHTReceptionne = resultOfRequest.MontantHTReceptionne;
                }
            }
        }
    }
}
