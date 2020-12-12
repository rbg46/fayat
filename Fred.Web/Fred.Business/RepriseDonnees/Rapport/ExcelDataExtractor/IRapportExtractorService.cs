using System.IO;

namespace Fred.Business.RepriseDonnees.Rapport.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les rapports du fichier excel
    /// </summary>
    public interface IRapportExtractorService : IService
    {
        /// <summary>
        ///   Récupération des rapports
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParseRapportsResult ParseExcelFile(Stream excelStream);
    }
}
