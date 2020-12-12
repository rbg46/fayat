using System.Collections.Generic;
using Fred.Entities.Depense;


namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui permet gere les api pour la page 'tableau des reception'
    /// </summary>
    public interface IReceptionUpdateOrchestrator : IService
    {
        /// <summary>
        /// Met a jour une liste de receptions
        /// </summary>
        /// <param name="frontReceptions">les reception front </param>        
        void UpdateReceptions(List<DepenseAchatEnt> frontReceptions);

        /// <summary>
        /// Met a jour une reception
        /// </summary>
        /// <param name="frontReception">la reception front </param>
        /// <returns>la reception back</returns>
        DepenseAchatEnt UpdateReception(DepenseAchatEnt frontReception);
    }
}
