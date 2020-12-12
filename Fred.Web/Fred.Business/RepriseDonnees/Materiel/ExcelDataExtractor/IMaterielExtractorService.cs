using System.IO;

namespace Fred.Business.RepriseDonnees.Materiel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Materiels du fichier excel
    /// </summary>
    public interface IMaterielExtractorService : IService
    {
        /// <summary>
        ///  Récupération des Materiels
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParseMaterielResult ParseExcelFile(Stream excelStream);
    }
}
