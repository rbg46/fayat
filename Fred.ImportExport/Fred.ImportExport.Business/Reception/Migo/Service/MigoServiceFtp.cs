using System.Collections.Generic;
using Fred.Business.Reception.FredIe;
using Fred.Entities.Depense;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Business.Reception.Migo.Service
{

    public class MigoServiceFtp : MigoService
    {
        private readonly IReceptionSapProviderService receptionSapProviderService;

        public MigoServiceFtp(IReceptionSapProviderService receptionSapProviderService)
            : base(receptionSapProviderService)
        {
            this.receptionSapProviderService = receptionSapProviderService;
        }

        public override IEnumerable<DepenseAchatEnt> GetReceptionsFilteredForSap(List<int> receptionIds)
        {
            return receptionSapProviderService.GetReceptionsPositivesAndNegativesFilteredForSap(receptionIds);
        }

        public override void UpdateQuantityAndDiminutionField(ReceptionSapModel receptionSapModel)
        {
            if (receptionSapModel.Quantite < 0)
            {
                receptionSapModel.Quantite = -receptionSapModel.Quantite;
                receptionSapModel.Diminution = true;
                return;
            }

            if (receptionSapModel.Quantite > 0)
            {
                receptionSapModel.Diminution = false;
                return;
            }

            if (receptionSapModel.Quantite == 0)
            {
                // RG_12552_005 : Cela n'est normalement pas possible de passer dans ce cas, car on filtre les receptions qui sont a zero avant. 
                throw new FredIeBusinessException(FredImportExportBusinessResources.Error_Reception_with_quantity_zero_must_be_not_send_to_sap);
            }

        }
    }
}
