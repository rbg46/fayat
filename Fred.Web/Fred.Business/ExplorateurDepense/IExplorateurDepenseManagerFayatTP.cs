using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Interface du Gestionnaire de l'explorateur des dépenses Fayat TP
    /// </summary>
    public interface IExplorateurDepenseManagerFayatTP
    {
        /// <summary>
        /// Retourne la liste des dépenses
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepensesAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Récupération des dépenses selon deux axes
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses + autres données</returns>
        Task<ExplorateurDepenseResult> GetDepensesAsync(SearchExplorateurDepense filtre, int? page, int? pageSize);

        /// <summary>
        /// Récupération du tableau de byte du fichier excel
        /// </summary>
        /// <param name="filtre">Filtre explorateur dépense</param>
        /// <returns>Tableau byte excel</returns>
        Task<byte[]> GetExplorateurDepensesExcelAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses pour un export avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseForExportWithTacheAndRessourceAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseWithTacheAndRessourceAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseForExportAsync(SearchExplorateurDepense filtre);
    }
}
