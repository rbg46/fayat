using System.IO;

namespace Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les ci du fichier excel
    /// </summary>
    public interface ICiExtractorService : IService
    {
        /// <summary>
        ///   Récupération des personnels d'AS400 
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParseCisResult ParseExcelFile(Stream excelStream);
    }
}
