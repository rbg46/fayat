using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Shared.Models.Avis
{
    /// <summary>
    /// Modèle d'une avis
    /// </summary>
    public class AvisModel
    {
        /// <summary>
        /// Obtient ou définti l'identifiant de l'avis
        /// </summary>
        public int? AvisId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le type de l'avis
        /// </summary>
        public TypeAvis TypeAvis { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de l'expéditeur
        /// </summary>
        public int? ExpediteurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'expéditeur
        /// </summary>
        public UtilisateurModel Expediteur { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du destinataire
        /// </summary>
        public int? DestinataireId { get; set; }

        /// <summary>
        /// Obtient ou définit le destinataire de l'avis
        /// </summary>
        public UtilisateurModel Destinataire { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeAvenantId { get; set; }
        
    }
}
