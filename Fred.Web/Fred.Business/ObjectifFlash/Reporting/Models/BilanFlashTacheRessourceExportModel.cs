namespace Fred.Business.ObjectifFlash.Reporting.Models
{
    /// <summary>
    /// Model d'export d'une ressource de tache de bilan flash
    /// </summary>
    public class BilanFlashTacheRessourceExportModel
    {
        /// <summary>
        /// Identifiant de la tache
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Resource Id
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Resource name
        /// </summary>
        public string RessourceLibelle { get; set; }

        /// <summary>
        /// Determines if the resource is the repartition one
        /// </summary>
        public bool IsRepartitionKey { get; set; }

        /// <summary>
        /// PUHT de la resource
        /// </summary>
        public decimal? PuHT { get; set; }

        /// <summary>
        /// Unité de la ressource
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// Quantite objectif
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }

        /// <summary>
        /// Quantite Réalisée
        /// </summary>
        public decimal? QuantiteRealise { get; set; }

        /// <summary>
        /// Montant Realisé
        /// </summary>
        public decimal MontantRealise { get; internal set; }

        /// <summary>
        /// Indique si la ressource est dans la liste des ressources de la tache d'objectif flash
        /// </summary>
        public bool HorsPerimetre { get; set; }
    }
}
