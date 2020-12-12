using System.IO;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les validation de Commandes du fichier excel
    /// </summary>
    public interface IValidationCommandeExtractorService : IService
    {
        /// <summary>
        ///  Récupération les validation de Commandes
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParseValidationCommandesResult ParseExcelFile(Stream excelStream);
    }
}
