using System.IO;
using Fred.Business;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel
{
    /// <summary>
    /// Parse le fichier excel pour l'import des ci par excel
    /// </summary>
    public interface IExcelFournisseurExtractorService : IService
    {
        /// <summary>
        ///  Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        ParseImportFournisseursResult ParseExcelFile(Stream excelStream);
    }
}
