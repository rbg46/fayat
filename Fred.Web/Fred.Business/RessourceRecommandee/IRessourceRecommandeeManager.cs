using System.Collections.Generic;
using Fred.Entities.RessourcesRecommandees;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Business.RessourceRecommandee
{
    /// <summary>
    /// IRessourceRecommandeeManager Interface
    /// </summary>
    public interface IRessourceRecommandeeManager : IManager<RessourceRecommandeeEnt>
    {
        /// <summary>
        /// Adds the or update ressources recommandee list.
        /// </summary>
        /// <param name="ressourcesRecommandeesList">The ressources recommandees list.</param>
        /// <returns>List of RessourceRecommandeeModel</returns>
        List<RessourceRecommandeeModel> AddOrUpdateRessourcesRecommandeeList(List<RessourceRecommandeeModel> ressourcesRecommandeesList);
    }
}
