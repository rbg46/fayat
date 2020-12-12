using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Organisation
{
    /// <summary>
    ///   Représente une association d'une organisation et d'un groupe
    /// </summary>
    public class TypeOrganisationEnt
    {
        /// <summary>
        ///   Obtient le type d'organisation : Holding
        /// </summary>
        public const string CodeHolding = "HOLDING";

        /// <summary>
        ///   Obtient le type d'organisation : Pôle
        /// </summary>
        public const string CodePole = "POLE";

        /// <summary>
        ///   Obtient le type d'organisation : Groupe
        /// </summary>
        public const string CodeGroupe = "GROUPE";

        /// <summary>
        ///   Obtient le type d'organisation : société
        /// </summary>
        public const string CodeSociete = "SOCIETE";

        /// <summary>
        ///   Obtient le type d'organisation : PUO
        /// </summary>
        public const string CodePuo = "PUO";

        /// <summary>
        ///   Obtient le type d'organisation : UO
        /// </summary>
        public const string CodeUo = "UO";

        /// <summary>
        ///   Obtient le type d'organisation : PUO
        /// </summary>
        public const string CodeEtablissement = "ETABLISSEMENT";

        /// <summary>
        ///   Obtient le type d'organisation : CI
        /// </summary>
        public const string CodeCi = "CI";

        /// <summary>
        ///   Obtient le type d'organisation : UO
        /// </summary>
        public const int TypeOrganisationUo = 6;

        /// <summary>
        ///   Obtient le type d'organisation : Etablissement
        /// </summary>
        public const int TypeOrganisationEtablissement = 7;

        /// <summary>
        ///   Obtient le type d'organisation : Societe
        /// </summary>
        public const int TypeOrganisationSociete = 4;

        /// <summary>
        ///   Obtient le type d'organisation : CI
        /// </summary>
        public const int TypeOrganisationCi = 8;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une OrgaGroupe.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un Groupe.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        // MIGRATION CODE FIRST 

        /// <summary>
        /// Child Organisations where [FRED_ORGANISATION].[TypeOrganisationId] point to this entity (FK_FRED_ORGANISATION_FRED_ORGA_GROUPE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<OrganisationEnt> Organisations { get; set; } // FRED_ORGANISATION.FK_FRED_ORGANISATION_FRED_ORGA_GROUPE
    }
}
