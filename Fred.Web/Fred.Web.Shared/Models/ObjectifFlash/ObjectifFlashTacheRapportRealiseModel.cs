using System;

namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashTacheRapportRealiseModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la tache d'objectif flash realise de rapport.
        /// </summary>
        public int ObjectifFlashTacheRapportRealiseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public ObjectifFlashTacheModel ObjectifFlashTache { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de rapport de la tache.
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité réalisée de la tâche.
        /// </summary>
        public decimal? QuantiteRealise { get; set; }

        /// <summary>
        /// Obtient ou définit la date de réalisation de la tâche d'objectif flash.
        /// </summary>
        public DateTime DateRealise { get; set; }
    }
}
