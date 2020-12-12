using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Entities.ObjectifFlash
{
    /// <summary>
    ///   Représente une tache d'Objectif Flash.
    /// </summary>
    public class ObjectifFlashTacheEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public int ObjectifFlashId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public ObjectifFlashEnt ObjectifFlash { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche à laquelle cette ressource appartient
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche à laquelle cette ressource appartient
        /// </summary>
        public virtual TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité objectif de la tache
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des quantité de tache journalisées => calculé.
        /// </summary>
        public decimal? TotalQuantiteJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des quantité de tache journalisées => calculé.
        /// </summary>
        public decimal? TotalMontantJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des montant ressource => calculé.
        /// </summary>
        public decimal? TotalMontantRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la tache d'objectif flash.
        /// </summary>
        public List<string> ListErreurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources liées à une tache d'objectif flash
        /// </summary>
        public virtual ICollection<ObjectifFlashTacheRessourceEnt> Ressources { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des journalisations liées à une tache d'objectif flash
        /// </summary>
        public virtual ICollection<ObjectifFlashTacheJournalisationEnt> TacheJournalisations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des réalisations liées à une tache d'objectif flash
        /// </summary>
        public virtual ICollection<ObjectifFlashTacheRapportRealiseEnt> TacheRealisations { get; set; }
        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void CleanProperties()
        {
            this.Tache = null;
            this.Unite = null;
            if (UniteId == 0)
            {
                UniteId = null;
            }
            foreach (var objectifFlashTacheRessource in Ressources ?? new List<ObjectifFlashTacheRessourceEnt>())
            {
                objectifFlashTacheRessource.CleanProperties();
            }
        }
    }
}
