using Fred.Entities.Societe;
using System;

namespace Fred.Entities.DatesCalendrierPaie
{
    /// <summary>
    ///   Représente ou définie le paramétrage des dates butoires pour la paie pour une société et une année
    /// </summary>
    public class DatesCalendrierPaieEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique du paramétrage d'un mois.
        /// </summary>
        public int DatesCalendrierPaieId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de saisie des pointages.
        /// </summary>
        public DateTime? DateFinPointages { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de transfert des pointages pour la paie.
        /// </summary>
        public DateTime? DateTransfertPointages { get; set; }

        /// <summary>
        /// Parent Societe pointed by [FRED_DATES_CALENDRIER_PAIE].([SocieteId]) (FK_SOCIETE_ID)
        /// </summary>
        public virtual SocieteEnt Societe { get; set; }
    }
}