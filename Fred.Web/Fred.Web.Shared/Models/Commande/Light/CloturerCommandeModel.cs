using System;
using Fred.Web.Models.Commande;

namespace Fred.Web.Shared.Models.Commande.Light
{
    public class CloturerCommandeModel
    {
        public DateTime? DateCloture { get; set; }

        public bool IsStatutCloturee { get; set; }

        public bool IsStatutValidee { get; set; }

        public int? StatutCommandeId { get; set; }

        public StatutCommandeModel StatutCommande { get; set; } = null;

    }
}
