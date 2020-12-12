using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Societe;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Converter;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;
using Fred.ImportExport.Models.Personnel;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Sap
{
    /// <summary>
    /// Envoie a SAP les Personnels
    /// </summary>
    public class PersonnelSapSender : IPersonnelSapSender
    {
        private readonly ApplicationsSapManager applicationsSapManager;

        public PersonnelSapSender(ApplicationsSapManager applicationsSapManager)
        {
            this.applicationsSapManager = applicationsSapManager;
        }

        /// <summary>
        /// Mappe les personnels en sap model et avoie a sap
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        public async Task MapAndSendToSapAsync<T>(PersonnelImportExportLogger logger, ImportPersonnelContext<T> context, List<ImportPersonnelSocieteContext> societesContexts) where T : class
        {
            var mapper = new PersonnelEntToSapModelConverter();

            foreach (ImportPersonnelSocieteContext societeContext in societesContexts)
            {
                List<PersonnelStormModel> transformedPersonnelsForSap = mapper.ConvertPersonnelEntToPersonnelSapModels(context, societeContext);

                await SendAsync(logger, societeContext.Societe, transformedPersonnelsForSap);
            }
        }

        private async Task SendAsync(PersonnelImportExportLogger logger, SocieteEnt societe, List<PersonnelStormModel> personnelStormModels)
        {
            if (personnelStormModels.Count == 0 || societe == null)
            {
                return;
            }

            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societe.SocieteId);

            if (applicationSapParameter.IsFound)
            {
                var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

                foreach (PersonnelStormModel personnelStormModel in personnelStormModels)
                {
                    string url = $"{applicationSapParameter.Url}&ACTION=PA30";
                    try
                    {
                        logger.LogSapModel(personnelStormModel);
                        await restClient.PostAsync(url, personnelStormModel);
                    }
                    catch (Exception e)
                    {
                        logger.ErrorWhenSendToSap(e, url, personnelStormModel);
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
