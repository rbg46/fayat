using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    /// Interface dépense Service
    /// </summary>
    public interface IDepenseAchatService : IService
    {
        /// <summary>
        /// Récupération et affectation de la nature de chaque dépense de la liste depenseAchats
        /// </summary>
        /// <param name="depenseAchats">Liste de dépenses achat</param>
        /// <returns>Liste de dépenses achat avec Nature Analytique</returns>
        List<DepenseAchatEnt> ComputeNature(IEnumerable<DepenseAchatEnt> depenseAchats);
    }
}
