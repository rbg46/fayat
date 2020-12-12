using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input
{
    /// <summary>
    /// Parametre d'entrée pour un import ci par excel
    /// </summary>
    [DebuggerDisplay("GroupeId = {GroupeId} RepriseImportCis = {RepriseImportCis?.Count} ")]
    public class ImportFournisseursByExcelInputs
    {
        /// <summary>
        /// le GroupeId
        /// </summary>
        public List<string> CodeSocietes { get; set; }

        public string ModeleSociete { get; set; }

        /// <summary>
        /// Le fichier excel en stream
        /// </summary>
        public Stream ExcelStream { get; set; }

        public string RegleGestion { get; set; }

        /// <summary>
        /// Les Cis du fichier Excel
        /// </summary>
        public List<RepriseImportFournisseur> RepriseImportFournisseurs { get; internal set; }
    }
}
