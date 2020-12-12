using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System.Collections.Generic;

namespace Fred.Entities.ObjectifFlash
{
    /// <summary>
    ///   Représente une ressource liée à une tache d'objectif flash.
    /// </summary>
    public class ObjectifFlashTacheRessourceEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ressource de tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheRessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public ObjectifFlashTacheEnt ObjectifFlashTache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la ressource de référence attachée à cette ressource de tache d'objectif flash
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code chapitre de la ressource
        /// </summary>
        public string ChapitreCode { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource de référence attachée à cette ressource de tache d'objectif flash
        /// </summary>
        public virtual RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité de la tache
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        ///  Obtient ou définit le prix unitaire HT de la tache
        /// </summary>
        public decimal? PuHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        /// Obtient ou définit si la ressource est une clé de calcul
        /// de la journalisation
        /// </summary>
        public bool IsRepartitionKey { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des quantité de ressources journalisées => calculé.
        /// </summary>
        public decimal? TotalQuantiteJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la ressource d'objectif flash.
        /// </summary>
        public List<string> ListErreurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des journalisations liées à une ressource d'objectif flash
        /// </summary>
        public virtual ICollection<ObjectifFlashTacheRessourceJournalisationEnt> TacheRessourceJournalisations { get; set; }

        /// <summary>
        /// Clean navigation properties
        /// </summary>
        public void CleanProperties()
        {
            this.Ressource = null;
            if (UniteId == 0)
            {
                UniteId = null;
            }
            this.Unite = null;
        }
    }
}
