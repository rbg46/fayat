using System;
using System.Drawing;
using Fred.Framework.Reporting.SyncFusion.Excel;
using Syncfusion.XlsIO;


namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente le style Excel d'un item d'un groupe.
    /// </summary>
    public class BudgetComparaisonExcelGroupItemStyle : ExcelStyle
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="baseStyle">Le style de base de l'item.</param>
        /// <param name="apply">La fonction qui applique le style.</param>
        public BudgetComparaisonExcelGroupItemStyle(ExcelStyle baseStyle, Action<IRange> apply)
            : base(baseStyle, apply)
        { }

        /// <summary>
        /// Applique le style.
        /// </summary>
        /// <param name="columns">Les colonnes où appliquer le style.</param>
        /// <param name="row">L'index de la ligne où appliquer le style.</param>
        /// <param name="useEcartStyle">Indique si le style "écart" doit être appliqué.</param>
        /// <param name="value">La valeur à mettre dans les colonnes.</param>
        public void Apply(IExcelColumns columns, int row, bool useEcartStyle, string value)
        {
            Apply(columns, row, value);
            if (useEcartStyle)
            {
                var range = columns.GetRange(row);
                range.CellStyle.Color = Color.FromArgb(249, 219, 231);
                range.CellStyle.Font.RGBColor = Color.Black;
            }
        }
    }
}
