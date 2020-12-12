using Fred.Business.Facturation;
using Fred.Entities.Facturation;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Facturation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.ImportExport.Business.Facturation.Validators
{
    public class BaseFluxImporterValidator : IFluxValidator<FacturationSapModel>
    {
        private readonly IFacturationManager facturationManager;

        public BaseFluxImporterValidator(IFacturationManager facturationManager)
        {
            this.facturationManager = facturationManager;
        }

        public virtual void Validate(FacturationSapModel modelToValidate)
        {
            //Default validation method
            if (modelToValidate == null)
            {
                throw new ArgumentNullException(nameof(modelToValidate));
            }

            ValidateAlreadyExistingFacturation(modelToValidate.NumeroFactureSAP);
        }

        private void ValidateAlreadyExistingFacturation(string numeroFactureSAP)
        {
            List<FacturationEnt> facturations = facturationManager.GetList(numeroFactureSAP).ToList();

            // RG_3656_108
            if (facturations?.Any() == true)
            {
                throw new FredIeBusinessException(string.Format(FredImportExportBusinessResources.Error_Already_Existing_Facturation, numeroFactureSAP));
            }
        }
    }
}
