using Fred.Entities.CI;
using Fred.Entities.Search;
using System;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    ///   Représente une recherche d'Avancement
    /// </summary>
    [Serializable]
    public class SearchAvancementEnt : AbstractSearch
    {
        /// <summary>
        /// Obtient ou défini le CI
        /// </summary>
        public CILightEnt CI { get; set; }

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
