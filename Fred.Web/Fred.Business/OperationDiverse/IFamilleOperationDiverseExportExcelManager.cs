using Fred.Entities.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Interface du manager des exports Excel des familles d'OD
    /// </summary>
    public interface IFamilleOperationDiverseExportExcelManager : IManager<FamilleOperationDiverseEnt>
    {
        /// <summary>
        /// Génère le byte[] pour l'export Excel
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>le byte[] pour l'export Excel</returns>
        byte[] GetExportExcelForNature(int societeId, string typeDonnee);

        /// <summary>
        /// Génère le byte[] pour l'export Excel journal
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Le byte[] pour l'export Excel journal</returns>
        byte[] GetExportExcelForJournal(int societeId, string typeDonnee);
    }
}
