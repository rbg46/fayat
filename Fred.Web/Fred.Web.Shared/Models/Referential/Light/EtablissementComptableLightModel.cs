using Fred.Web.Models.Organisation;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente un établissement comptable
    /// </summary>
    public class EtablissementComptableLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un établissement comptable.
        /// </summary>    
        public int EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation de l'établissement
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation de l'établissement
        /// </summary>
        public OrganisationLightModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société de l'établissement
        /// </summary>
        public SocieteLightModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement comptable.
        /// </summary>    
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'établissement comptable.
        /// </summary>    
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant les ressources recommandées sont gérées pour l'établissement comptable
        /// </summary>
        public bool RessourcesRecommandeesEnabled { get; set; }

        /// <summary>
        /// Obtient ou définit la Facturation de l'établissement comptable.
        /// </summary>
        public string Facturation { get; set; }

        /// <summary>
        /// Obtient ou définit le Paiement de l'établissement comptable.
        /// </summary>
        public string Paiement { get; set; }
    }
}
