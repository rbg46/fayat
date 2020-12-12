using System.Collections.Generic;
using Fred.Web.Models.Referential.Light;

namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashTacheModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Tache pour un OF.
        /// </summary>
        public int RessourceTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Objectif Flash laquelle cette ressource appartient
        /// </summary>
        public int? ObjectifFlashId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche à laquelle cette ressource appartient
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche à laquelle cette ressource appartient
        /// </summary>
        public TacheLightModel Tache { get; set; }

        /// <summary>
        ///   Obtient le libellé de la tâche associée.
        /// </summary>
        public string Libelle
        {
            get
            {
                return Tache != null ? Tache.Code + " - " + Tache.Libelle : "";
            }
        }

        /// <summary>
        ///   Obtient le libellé de l'unité associée.
        /// </summary>
        public string LibelleUnite
        {
            get
            {
                return Unite != null ? Unite.Libelle : "";
            }
        }

        /// <summary>
        ///   Obtient ou définit la quantité
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteLightModel Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des quantité de tache journalisées => calculé.
        /// </summary>
        public decimal? TotalQuantiteJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des montant ressource => calculé.
        /// </summary>
        public decimal? TotalMontantJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit le total des montant ressource => calculé.
        /// </summary>
        public decimal? TotalMontantRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la tache.
        /// </summary>
        public List<string> ListErreurs { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des Ressources associées à la tache d'Objectif Flash.
        /// </summary>
        public List<ObjectifFlashTacheRessourceModel> Ressources { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des journalisations associées à la tache d'Objectif Flash.
        /// </summary>
        public List<ObjectifFlashTacheJournalisationModel> TacheJournalisations { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des realisations associées à la tache d'Objectif Flash.
        /// </summary>
        public List<ObjectifFlashTacheRapportRealiseModel> TacheRealisations { get; set; }

    }
}
