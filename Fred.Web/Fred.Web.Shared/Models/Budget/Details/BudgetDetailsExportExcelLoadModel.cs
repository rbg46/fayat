using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.Details
{
    /// <summary>
    /// Décrit le model a utiliser pour chager un excel du détail des budgets
    /// </summary>
    public class BudgetDetailsExportExcelLoadModel
    {
        /// <summary>
        /// Id du budget dont on extrait les données
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Nom du template à éditer
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Flag de conversion en pdf
        /// </summary>
        public bool IsPdfConverted { get; set; }

        /// <summary>
        /// Flag de l'affihage des tâches inactives
        /// </summary>
        public bool DisabledTasksDisplayed { get; set; }

        /// <summary>
        /// Liste de niveaux qu'on souhaite affichés
        /// </summary>
        public List<int> NiveauxVisibles { get; set; }
    }
}
