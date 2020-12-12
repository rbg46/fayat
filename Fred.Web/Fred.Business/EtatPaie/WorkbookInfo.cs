using Fred.Framework.Reporting;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// NPI : Cette classe ne sert qu'à éviter de rajouter un pragma warning disable S107 dans EtatPaieManager.
    /// Elle n'a aucun autre interet.
    /// </summary>
    public class WorkbookInfo
    {
        /// <summary>
        /// Le générateur de documents Excel.
        /// </summary>
        public ExcelFormat ExcelFormat { get; set; }

        /// <summary>
        /// Le Workbook.
        /// </summary>
        public IWorkbook Workbook { get; set; }
    }
}
