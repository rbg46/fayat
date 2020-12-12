using System;
using System.Drawing;
using Fred.Framework.Reporting.SyncFusion;
using Fred.Framework.Reporting.SyncFusion.Excel;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente le style Excel d'un axe.
    /// </summary>
    public class BudgetComparaisonExcelAxeStyle
    {
        private const double AxeColumnWidth = 54;

        private static readonly ExcelBasicStyle HeaderStyle;
        private static readonly ExcelBasicStyle Tache1Style;
        private static readonly ExcelBasicStyle Tache2Style;
        private static readonly ExcelBasicStyle Tache3Style;
        private static readonly ExcelBasicStyle Tache4Style;
        private static readonly ExcelBasicStyle ChapitreStyle;
        private static readonly ExcelBasicStyle SousChapitreStyle;
        private static readonly ExcelBasicStyle RessourceStyle;

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static BudgetComparaisonExcelAxeStyle()
        {
            HeaderStyle = new ExcelBasicStyle(range =>
            {
                range.CellStyle.Color = Color.FromArgb(0, 82, 160);
                range.CellStyle.Font.RGBColor = Color.White;
                range.CellStyle.Font.Bold = true;
                range.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                range.VerticalAlignment = ExcelVAlign.VAlignCenter;
                range.ColumnWidth = AxeColumnWidth;
                range.FullBorders(ExcelLineStyle.Medium);
            });

            var axeStyleBase = new ExcelBasicStyle(range =>
            {
                range.LeftRightBorders(ExcelLineStyle.Medium);
            });

            Tache1Style = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(64, 109, 161);
                range.CellStyle.Font.RGBColor = Color.White;
            });

            Tache2Style = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(102, 138, 180);
                range.CellStyle.Font.RGBColor = Color.White;
            });

            Tache3Style = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(160, 182, 208);
                range.CellStyle.Font.RGBColor = Color.White;
            });

            Tache4Style = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(207, 218, 231);
                range.CellStyle.Font.RGBColor = Color.Black;
            });

            ChapitreStyle = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(255, 216, 0);
                range.CellStyle.Font.RGBColor = Color.Black;
            });

            SousChapitreStyle = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(255, 234, 113);
                range.CellStyle.Font.RGBColor = Color.Black;
            });

            RessourceStyle = new ExcelBasicStyle(axeStyleBase, range =>
            {
                range.CellStyle.Color = Color.FromArgb(255, 244, 187);
                range.CellStyle.Font.RGBColor = Color.Black;
            });
        }

        /// <summary>
        /// Le style de l'entête de l'axe.
        /// </summary>
        public ExcelBasicStyle Header { get { return HeaderStyle; } }

        /// <summary>
        /// Applique le style.
        /// </summary>
        /// <param name="columns">Les colonnes où appliquer le style.</param>
        /// <param name="row">L'index de la ligne où appliquer le style.</param>
        /// <param name="axeType">Le type de l'axe.</param>
        /// <param name="nodeIndex">L'index hiérarchique de l'axe.</param>
        /// <param name="value">La valeur à mettre dans le range.</param>
        public void Apply(IExcelColumns columns, int row, Dto.AxeType axeType, int nodeIndex, string value)
        {
            GetStyle(axeType).Apply(columns, row, value);
            columns.GetRange(row).IndentLevel = nodeIndex;
        }

        /// <summary>
        /// Retourne le style à appliquer en fonction du type d'axe.
        /// </summary>
        /// <param name="axeType">Le type d'axe.</param>
        /// <returns>Le style.</returns>
        private ExcelBasicStyle GetStyle(Dto.AxeType axeType)
        {
            switch (axeType)
            {
                case Dto.AxeType.Tache1:
                    return Tache1Style;
                case Dto.AxeType.Tache2:
                    return Tache2Style;
                case Dto.AxeType.Tache3:
                    return Tache3Style;
                case Dto.AxeType.Tache4:
                    return Tache4Style;
                case Dto.AxeType.Chapitre:
                    return ChapitreStyle;
                case Dto.AxeType.SousChapitre:
                    return SousChapitreStyle;
                case Dto.AxeType.Ressource:
                    return RessourceStyle;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
