using System.Collections.Generic;
using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.ReferentielFixe.Light;

namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashTacheRessourceModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ressource de tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheRessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche à laquelle cette ressource appartient
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit la tâche à laquelle cette ressource appartient
        /// </summary>
        public ObjectifFlashTacheModel Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le code chapitre de la ressource
        /// </summary>
        public string ChapitreCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource de référence attachée à cette ressource
        /// La tâche est de niveau T4
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource de référence attachée à cette ressource
        /// </summary>
        public RessourceLightModel Ressource { get; set; }

        /// <summary>
        /// Obtient le libellé de la ressource.
        /// </summary>
        public string Libelle
        {
            get
            {
                return Ressource != null ? Ressource.Code + " - " + Ressource.Libelle : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la quantité
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire HT de la ressource
        /// </summary>
        public decimal PuHT { get; set; }

        /// <summary>
        ///  Obtient ou définit le prix unitaire HT de la ressource
        /// </summary>
        public decimal Montant
        {
            get
            {
                return PuHT * (QuantiteObjectif ?? 0);
            }
        }

        /// <summary>
        /// Obtient ou définit si la ressource est une clé de calcul
        /// de la journalisation
        /// </summary>
        public bool IsRepartitionKey { get; set; }


        /// <summary>
        /// Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité
        /// </summary>
        public UniteLightModel Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des quantité de ressources journalisées => calculé.
        /// </summary>
        public decimal? TotalQuantiteJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la ressource.
        /// </summary>
        public List<string> ListErreurs { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des journalisations liées à une ressource d'objectif flash
        /// </summary>
        public List<ObjectifFlashTacheRessourceJournalisationModel> TacheRessourceJournalisations { get; set; }

    }
}
