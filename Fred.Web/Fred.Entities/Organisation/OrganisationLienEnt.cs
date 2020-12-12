using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Entities.Organisation
{
    /// <summary>
    ///   Représente une association entre un utilisateur, un rôle et une organisation
    /// </summary>
    public class OrganisationLienEnt
    {
        // DENORMALISATION : L'ATTRIBUT '[Column] EST DIFFERENT DU NOM DE LA PROPRIETE.
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int OrganisationLiensId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un utilisateur.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit le groupe associé
        /// </summary>
        public virtual OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe associé
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe associé
        /// </summary>
        public EtablissementComptableEnt EtablissementComptable { get; set; }
    }
}