using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion.Excel
{
    /// <summary>
    /// Représente une colonne Excel.
    /// </summary>
    public class ExcelColumn : IExcelColumns
    {
        /// <summary>
        /// La page.
        /// </summary>
        protected readonly IWorksheet worksheet;

        /// <summary>
        /// L'index de la colonne.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="worksheet">La page.</param>
        /// <param name="index">L'index de la colonne.</param>
        public ExcelColumn(IWorksheet worksheet, int index)
        {
            this.worksheet = worksheet;
            Index = index;
        }

        /// <summary>
        /// Retourne le range en fonction de l'index de la ligne.
        /// </summary>
        /// <param name="row">Index de la ligne</param>
        /// <returns>Le range.</returns>
        public IRange GetRange(int row)
        {
            return worksheet.Range[row, Index];
        }
    }
}
