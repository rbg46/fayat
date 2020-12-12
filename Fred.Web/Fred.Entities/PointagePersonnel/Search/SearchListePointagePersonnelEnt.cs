using Fred.Entities.Search;
using System;

namespace Fred.Entities.PointagePersonnel.Search
{
    /// <summary>
    /// Représente une recherche de pointage personnel
    /// </summary>
    [Serializable]
    public class SearchListePointagePersonnelEnt : AbstractSearch
    {
        /// <summary>
        /// Obtient ou définit la période
        /// </summary>
        public DateTime? Periode { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public int? PersonnelId { get; set; }
    }
}
