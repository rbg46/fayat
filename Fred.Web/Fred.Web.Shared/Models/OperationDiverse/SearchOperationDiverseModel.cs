using Fred.Web.Models;
using Fred.Web.Models.Search;
using System;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Représente une recherche d'opération diverse
    /// </summary>
    public class SearchOperationDiverseModel : ISearchValueModel
    {
        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI
        /// </summary>
        public CILightModel CI { get; set; }

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
