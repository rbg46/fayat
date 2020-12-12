using System;
using Fred.Entities;
using Fred.Web.Models.Utilisateur;

namespace Fred.Business.Template.Models.Avis
{
    /// <summary>
    /// Modèle Avis utilisé par le template
    /// </summary>
    public class AvisTemplateModel
    {
        /// <summary>
        /// Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le type de l'avis
        /// </summary>
        public TypeAvis TypeAvis { get; set; }

        /// <summary>
        /// Obtient ou définit l'expéditeur de l'avis
        /// </summary>
        public string Expediteur { get; set; }

        /// <summary>
        /// Obtient ou définit le destinataire de l'avis
        /// </summary>
        public string Destinataire { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de l'avis
        /// </summary>
        public string DateCreation { get; set; }
    }
}
