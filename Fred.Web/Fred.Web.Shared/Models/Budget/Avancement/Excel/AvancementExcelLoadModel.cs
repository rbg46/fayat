namespace Fred.Web.Shared.Models.Budget.Avancement.Excel
{
    /// <summary>
    /// Décrit le modèle à utiliser pour demander la création d'un export excel de l'avancement
    /// </summary>
    public class AvancementExcelLoadModel
    {
        /// <summary>
        /// Id du Ci contenant un avancement pour le budget en application
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// La période au format YYYYMM contenant un avancement enregistré
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Toutes les valeurs à afficher dans l'export, la hiérarchie doit être "à plat"
        /// </summary>
        /// <example>
        /// A l'indice 0 un T1
        /// A l'indice 1 Un T2 enfant du T1
        /// </example>
        public AvancementExcelModelValeurs[] Valeurs { get; set; }

        /// <summary>
        /// Flag de conversion en pdf
        /// </summary>
        public bool IsPdfConverted { get; set; }

        public AxeAnalyseAvancementModel[] AnalyticalAxis { get; set; }
    }
}
