using System.Drawing;
using Fred.Framework.Reporting.SyncFusion;
using Fred.Framework.Reporting.SyncFusion.Excel;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente les styles Excel.
    /// </summary>
    public class BudgetComparaisonExcelStyles
    {
        private static readonly BudgetComparaisonExcelAxeStyle AxeStyle;
        private static readonly BudgetComparaisonExcelGroupStyle Budget1Style;
        private static readonly BudgetComparaisonExcelGroupStyle Budget2Style;
        private static readonly BudgetComparaisonExcelGroupStyle EcartStyle;
        private static readonly ExcelBasicStyle LastNodeStyle;
        private static readonly BudgetComparaisonExcelTotalStyle TotalStyle;
        private static readonly ExcelBasicStyle SeparatorStyle;

        /// <summary>
        /// La hauteur d'une grande ligne.
        /// </summary>
        public const double BigRowHeight = 29.4;

        /// <summary>
        /// La largeur standard d'une colonne.
        /// </summary>
        public const double StandardColumnWidth = 10.78;

        /// <summary>
        /// La largeur d'une colonne séparatrice.
        /// </summary>
        public const double SeparatorColumnWidth = 1.44;

        /// <summary>
        /// Le style de base des en-têtes (ou pied de page).
        /// </summary>
        public static readonly ExcelStyle HeaderStyleBase;

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static BudgetComparaisonExcelStyles()
        {
            HeaderStyleBase = new ExcelBasicStyle(range =>
            {
                range.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                range.VerticalAlignment = ExcelVAlign.VAlignCenter;
                range.FullBorders(ExcelLineStyle.Medium);
                range.CellStyle.Font.Bold = true;
            });

            // Les axes
            AxeStyle = new BudgetComparaisonExcelAxeStyle();

            // Le groupe budget 1
            Budget1Style = new BudgetComparaisonExcelGroupStyle(
                subHeader: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(76, 134, 188);
                    range.CellStyle.Font.RGBColor = Color.White;
                    range.RowHeight = BigRowHeight;
                    range.Merge();
                }),
                header: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(76, 134, 188);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.RowHeight = BigRowHeight;
                }),
                items: new BudgetComparaisonExcelGroupItemsStyle(new ExcelBasicStyle(range =>
                {
                    range.CellStyle.Color = Color.FromArgb(204, 220, 236);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.LeftRightBorders(ExcelLineStyle.Medium);
                }))
            );

            // Le groupe budget 2
            Budget2Style = new BudgetComparaisonExcelGroupStyle(
                subHeader: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(77, 134, 189);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.Merge();
                }),
                header: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(130, 170, 209);
                    range.CellStyle.Font.RGBColor = Color.Black;
                }),
                items: new BudgetComparaisonExcelGroupItemsStyle(new ExcelBasicStyle(range =>
                {
                    range.CellStyle.Color = Color.FromArgb(219, 231, 242);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.LeftRightBorders(ExcelLineStyle.Medium);
                }))
            );

            // Le groupe écart
            EcartStyle = new BudgetComparaisonExcelGroupStyle(
                subHeader: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(255, 192, 0);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.Merge();
                }),
                header: new ExcelBasicStyle(HeaderStyleBase, range =>
                {
                    range.CellStyle.Color = Color.FromArgb(255, 211, 76);
                    range.CellStyle.Font.RGBColor = Color.Black;
                }),
                items: new BudgetComparaisonExcelGroupItemsStyle(new ExcelBasicStyle(range =>
                {
                    range.CellStyle.Color = Color.FromArgb(255, 242, 204);
                    range.CellStyle.Font.RGBColor = Color.Black;
                    range.LeftRightBorders(ExcelLineStyle.Medium);
                }))
            );

            // Le dernier noeud
            LastNodeStyle = new ExcelBasicStyle(range =>
            {
                range.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            });

            // La ligne du total
            TotalStyle = new BudgetComparaisonExcelTotalStyle();

            // Les séparateurs
            SeparatorStyle = new ExcelBasicStyle(range =>
            {
                range.ColumnWidth = SeparatorColumnWidth;
            });
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="worksheet">La page.</param>
        public BudgetComparaisonExcelStyles(IWorksheet worksheet)
        {
            // Général
            worksheet.Workbook.StandardFont = "Calibri";
            worksheet.Workbook.StandardFontSize = 11;
            worksheet.Range[BudgetComparaisonExcelPositions.FirstNodeRowIndex, 1].FreezePanes();
            worksheet.StandardWidth = StandardColumnWidth;
            worksheet.Range[1, 1].Activate();

            // Impression
            worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
            worksheet.PageSetup.FitToPagesTall = 0;
            worksheet.PageSetup.PrintTitleRows = "$1:$" + BudgetComparaisonExcelPositions.HeaderRowIndex;
            worksheet.PageSetup.RightFooter = $"{FeatureBudgetComparaison.BudgetComparaison_Print_Page} &P / &N";
        }

        /// <summary>
        /// Le style de la colonne des axes.
        /// </summary>
        public BudgetComparaisonExcelAxeStyle Axe { get { return AxeStyle; } }

        /// <summary>
        /// Le style du groupe budget 1.
        /// </summary>
        public BudgetComparaisonExcelGroupStyle Budget1 { get { return Budget1Style; } }

        /// <summary>
        /// Le style du groupe budget 2.
        /// </summary>
        public BudgetComparaisonExcelGroupStyle Budget2 { get { return Budget2Style; } }

        /// <summary>
        /// Le style du groupe écart.
        /// </summary>
        public BudgetComparaisonExcelGroupStyle Ecart { get { return EcartStyle; } }

        /// <summary>
        /// Le style du dernier noeud.
        /// </summary>
        public ExcelBasicStyle LastNode { get { return LastNodeStyle; } }

        /// <summary>
        /// Le style de la ligne du total.
        /// </summary>
        public BudgetComparaisonExcelTotalStyle Total { get { return TotalStyle; } }

        /// <summary>
        /// Le style des séparateurs.
        /// </summary>
        public ExcelBasicStyle Separator { get { return SeparatorStyle; } }
    }
}
