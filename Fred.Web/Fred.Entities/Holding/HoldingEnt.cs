using Fred.Entities.Organisation;
using Fred.Entities.Pole;
using Fred.Entities.Referential;
using System.Collections.Generic;

namespace Fred.Entities.Holding
{
    /// <summary>
    ///   Représente une holding
    /// </summary>
    public class HoldingEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une holding.
        /// </summary>
        public int HoldingId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation du holding
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet holding attaché à une organisation
        /// </summary>   
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une holding.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une holding.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        /// Child CodeAbsences where [FRED_CODE_ABSENCE].[HoldingId] point to this entity (FK_FRED_CODE_ABSENCE_HOLDING)
        /// </summary>
        public virtual ICollection<CodeAbsenceEnt> CodeAbsences { get; set; }

        /// <summary>
        /// Child Poles where [FRED_POLE].[HoldingId] point to this entity (FK_FRED_POLE_Holding)
        /// </summary>
        public virtual ICollection<PoleEnt> Poles { get; set; }
    }
}