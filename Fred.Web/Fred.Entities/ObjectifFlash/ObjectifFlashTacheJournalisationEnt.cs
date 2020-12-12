using System;

namespace Fred.Entities.ObjectifFlash
{
    /// <summary>
    ///   Représente la journalisation d'une tache d'Objectif Flash.
    /// </summary>
    public class ObjectifFlashTacheJournalisationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheJournalisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tache d'Objectif Flash à laquelle cette journalisation appartient
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache d'Objectif Flash à laquelle cette journalisation appartient
        /// </summary>
        public virtual ObjectifFlashTacheEnt ObjectifFlashTache { get; set; }

        /// <summary>
        /// Obtient ou définit la date de journalisation
        /// </summary>
        public DateTime DateJournalisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité objectif journalière de la tache
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des montants de ressources => calculé
        /// </summary>
        public decimal? TotalMontantRessource { get; set; }
    }
}
