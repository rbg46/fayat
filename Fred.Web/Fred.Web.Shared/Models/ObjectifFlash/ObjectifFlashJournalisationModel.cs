using System;

namespace Fred.Web.Models.ObjectifFlash
{
    /// <summary>
    /// Représente une journalisation d'objectif flash.
    /// </summary>
    public class ObjectifFlashJournalisationModel
    {
        /// <summary>
        /// Obtient ou définit la date de journalisation
        /// </summary>
        public DateTime DateJournalisation { get; set; }

        /// <summary>
        /// Obtient ou définit le flag weekend ou jourferie
        /// </summary>
        public bool IsWeekEndOrHoliday { get; set; }

        /// <summary>
        /// Obtient ou définit le total des montants de l'objectif flash pour la date de journalisation
        /// </summary>
        public decimal? TotalMontant { get; set; }

    }
}
