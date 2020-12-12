using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui renvoie les receptions en fonction d'un filtre
    /// </summary>
    public interface IReceptionsProviderWithFilterService : IService
    {
        /// <summary>
        /// Determine les receptions en fonction du filtre
        /// La dificulté ici est de determiner les receptions impacter par le filtre.
        /// On est obligé de recupere toutes les receptions pour faire le filtre 'Far'
        /// C'est un champ calculé.
        /// </summary>
        /// <param name="filter">filter</param>     
        /// <returns>La liste des receptions</returns>      
        List<int> GetReceptionsIdsWithFilter(SearchDepenseEnt filter);

        List<DepenseAchatEnt> GetReceptionsWithFilter(SearchDepenseEnt filter);
    }
}
