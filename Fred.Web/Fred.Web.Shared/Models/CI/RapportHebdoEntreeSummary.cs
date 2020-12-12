using Fred.Web.Shared.Models.Personnel;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.CI
{
    /// <summary>
    /// Rapport hebdo entree summary
    /// </summary>
    public class RapportHebdoEntreeSummary
    {
        /// <summary>
        /// Ci pointage summary list
        /// </summary>
        public IEnumerable<CiSummaryPointageModel> CiPointageSummaryList { get; set; }

        /// <summary>
        /// Personnel summary list
        /// </summary>
        public IEnumerable<PersonnelSummaryPointageModel> PersonnelSummaryList { get; set; }
    }
}
