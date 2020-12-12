using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion.Excel
{
    /// <summary>
    /// Interface des colonnes Excel.
    /// </summary>
    public interface IExcelColumns
    {
        /// <summary>
        /// Retourne le range en fonction de l'index de la ligne.
        /// </summary>
        /// <param name="row">Index de la ligne.</param>
        /// <returns>Le range.</returns>
        IRange GetRange(int row);
    }
}
