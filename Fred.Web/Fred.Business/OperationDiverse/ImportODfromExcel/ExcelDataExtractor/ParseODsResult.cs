using System.Collections.Generic;
using Fred.Entities.OperationDiverse.Excel;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des Operations Diverses a partir du fichier excel
    /// </summary>
    public class ParseODsResult
    {
        /// <summary>
        /// Liste des Operations Diverses du fichier excel
        /// </summary>
        public List<ExcelOdModel> OperationsDiverses { get; set; } = new List<ExcelOdModel>();

        /// <summary>
        /// Messages d'erreur
        /// </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
