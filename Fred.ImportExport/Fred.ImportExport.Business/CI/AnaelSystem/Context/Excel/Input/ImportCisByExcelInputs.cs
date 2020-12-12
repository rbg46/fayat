using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input
{
    /// <summary>
    /// Parametre d'entrée pour un import ci par excel
    /// </summary>
    [DebuggerDisplay("GroupeId = {GroupeId} RepriseImportCis = {RepriseImportCis?.Count} ")]
    public class ImportCisByExcelInputs
    {
        /// <summary>
        /// le GroupeId
        /// </summary>
        public int GroupeId { get; set; }
        /// <summary>
        /// Le fichier excel en stream
        /// </summary>
        public Stream ExcelStream { get; set; }
        /// <summary>
        /// Les Cis du fichier Excel
        /// </summary>
        public List<RepriseExcelCi> RepriseImportCis { get; internal set; }
    }
}
