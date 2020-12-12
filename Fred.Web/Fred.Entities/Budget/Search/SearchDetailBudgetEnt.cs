using System;
using System.Collections.Generic;
using Fred.Entities.Search;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    ///   Représente un chargement du détail budget
    /// </summary>
    [Serializable]
    public class SearchDetailBudgetEnt : AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit les niveaux de tâches visibles
        /// </summary>
        public List<int> NiveauxVisible { get; set; }

        /// <summary>
        ///   Obtient ou définit les colonnes visible
        /// </summary>
        public List<string> ColumnsVisible { get; set; }

        /// <summary>
        ///   Obtient ou définit si l'affichage est par défaut ou non 
        /// </summary>
        public bool IsDisplayCustomized { get; set; }

        /// <summary>
        ///   Obtient ou définit si on affiche les tâches inactives ou non 
        /// </summary>
        public bool DisabledTasksDisplayed { get; set; }
    }
}
