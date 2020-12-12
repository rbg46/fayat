using Fred.Web.Models.Organisation;
using Fred.Web.Shared.Models;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente une société
    /// </summary>
    public class SocieteLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une société.
        /// </summary>    
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation de la société
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation de la société
        /// </summary>
        public OrganisationLightModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du groupe de la société
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code condensé de la société
        /// </summary>    
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la société
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du type de societe
        /// </summary>
        public int TypeSocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de societe
        /// </summary>
        public TypeSocieteModel TypeSociete { get; set; }
    }
}
