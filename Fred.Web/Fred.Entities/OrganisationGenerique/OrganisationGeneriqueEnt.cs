using Fred.Entities.Organisation;

namespace Fred.Entities.OrganisationGenerique
{
    /// <summary>
    ///   Représente une organisation générique
    /// </summary>
    public class OrganisationGeneriqueEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organisation générique.
        /// </summary>
        public int OrganisationGeneriqueId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation de l'organisation generique.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet organisation attaché à une organisation générique
        /// </summary>  
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une organisation générique.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une organisation générique.
        /// </summary>
        public string Libelle { get; set; }
    }
}