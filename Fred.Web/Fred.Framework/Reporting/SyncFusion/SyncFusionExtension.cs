using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion
{
    /// <summary>
    /// Methodes d'extension pour les objets SyncFusion.
    /// </summary>
    public static class SyncFusionExtension
    {
        /// <summary>
        /// Encadre un range.
        /// </summary>
        /// <param name="range">Le range concerné.</param>
        /// <param name="lineStyle">Le style de ligne de la bordure.</param>
        public static void FullBorders(this IRange range, ExcelLineStyle lineStyle)
        {
            range.SetBorders(lineStyle, ExcelBordersIndex.EdgeLeft, ExcelBordersIndex.EdgeTop, ExcelBordersIndex.EdgeRight, ExcelBordersIndex.EdgeBottom);
        }

        /// <summary>
        /// Met une bordure à gauche et à droite d'un range.
        /// </summary>
        /// <param name="range">Le range concerné.</param>
        /// <param name="lineStyle">Le style de ligne de la bordure.</param>
        public static void LeftRightBorders(this IRange range, ExcelLineStyle lineStyle)
        {
            range.SetBorders(lineStyle, ExcelBordersIndex.EdgeLeft, ExcelBordersIndex.EdgeRight);
        }

        /// <summary>
        /// Met les bordures indiquées.
        /// </summary>
        /// <param name="range">Le range concerné.</param>
        /// <param name="lineStyle">Le style de ligne de la bordure.</param>
        /// <param name="borders">Les bordures concernées.</param>
        public static void SetBorders(this IRange range, ExcelLineStyle lineStyle, params ExcelBordersIndex[] borders)
        {
            foreach (var border in borders)
            {
                range.CellStyle.Borders[border].LineStyle = lineStyle;
            }
        }
    }
}
