namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashTacheRessourceJournalisationModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ressource de tache d'objectif flash.
        /// </summary>
        public int ObjectifFlashTacheRessourceJournalisationId { get; set; }


        /// <summary>
        ///   Obtient ou définit la quantité objectif journalière de la ressource
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }
    }
}
