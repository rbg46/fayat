namespace Fred.Business.ObjectifFlash.Reporting.Models
{
    /// <summary>
    /// Modele d'export d'une ligne de synthese de bilan flash
    /// </summary>
    public class BilanFlashSyntheseExportModel
    {
        /// <summary>
        /// Axe
        /// </summary>
        public string AxeGroup { get; set; }

        /// <summary>
        /// Libellé du regroupement
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Montant 
        /// </summary>
        public decimal Montant { get; set; }
    }
}
