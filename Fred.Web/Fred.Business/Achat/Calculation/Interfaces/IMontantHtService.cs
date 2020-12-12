using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.Business.Achat.Calculation
{
    public interface IMontantHtService
    {
        void GetAndMapMontantHT(List<CommandeEnt> commandesToMustBeCalculatedAndMapped);
        decimal CalculateMontantHtTotal(List<int> receptionsIdsMustBeCalculated);
    }
}
