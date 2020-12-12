using Fred.Framework.Reporting.SyncFusion.Excel;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente les positions des éléments dans le fichier Excel.
    /// </summary>
    public class BudgetComparaisonExcelPositions
    {
        /// <summary>
        /// L'index de la ligne du sous-entête.
        /// </summary>
        public const int SubHeaderRowIndex = 3;

        /// <summary>
        /// L'index de la ligne de l'entête.
        /// </summary>
        public const int HeaderRowIndex = 4;

        /// <summary>
        /// L'index de la ligne du premier noeud.
        /// </summary>
        public const int FirstNodeRowIndex = 5;

        /// <summary>
        /// La colonne des axes.
        /// </summary>
        public ExcelColumn AxeColumn { get; set; }

        /// <summary>
        /// Les colonnes du budget 1.
        /// </summary>
        public BudgetComparaisonExcelGroupColumns Budget1Columns { get; set; }

        /// <summary>
        /// Les colonnes du budget 2.
        /// </summary>
        public BudgetComparaisonExcelGroupColumns Budget2Columns { get; set; }

        /// <summary>
        /// Les colonnes de l'écart.
        /// </summary>
        public BudgetComparaisonExcelGroupColumns EcartColumns { get; set; }
    }
}
