using System.IO;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les opérations diverses du fichier excel
    /// </summary>
    public interface IOdExtractorService : IService
    {
        /// <summary>
        /// Récupération des opérations diverses
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des OperationsDiverses</returns>
        ParseODsResult ParseExcelFile(Stream excelStream);
    }
}
