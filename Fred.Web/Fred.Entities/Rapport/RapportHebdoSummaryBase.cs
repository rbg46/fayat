namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Rapport hebdo summary
    /// </summary>
    public class RapportHebdoSummaryBase
    {
        /// <summary>
        /// Id du Ci
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Le code du Ci
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Total heures
        /// </summary>
        public double? TotalHeuresNormale { get; set; }

        /// <summary>
        /// Total heures sup
        /// </summary>
        public double? TotalHeuresMajorations { get; set; }

        /// <summary>
        /// Total heures absence
        /// </summary>
        public double? TotalHeuresAbsence { get; set; }

        /// <summary>
        /// Total des heures travaillées
        /// </summary>
        public double TotalHeures
        {
            get
            {
                return TotalHeuresNormale.Value + TotalHeuresMajorations.Value + TotalHeuresAbsence.Value;
            }
        }

        /// <summary>
        /// Total des heures sup pour les heures normales
        /// </summary>
        public double? TotalHeuresNormalesSup { get; set; }
    }
}
