using System;
using System.Collections.Generic;
using Fred.Entities.Search;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    ///   Représente une recherche de contrôle budgétaire
    /// </summary>
    [Serializable]
    public class SearchControleBudgetaireEnt : AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit la colonnes à affichées
        /// </summary>
        public List<string> ColonnesAffichees { get; set; }

        /// <summary>
        ///   Obtient ou définit les filtres 
        /// </summary>
        public SearchControleBudgetaireFilterEnt Filter { get; set; }

        /// <summary>
        ///   Obtient ou définit la colonne de tri
        /// </summary>
        public string ColonneOrder { get; set; }

        /// <summary>
        ///   Obtient ou définit la direction de tri
        /// </summary>
        public bool? ColonneOrderAsc { get; set; }
    }
}
