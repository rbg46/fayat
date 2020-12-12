using Fred.Business.EcritureComptable;
using Fred.Business.Facturation;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Facturation;
using Fred.Web.Shared.Models.EcritureComptable;
using System.Collections.Generic;
using System.Linq;

namespace Fred.ImportExport.Business.Facturation.Validators.GFTP
{
    public class FluxFB60ImporterValidatorFtp : BaseFluxImporterValidator, IFluxFB60ImporterValidator
    {
        private readonly IEcritureComptableManager ecritureComptableManager;

        public FluxFB60ImporterValidatorFtp(IFacturationManager facturationManager,
            IEcritureComptableManager ecritureComptableManager)
            : base(facturationManager)
        {
            this.ecritureComptableManager = ecritureComptableManager;
        }

        public override void Validate(FacturationSapModel modelToValidate)
        {
            base.Validate(modelToValidate);

            ValidateExistingEcritureComptable(modelToValidate.NumeroFactureSAP);
        }

        private void ValidateExistingEcritureComptable(string numeroFactureSAP)
        {
            var numeroFactures = new List<string> { numeroFactureSAP };
            IReadOnlyList<ExistingEcritureComptableNumeroFactureSAPModel> modeles = ecritureComptableManager.GetByNumerosFacturesSAP(numeroFactures);

            //RG_11419_002 : il ne faut pas qu'il y ait d'ecriture comptable dans FRED avec ce numero de facture SAP
            if (modeles != null && modeles.Count == 0)
            {
                return;
            }

            if (CheckNumeroFactureSapExistsInModels(numeroFactureSAP, modeles))
            {
                throw new FredIeBusinessException(string.Format(FredImportExportBusinessResources.Error_Already_Existing_Facturation_In_OD, numeroFactureSAP));
            }
        }

        private bool CheckNumeroFactureSapExistsInModels(string numeroFactureSAP, IReadOnlyList<ExistingEcritureComptableNumeroFactureSAPModel> modeles)
        {
            return modeles.Any(x => x.NumeroFactureSAP == numeroFactureSAP && x.IsExist);
        }
    }
}
