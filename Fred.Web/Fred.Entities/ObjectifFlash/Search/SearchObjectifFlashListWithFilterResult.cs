using System.Collections.Generic;

namespace Fred.Entities.ObjectifFlash.Search
{
    /// <summary>
    /// Résultat d'une recherche d'objectif flash
    /// </summary>
    public class SearchObjectifFlashListWithFilterResult
    {
        /// <summary>
        /// Liste des objectifs flash en fonction du paging
        /// </summary>
        public IEnumerable<ObjectifFlashEnt> Items { get; set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; set; }
    }
}
