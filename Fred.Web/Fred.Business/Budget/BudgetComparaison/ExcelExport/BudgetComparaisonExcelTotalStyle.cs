using System.Drawing;
using Fred.Framework.Reporting.SyncFusion;
using Fred.Framework.Reporting.SyncFusion.Excel;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente le style de la ligne du total.
    /// </summary>
    public class BudgetComparaisonExcelTotalStyle
    {
        private static readonly ExcelBasicStyle LibelleStyle;
        private static readonly BudgetComparaisonExcelGroupItemsStyle EcartItemsStyle;

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static BudgetComparaisonExcelTotalStyle()
        {
            LibelleStyle = new ExcelBasicStyle(BudgetComparaisonExcelStyles.HeaderStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(255, 192, 0);
                range.CellStyle.Font.RGBColor = Color.Black;
                range.RowHeight = BudgetComparaisonExcelStyles.BigRowHeight;
            });
            EcartItemsStyle = new BudgetComparaisonExcelGroupItemsStyle(new ExcelBasicStyle(range =>
            {
                range.CellStyle.Color = Color.FromArgb(255, 192, 0);
                range.CellStyle.Font.RGBColor = Color.Black;
                range.SetBorders(ExcelLineStyle.Medium, ExcelBordersIndex.EdgeLeft, ExcelBordersIndex.EdgeBottom, ExcelBordersIndex.EdgeRight);
                range.VerticalAlignment = ExcelVAlign.VAlignCenter;
                range.CellStyle.Font.Bold = true;
            }));
        }

        /// <summary>
        /// Le style du libellé du total.
        /// </summary>
        public ExcelBasicStyle Libelle { get { return LibelleStyle; } }

        /// <summary>
        /// Le style des items de la colonne des écarts.
        /// </summary>
        public BudgetComparaisonExcelGroupItemsStyle EcartItems { get { return EcartItemsStyle; } }
    }
}
