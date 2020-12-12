using Fred.Entities.Personnel;

namespace Fred.Business.Personnel.Excel
{
    /// <summary>
    /// Manager de l'export excel des personnels
    /// </summary>
    public interface IPersonnelExcelManager : IManager<PersonnelEnt>
    {
        /// <summary>
        /// Retourune un tableau d'octets contenant les données relative à l'export excel des personnels
        /// </summary>
        /// <param name="filter">Filtre permettant de délimiter le type ou le nom du personnel que l'on souhaite récupérer</param>
        /// <returns>Un tableau d'octet, jamais null</returns>
        byte[] GetExportExcel(SearchPersonnelEnt filter, bool haveHabilitation);

        /// <summary>
        /// ajouter l'excel généré dans le cache et retourner la clé de cache
        /// </summary>
        /// <param name="utilisateurID">id d'utilsateur</param>
        /// <returns>return la clé de cache</returns>
        string AddGeneratedExcelStreamToCache(int utilisateurID, string templateFolderPath);
    }
}
