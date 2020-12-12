using System.Collections.Generic;
using Fred.Web.Models.Commande;

namespace Fred.Web.Shared.Models.Valorisation
{
    public class ValorisationMaterielModel
    {
        public int MaterielId { get; set; }

        public bool IsMaterielLocation { get; set; }

        public ICollection<CommandeLigneModel> CommandeLignes { get; set; }

        public int SocieteId { get; set; }

        public int RessourceId { get; set; }

        public bool IsStorm { get; set; }
    }
}

