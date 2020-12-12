using System.Collections.Generic;
using Fred.Entities.Depense;
using Fred.GroupSpecific.Infrastructure;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Business.Reception.Migo.Service
{
    public interface IMigoService : IGroupAwareService
    {
        void UpdateQuantityAndDiminutionField(ReceptionSapModel receptionSapModel);
        IEnumerable<DepenseAchatEnt> GetReceptionsFilteredForSap(List<int> receptionIds);
    }
}
