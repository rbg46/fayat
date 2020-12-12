using System;
using System.Collections.Generic;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    ///   Représente les filtres de recherche pour une recherche de contrôle budgétaire
    /// </summary>
    [Serializable]
    public class SearchControleBudgetaireFilterEnt
    {
        /// <summary>
        ///   Obtient ou définit les axes à affiches
        /// </summary>
        public List<string> AxeAffichees { get; set; }

        /// <summary>
        ///   Obtient ou définit la Axe Principal
        /// </summary>
        public string AxePrincipal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'indetifiant unique du ci
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit si le contrôle budgétaire et en cumul ou non 
        /// </summary>
        public bool Cumul { get; set; }

        /// <summary>
        ///   Obtient ou définit la période comptable
        /// </summary>
        public int PeriodeComptable { get; set; }
    }
}
