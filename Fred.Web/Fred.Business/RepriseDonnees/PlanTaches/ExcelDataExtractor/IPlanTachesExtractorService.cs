using System.IO;

namespace Fred.Business.RepriseDonnees.PlanTaches.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Plans de tâches du fichier excel
    /// </summary>
    public interface IPlanTachesExtractorService : IService
    {
        /// <summary>
        ///  Récupération des Plans de tâches
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParsePlanTachesResult ParseExcelFile(Stream excelStream);
    }
}
