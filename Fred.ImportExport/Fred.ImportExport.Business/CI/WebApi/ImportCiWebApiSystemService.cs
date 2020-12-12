using System.Collections.Generic;
using System.Configuration;
using Fred.Entities.CI;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.WebApi.Context;
using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Business.CI.WebApi.Fred;
using Fred.ImportExport.Business.CI.WebApi.Mapper;
using Fred.ImportExport.Business.CI.WebApi.Validator;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.CI.WebApi
{
    public class ImportCiWebApiSystemService : IImportCiWebApiSystemService
    {
        private readonly string importJobId = ConfigurationManager.AppSettings["flux:import:ci"];

        private readonly IImportCiByApiContextProvider importCiByApiContextProvider;
        private readonly IImportCiByWebApiValidator importCiByWebApiValidator;
        private readonly IFredCiImportForWebApiService fredCiImportForWebApi;
        private readonly IFluxManager fluxManager;

        public ImportCiWebApiSystemService(
            IImportCiByApiContextProvider importCiByApiContextProvider,
            IImportCiByWebApiValidator importCiByWebApiValidator,
            IFredCiImportForWebApiService fredCiImportForWebApi,
            IFluxManager fluxManager)
        {
            this.importCiByApiContextProvider = importCiByApiContextProvider;
            this.importCiByWebApiValidator = importCiByWebApiValidator;
            this.fredCiImportForWebApi = fredCiImportForWebApi;
            this.fluxManager = fluxManager;
        }

        public void ImportCisByApi(ImportCisByApiInputs importCisByApiInput)
        {
            VerifyFluxInformationForImport();

            // Récuperations de toutes les données qui sont necessaires pour faire la validation
            ImportCiByWebApiContext contextForImport = importCiByApiContextProvider.GetContext(importCisByApiInput);

            var mapper = new WebApiModelToCiEntMapper();

            foreach (ImportCiByWebApiSocieteContext societeContext in contextForImport.SocietesContexts)
            {
                List<CIEnt> anaelCisConvertedToCiEnts = mapper.ConvertWebApiModelToCiEnts(societeContext);

                societeContext.WebApiCisMappedToCiEnt = anaelCisConvertedToCiEnts;
            }
            // verification des règles(RG) de l'import.
            importCiByWebApiValidator.VerifyRulesAndThrowIfNecessary(contextForImport);

            fredCiImportForWebApi.ManageImportedCIsFromApi(contextForImport.SocietesContexts, contextForImport.OrganisationTree);
        }

        private void VerifyFluxInformationForImport()
        {
            FluxEnt flux = fluxManager.GetByCode(importJobId);

            string msg = string.Empty;

            if (flux == null)
            {
                msg = string.Format(IEBusiness.FluxInconnu, importJobId);
            }
            else if (!flux.IsActif)
            {
                msg = string.Format(IEBusiness.FluxInactif, importJobId);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                throw new FredBusinessException(msg);
            }
        }
    }
}
