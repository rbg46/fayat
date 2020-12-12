using System.IO;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Indemnités de Déplacement du fichier excel
    /// </summary>
    public interface IIndemniteDeplacementExtractorService : IService
    {
        /// <summary>
        ///  Récupération des Indemnités de Déplacement
        /// </summary>
        /// <param name="excelStream">Stream representant le fichier excel</param>   
        /// <returns>Resultat du parsage</returns>
        ParseIndemniteDeplacementResult ParseExcelFile(Stream excelStream);
    }
}
