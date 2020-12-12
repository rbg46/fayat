using System.Collections.Generic;

namespace Fred.Entities.Achat.Calculation.Commande
{
    public class CommandeMontantHtModel
    {
        public int CommandeId { get; set; }
        public List<int> CommandLigneIds { get; set; }
        public decimal MontantHT { get; set; }
    }
}
