namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Ci pointage summary
    /// </summary>
    public class CiPointageSummaryEnt : RapportHebdoSummaryBase
    {
        /// <summary>
        /// Libelle du Ci
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Ci type
        /// </summary>
        public string TypeCI { get; set; }
    }
}
