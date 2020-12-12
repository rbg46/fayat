using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion.Excel
{
    /// <summary>
    /// Représente des colonnes Ecxel consécutives.
    /// </summary>
    public class ExcelColumns : IExcelColumns
    {
        /// <summary>
        /// La page.
        /// </summary>
        protected readonly IWorksheet worksheet;

        /// <summary>
        /// L'index de la première colonne.
        /// </summary>
        public readonly int Start;

        /// <summary>
        /// L'index de la dernière colonne.
        /// </summary>
        public readonly int End;

        /// <summary>
        /// Le nombre de colonne.
        /// </summary>
        public readonly int Count;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="worksheet">La page.</param>
        /// <param name="start">L'index de la première colonne.</param>
        /// <param name="end">L'index de la dernière colonne.</param>
        public ExcelColumns(IWorksheet worksheet, int start, int end)
        {
            this.worksheet = worksheet;
            Start = start;
            End = end;
            Count = end - start + 1;
        }

        /// <summary>
        /// Retourne le range en fonction de l'index de la ligne.
        /// </summary>
        /// <param name="row">Index de la ligne</param>
        /// <returns>Le range.</returns>
        public IRange GetRange(int row)
        {
            return worksheet.Range[row, Start, row, End];
        }
    }
}
