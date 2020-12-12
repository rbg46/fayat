using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Materiel
{
    /// <summary>
    /// Repository des services pour les commandes
    /// </summary>
    public class MaterielRepositoryExterne : BaseExternalRepositoy, IMaterielRepositoryExterne
    {
        public MaterielRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Permet d'exporter les pointages des materiels vers STORM.
        /// </summary>
        /// <param name="rapportId">L'identifiant d'un rapport.</param>
        public async Task ExportPointageMaterielToStormAsync(int rapportId)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Rapport_Get_ExportPointageMaterielToStorm}/{rapportId}";
                await RestClient.GetAsync<string>(requestUri);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Erreur lors de l'export des pointages materiel de rapport vers STORM");
            }
        }

        /// <summary>
        /// Permet d'exporter les pointages des materiels vers STORM d'une liste de rapports
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapports.</param>
        public async Task ExportPointageMaterielToStormAsync(List<int> rapportIds)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Rapport_Post_ExportPointageMaterielToStorm}";
                await RestClient.PostAndEnsureSuccessAsync<string>(requestUri, rapportIds);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Erreur lors de l'export des pointages materiel de liste de rapports vers STORM");
            }
        }
    }
}
