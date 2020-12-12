using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.Business.Achat.Calculation.Interfaces
{
    /// <summary>
    /// Service qui calcule le montant HT receptionne des commandes
    /// </summary>
    public interface IMontantHtReceptionneService : IService
    {
        /// <summary>
        /// Calcule  et mappe le montant HT receptionne des commandes
        /// </summary>
        /// <param name="commandesToMustBeCalculatedAndMapped">Commandes dont on doit calcule le Montant HT receptiionnes</param>    
        void GetAndMapMontantHTReceptionne(List<CommandeEnt> commandesToMustBeCalculatedAndMapped);
    }
}
