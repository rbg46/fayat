using Fred.Framework.Reporting.SyncFusion.Excel;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente le style Excel d'un groupe.
    /// </summary>
    public class BudgetComparaisonExcelGroupStyle
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="subHeader">Le style du sous-entête.</param>
        /// <param name="header">Le style de l'entête.</param>
        /// <param name="items">Le style des items.</param>
        public BudgetComparaisonExcelGroupStyle(ExcelBasicStyle subHeader, ExcelBasicStyle header, BudgetComparaisonExcelGroupItemsStyle items)
        {
            SubHeader = subHeader;
            Header = header;
            Items = items;
        }

        /// <summary>
        /// Le style du sous-entête.
        /// </summary>
        public ExcelBasicStyle SubHeader { get; }

        /// <summary>
        /// Le style de l'entête.
        /// </summary>
        public ExcelBasicStyle Header { get; }

        /// <summary>
        /// Le style des items.
        /// </summary>
        public BudgetComparaisonExcelGroupItemsStyle Items { get; }
    }
}
