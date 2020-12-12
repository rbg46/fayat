using System.Collections.Generic;
using Fred.Web.Models.ObjectifFlash;

namespace Fred.Web.Shared.Models.ObjectifFlash.Search
{
    public class SearchObjectifFlashListWithFilterResultModel
    {
        /// <summary>
        /// Liste des objectifs flash en fonction du paging
        /// </summary>
        public IEnumerable<ObjectifFlashLightModel> Items { get; set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; set; }
    }
}
