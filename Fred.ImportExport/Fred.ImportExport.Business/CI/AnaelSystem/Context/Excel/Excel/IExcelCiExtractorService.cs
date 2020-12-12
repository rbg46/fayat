using System.IO;
using Fred.Business;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Excel
{
    /// <summary>
    /// Parse le fichier excel pour l'import des ci par excel
    /// </summary>
    public interface IExcelCiExtractorService : IService
    {
        /// <summary>
        ///  Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        ParseImportCisResult ParseExcelFile(Stream excelStream);
    }
}
