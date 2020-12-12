using Fred.Web.Shared.Models.Rapport;

namespace Fred.Web.Shared.Models.CI
{
    /// <summary>
    /// Ci summary pointage model
    /// </summary>
    public class CiSummaryPointageModel : RapportHebdoEntreeSummaryModel
    {
        /// <summary>
        /// Libelle du Ci
        /// </summary>
        public string Libelle { get; set; }
    }
}
