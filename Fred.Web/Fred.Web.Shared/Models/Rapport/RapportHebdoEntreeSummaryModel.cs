namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Rapport hebdo entree summary model
    /// </summary>
    public class RapportHebdoEntreeSummaryModel
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
        /// Code de l'établissement comptable du CI
        /// </summary>
        public string CiEtablissementComptableCode { get; set; }

        /// <summary>
        /// Code de la société du CI
        /// </summary>
        public string CiSocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource associée au type du CI
        /// </summary>
        public string CiTypeRessourceKey { get; set; }

        /// <summary>
        /// Obtient et définit le flag pour déterminer si ci générique
        /// </summary>
        public bool IsAbsence { get; set; }

        /// <summary>
        /// Get or set le total des heures
        /// </summary>
        public double? TotalHeure { get; set; }

        /// <summary>
        /// Get or set le total des heures d'absence
        /// </summary>
        public double? TotalHeureAbsence { get; set; }

        /// <summary>
        /// Get or set le total des heures supplémentaires
        /// </summary>
        public double? TotalHeureSup { get; set; }

        /// <summary>
        /// Ajouté pour l'utiliser au niveau de l'interface graphique . 
        /// False par défault et ne sert qu'on stocker l'information de séléction
        /// </summary>
        public bool Selected { get; set; }
    }
}
