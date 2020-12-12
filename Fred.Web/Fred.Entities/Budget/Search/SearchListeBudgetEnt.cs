using System;
using System.Collections.Generic;
using Fred.Entities.Search;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    ///   Représente une recherche de liste de budget
    /// </summary>
    [Serializable]
    public class SearchListeBudgetEnt : AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un ci 
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit la période
        /// </summary>
        public DateTime? Periode { get; set; }

        /// <summary>
        ///   Obtient ou définit les etat de budget
        /// </summary>
        public List<string> CurrentBudgetEtatFilter { get; set; }

        /// <summary>
        ///   Obtient ou définit l'affichage des budget supprimé
        /// </summary>
        public bool DisplayBudgetDeleted { get; set; }
    }
}
