using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Societe;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Converter;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Sap
{
    public class CiSapSender : ICiSapSender
    {
        private readonly IApplicationsSapManager applicationsSapManager;

        public CiSapSender(IApplicationsSapManager applicationsSapManager)
        {
            this.applicationsSapManager = applicationsSapManager;
        }

        /// <summary>
        /// Mappe les ci en sap model et avoie a sap
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        public async Task MapAndSendToSapAsync<T>(CiImportExportLogger logger, ImportCiContext<T> context, List<ImportCiSocieteContext> societesContexts) where T : class
        {
            var mapper = new CiEntToSapModelConverter();

            foreach (ImportCiSocieteContext societeContext in societesContexts)
            {
                List<CiStormModel> transformedCisForSap = mapper.ConvertCIEntToCiSapModels(context, societeContext);

                await SendAsync(logger, societeContext.Societe, transformedCisForSap);
            }
        }


        private async Task SendAsync(CiImportExportLogger logger, SocieteEnt societe, List<CiStormModel> ciStormModels)
        {
            if (ciStormModels.Count == 0 || societe == null)
            {
                return;
            }

            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societe.SocieteId);

            if (applicationSapParameter.IsFound)
            {
                var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

                foreach (CiStormModel ciStormModel in ciStormModels)
                {
                    string url = $"{applicationSapParameter.Url}&ACTION=CJ20N";
                    try
                    {
                        logger.LogSapModel(ciStormModel);
                        await restClient.PostAsync($"{applicationSapParameter.Url}&ACTION=CJ20N", ciStormModel);
                    }
                    catch (Exception e)
                    {
                        logger.ErrorWhenSendToSap(e, url, ciStormModel);
                    }
                }
            }
            else
            {
                logger.ErrorWhenSendToSapNoConfig(societe);
            }
        }
    }
}
