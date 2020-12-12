using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.Search
{
    /// <summary>
    ///   Représente pour une recherche de liste de budget
    /// </summary>
    public class SearchListeBudgetModel
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
