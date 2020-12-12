using System;

namespace Fred.Entities.ObjectifFlash
{
    /// <summary>
    ///   Représente la journalisation d'une ressource de tache d'Objectif Flash.
    /// </summary>
    public class ObjectifFlashTacheRessourceJournalisationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ressource de tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheRessourceJournalisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la ressource d'Objectif Flash à laquelle cette journalisation appartient
        /// </summary>
        public int ObjectifFlashTacheRessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource d'Objectif Flash à laquelle cette journalisation appartient
        /// </summary>
        public virtual ObjectifFlashTacheRessourceEnt ObjectifFlashTacheRessource { get; set; }

        /// <summary>
        /// Obtient ou définit la date de chantier
        /// </summary>
        public DateTime DateJournalisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité objectif journalière de la ressource
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }
    }
}
