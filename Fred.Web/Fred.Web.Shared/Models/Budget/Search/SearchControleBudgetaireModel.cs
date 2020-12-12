using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.Search
{
    /// <summary>
    ///   Représente une recherche de contrôle budgétaire
    /// </summary>
    public class SearchControleBudgetaireModel
    {
        /// <summary>
        ///   Obtient ou définit la colonnes à affichées
        /// </summary>
        public List<string> ColonnesAffichees { get; set; }

        /// <summary>
        ///   Obtient ou définit les filtres 
        /// </summary>
        public SearchControleBudgetaireFilterModel Filter { get; set; }

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
