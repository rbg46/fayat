using System;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.ObjectifFlash.Models
{
    /// <summary>
    /// Modèle pour les dépenses d'objectif Flash
    /// </summary>
    public class ObjectifFlashDepenseModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la tâche
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit la tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la dépense
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité
        /// </summary>
        public string Unite { get; set; }
    }
}
