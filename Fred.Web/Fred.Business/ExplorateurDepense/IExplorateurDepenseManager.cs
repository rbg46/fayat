using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Interface explorateur de dépenses manager
    /// </summary>
    public interface IExplorateurDepenseManager : IManager
    {
        /// <summary>
        /// Récupération de la liste d'explorateur de dépenses
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer l'arbre d'exploration :
        ///  Ordre des axe analytiques [0 = Axe Tâches puis Ressources, 1 = Axe Ressources puis Tâches]
        ///  Axe 1 d'exploration sous la forme d'une liste de string ["T1","T2","T3"]
        ///  Axe 2 d'exploration sous la forme d'une liste de string ["Chapitre","SousChapitre","Ressource"]</param>
        /// <returns>Liste des axes d'explorations</returns>
        Task<IEnumerable<ExplorateurAxe>> GetAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Récupération des dépenses selon deux axes
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses + autres données</returns>
        Task<ExplorateurDepenseResult> GetDepensesAsync(SearchExplorateurDepense filtre, int? page, int? pageSize);

        /// <summary>
        /// Récupération des dépenses selon deux axes pour un export
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <returns>Liste des dépenses + autres données</returns>
        Task<ExplorateurDepenseResult> GetDepensesForExportAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepensesAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Récupération d'un nouveau filtre ExplorateurDepense
        /// </summary>
        /// <returns>Filtre</returns>
        SearchExplorateurDepense GetNewFilter();

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
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseForExportWithTacheAndRessourceAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseWithTacheAndRessourceAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseForExportAsync(SearchExplorateurDepense filtre);
        Task<Dictionary<SearchExplorateurDepense, List<ExplorateurAxe>>> GetAsync(int ciId, List<SearchExplorateurDepense> filters, List<Func<ExplorateurDepenseGeneriqueModel, bool>> additionalFilterFuncs);
        Task<IEnumerable<ExplorateurAxe>> GetAsync(SearchExplorateurDepense filtre, Func<ExplorateurDepenseGeneriqueModel, bool> additionalFilterFunc);
    }
}
