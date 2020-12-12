using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Budget.Search
{
    /// <summary>
    ///   Représente un chargement du détail budget
    /// </summary>
    public class SearchDetailBudgetModel
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
