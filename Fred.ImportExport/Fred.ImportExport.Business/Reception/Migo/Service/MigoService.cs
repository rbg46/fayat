using System.Collections.Generic;
using Fred.Business.Reception.FredIe;
using Fred.Entities.Depense;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Business.Reception.Migo.Service
{
    public class MigoService : IMigoService
    {
        private readonly IReceptionSapProviderService receptionSapProviderService;

        public MigoService(IReceptionSapProviderService receptionSapProviderService)
        {
            this.receptionSapProviderService = receptionSapProviderService;
        }


        public virtual IEnumerable<DepenseAchatEnt> GetReceptionsFilteredForSap(List<int> receptionIds)
        {
            return receptionSapProviderService.GetReceptionsFilteredForSap(receptionIds);
        }

        public virtual void UpdateQuantityAndDiminutionField(ReceptionSapModel receptionSapModel)
        {
            // De base, je ne fait rien de particulier.
            // il y a une surcharge un le groupe
        }
    }
}
