using Fred.Web.Models.Organisation;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente une ci
    /// </summary>
    public class CILightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ci.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation du ci
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation du ci
        /// </summary>
        public OrganisationLightModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'établissement comptable du ci.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité établissement comptable de l'ci.
        /// </summary>
        public EtablissementComptableLightModel EtablissementComptable { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société de l'ci
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société de l'ci
        /// </summary>
        public SocieteLightModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une ci.
        /// </summary>    
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une ci.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une ci est une SEP.
        /// </summary>    
        public bool Sep { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle { get; set; }
    }
}