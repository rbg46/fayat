using Fred.Entities.Depense;
using System.Collections.Generic;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Calcule la nature des Receptions
    /// </summary>
    public interface IReceptionNatureProviderService : IService
    {
        /// <summary>
        /// Initialize le champs Nature d'un ensemble de receptions
        /// </summary>
        /// <param name="depenseAchats">Les receptions</param>       
        void SetNatureOfReceptions(List<DepenseAchatEnt> depenseAchats);
    }
}
