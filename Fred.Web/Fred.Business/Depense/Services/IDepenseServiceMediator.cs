using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense.Services
{
    /// <summary>
    /// Interface ServiceMediator Depense
    /// </summary>
    public interface IDepenseServiceMediator : IService
    {

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filter"><see cref="SearchDepense"/>Filtre</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        Task<IEnumerable<DepenseExhibition>> GetAllDepenseExterneForExportAsync(SearchDepense filter);

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filter"><see cref="SearchDepense"/>Filtre</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        Task<IEnumerable<DepenseExhibition>> GetAllDepenseExternetWithTacheAndRessourceAsync(SearchDepense filter);



        /// <summary>
        /// Vérifie si une CI est attaché une société SEP ou non
        /// </summary>
        /// <param name="ciId">id du Ci</param>
        /// <returns>Renvoie true si le CI est attaché à une société SEP sinon faux</returns>
        bool IsSep(int ciId);

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">Liste d'identifiant de CI</param>
        /// <param name="tacheIdList">Liste d'identifiant de tâche</param>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        Task<IReadOnlyList<DepenseExhibition>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut, DateTime? dateFin, int? deviseId);
    }
}
