using System.Collections.Generic;
using Fred.Web.Models.Rapport;

namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// Résultat d'une recherche de rapport
    /// </summary>
    public class SearchRapportListWithFilterResultModel
    {
        /// <summary>
        /// Liste de rapports en fonction du paging
        /// </summary>
        public List<RapportLightModel> Rapports { get; internal set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; internal set; }
    }
}
