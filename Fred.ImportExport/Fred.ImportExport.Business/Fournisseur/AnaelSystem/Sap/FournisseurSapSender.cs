using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Entities.Societe;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Sap
{
    /// <summary>
    /// Envoie a SAP les Cis
    /// </summary>
    public class FournisseurSapSender : IFournisseurSapSender
    {
        private readonly ApplicationsSapManager applicationsSapManager;
        private readonly IMapper mapper;

        public FournisseurSapSender(ApplicationsSapManager applicationsSapManager, IMapper mapper)
        {
            this.applicationsSapManager = applicationsSapManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Mappe les ci en sap model et avoie a sap
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        public async Task MapAndSendToSapAsync<T>(FournisseurImportExportLogger logger, ImportFournisseurContext<T> context, List<ImportFournisseurSocieteContext> societesContexts) where T : class
        {
            var converter = new FournisseurEntToSapModelConverter(mapper);

            foreach (ImportFournisseurSocieteContext societeContext in societesContexts)
            {
                List<FournisseurStormModel> transformedCisForSap = converter.ConvertFournisseurEntToFournisseurSapModels(societeContext);

                await SendAsync(logger, societeContext.Societe, transformedCisForSap);
            }
        }


        private async Task SendAsync(FournisseurImportExportLogger logger, SocieteEnt societe, List<FournisseurStormModel> fournisseurStormModels)
        {
            if (fournisseurStormModels.Count == 0 || societe == null)
            {
                return;
            }

            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societe.SocieteId);

            if (applicationSapParameter.IsFound)
            {
                var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

                foreach (FournisseurStormModel item in fournisseurStormModels)
                {
                    string url = $"{applicationSapParameter.Url}&ACTION=XK01";
                    try
                    {
                        logger.LogSapModel(item);
                        await restClient.PostAndEnsureSuccessAsync(url, item);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorWhenSendToSap(ex, url, item);
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
