using Fred.Entities.EntityBase;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Avis
{
    /// <summary>
    /// Entité représentant un avis
    /// </summary>
    public class AvisEnt : AuditableEntity
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
        public UtilisateurEnt Expediteur { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du destinataire
        /// </summary>
        public int? DestinataireId { get; set; }

        /// <summary>
        /// Obtient ou définit le destinataire de l'avis
        /// </summary>
        public UtilisateurEnt Destinataire { get; set; }
    }
}
