using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;

namespace Fred.Business.Depense
{
    public interface IRemplacementTachesProviderService
    {
        Task FillRemplacementTachesOnEntitiesAsync(List<DepenseAchatEnt> depenseAchats,
                                                    List<OperationDiverseEnt> operationDiverses,
                                                    List<ValorisationEnt> valorisations);
    }
}