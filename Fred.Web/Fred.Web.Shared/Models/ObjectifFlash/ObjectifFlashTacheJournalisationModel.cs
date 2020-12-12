namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashTacheJournalisationModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheJournalisationId { get; set; }

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
