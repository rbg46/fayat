using Fred.Entities.CI;
using Fred.Entities.Search;
using System;

namespace Fred.Entities.OperationDiverse
{
    /// <summary>
    /// Représente une recherche d'opération diverse
    /// </summary>
    [Serializable]
    public class SearchOperationDiverseEnt : AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit le CI
        /// </summary>
        public CILightEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit le début de la période
        /// </summary>
        public DateTime? PeriodeDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la fin de la période
        /// </summary>
        public DateTime? PeriodeFin { get; set; }

        /// <summary>
        ///   Obtient ou définit la fonction de cumulation
        /// </summary>
        public bool? IsCumul { get; set; }
    }
}
