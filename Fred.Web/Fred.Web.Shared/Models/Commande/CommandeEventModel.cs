using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Web.Shared.Enum;

namespace Fred.Web.Shared.Models.Commande
{
    public class CommandeEventModel
    {
        /// <summary>
        /// Titre
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type de l'évènement
        /// </summary>
        public TypeCommandeEvent TypeCommandeEvent { get; set; }

        /// <summary>
        /// Auteur de l'évènement
        /// </summary>
        public string Auteur { get; set; }

        /// <summary>
        /// Date de l'évènement
        /// </summary>
        public DateTime? Creation { get; set; }

        /// <summary>
        /// Date de l'évènement
        /// </summary>
        public string CreationFormatted => Creation.HasValue ? Creation.Value.ToString("dd/MM/yyyy") : "-";

        /// <summary>
        /// Destinataire de l'évènement
        /// </summary>
        public string Destinataire { get; set; }

        /// <summary>
        /// Type de l'avis
        /// </summary>
        public TypeAvis TypeAvis { get; set; }

        /// <summary>
        /// Type de l'avis
        /// </summary>
        public string TypeAvisFormatted => TypeAvis.GetDescription();

        /// <summary>
        /// Commentaire
        /// </summary>
        public string Commentaire { get; set; }

    }
}
