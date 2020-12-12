using System.IO;

namespace Fred.Business.RepriseDonnees.Commande.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Commandes du fichier excel
    /// </summary>
    public interface ICommandeExtractorService : IService
    {
        /// <summary>
        ///  Récupération des Commandes
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        ParseCommandesResult ParseExcelFile(Stream excelStream);
    }
}
