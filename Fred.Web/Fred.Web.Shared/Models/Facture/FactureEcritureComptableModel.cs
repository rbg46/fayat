using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Facture
{
    public class FactureEcritureComptableModel
    {
        public string NumeroFactureSAP { get; set; }
        public int? DepenseAchatReceptionId { get; set; }
        public int? DepenseAchatFactureEcartId { get; set; }
        public int? DepenseAchatFactureId { get; set; }
        public int? DepenseAchatFarId { get; set; }
    }
}
