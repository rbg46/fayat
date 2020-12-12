using System;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.DataAccess.ExternalService.FredImportExport.Moyen
{
    public class MoyenRepositoryExterne : BaseExternalRepositoy, IMoyenRepositoryExterne
    {
        public MoyenRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Export du pointage des moyens
        /// </summary>
        /// <param name="startDate">Date de début d'export</param>
        /// <param name="endDate">Date de fin </param>
        /// <returns>Résponse de l'export</returns>
        public async Task<EnvoiPointageMoyenResultModel> ExportPointageMoyenAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Moyen_Envoi_Pointage}";
                EnvoiPointageMoyenResultModel result = await RestClient.PostAndEnsureSuccessAsync<EnvoiPointageMoyenResultModel>(requestUri, new { StartDate = startDate, EndDate = endDate });

                return result;
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
