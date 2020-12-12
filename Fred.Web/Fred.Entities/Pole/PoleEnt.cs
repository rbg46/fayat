using Fred.Entities.Groupe;
using Fred.Entities.Holding;
using Fred.Entities.Organisation;

namespace Fred.Entities.Pole
{
    /// <summary>
    ///   Représente un pole
    /// </summary>
    public class PoleEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un pole.
        /// </summary>
        public int PoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation du pole
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet pole attaché à une organisation
        /// </summary>
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un pole.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un pole.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une holding.
        /// </summary>
        public int HoldingId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à une holding
        /// </summary>
        public HoldingEnt Holding { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Child Groupes where [FRED_GROUPE].[PoleId] point to this entity (FK_FRED_GROUPE_POLE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<GroupeEnt> Groupes { get; set; } // FRED_GROUPE.FK_FRED_GROUPE_POLE
    }
}