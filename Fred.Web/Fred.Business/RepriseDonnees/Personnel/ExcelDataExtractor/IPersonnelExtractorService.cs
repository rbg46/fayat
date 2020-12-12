using System.IO;

namespace Fred.Business.RepriseDonnees.Personnel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Personnels du fichier excel
    /// </summary>
    public interface IPersonnelExtractorService : IService
    {
        /// <summary>
        ///  Récupération des Personnels
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParsePersonnelResult ParseExcelFile(Stream excelStream);
    }
}
