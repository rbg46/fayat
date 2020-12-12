using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport
{
    /// <summary>
    /// Résultat d'une recherche de rapport
    /// </summary>
    public class SearchRapportListWithFilterResult
    {
        /// <summary>
        /// Liste de rapports en fonction du paging
        /// </summary>
        public List<RapportEnt> Rapports { get; set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; set; }
    }
}
