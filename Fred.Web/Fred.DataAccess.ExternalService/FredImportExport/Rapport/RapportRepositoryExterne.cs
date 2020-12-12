using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Services;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Rapport
{
    public class RapportRepositoryExterne : BaseExternalRepositoy, IRapportRepositoryExterne
    {
        private static string baseUrlTibco = ConfigurationManager.AppSettings["Tibco:WebApiUrl"];

        public RapportRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
        : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Permet d'exporter les pointages des materiels vers STORM.
        /// </summary>
        /// <param name="rapportId">L'identifiant d'un rapport.</param>
        public async Task ExportPointagePersonnelToSapAsync(int rapportId)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Rapport_Get_ExportPointagePersonnelToSap}/{rapportId}";
                await RestClient.GetAsync<string>(requestUri);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Erreur lors de l'export des pointages personnel de rapport vers SAP");
            }
        }

        /// <summary>
        /// Permet d'exporter les pointages personnel de plusieurs rapports vers STORM.
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport.</param>
        public async Task ExportPointagePersonnelToSapAsync(List<int> rapportIds)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Rapport_Post_ExportPointagePersonnelToSap}";
                await RestClient.PostAndEnsureSuccessAsync(requestUri, rapportIds);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Erreur lors de l'export des pointages personnel de liste de rapports vers SAP");
            }
        }

        /// <summary>
        /// Permet d'exporter les pointages personnel de plusieurs rapports vers TIBCO.
        /// </summary>
        /// <param name="filter">model de filtre</param>      
        public async Task ExportPointagePersonnelToTibcoAsync(ExportPointagePersonnelFilterModel filter)
        {
            try
            {
                string requestUri =
                    string.Format(WebApiEndPoints.Rapport_Post_ExportPointagePersonnelToTibco, baseUrlTibco, filter.UserId, filter.Simulation,
                    filter.DateDebut.Date, filter.TypePeriode, filter.SocieteCode, string.Join(",", filter.EtablissementsComptablesCodes));
                var restClientTibco = new RestClient(string.Empty, string.Empty); //bouchon en attendant coté tibco
                await restClientTibco.GetAsync<string>(requestUri);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Erreur lors de l'export des pointages personnel de liste de rapports vers Tibco");
            }
        }
    }
}
