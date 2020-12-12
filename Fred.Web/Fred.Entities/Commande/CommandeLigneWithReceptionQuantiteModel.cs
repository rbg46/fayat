using System.Collections.Generic;
using System.Linq;

namespace Fred.Entities.Commande
{
    public class CommandeLigneWithReceptionQuantiteModel
    {
        public int CommandeLigneId { get; set; }

        public List<ReceptionQuantiteModel> Receptions { get; set; }

        public decimal Quantite { get; set; }

        public decimal GetQuantiteReceptionee()
        {
            return Receptions.Sum(r => r.Quantite);
        }
    }
}
