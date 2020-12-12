using System;
using System.Collections.Generic;
using Fred.Web.Models;
using Fred.Web.Models.Search;

namespace Fred.Web.Shared.Models.Budget.Search
{
    /// <summary>
    ///   Représente une recherche d'Avancement
    /// </summary>
    public class SearchAvancementModel : ISearchValueModel
    {

        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Obtient ou définit le CI
        /// </summary>
        public CILightModel CI { get; set; }

        /// <summary>
        ///   Obtient ou définit une période
        /// </summary>
        public DateTime? Periode { get; set; }

        /// <summary>
        /// Obtient ou définit les axes analytiques affichés (T1, T2, T3 ou T4)
        /// </summary>
        public string[] AxesAnalytiquesAffiches { get; set; }

        /// <summary>
        /// Obtient ou définit les colonnes affichées
        /// </summary>
        public string[] ColonnesAffichees { get; set; }
    }
}
