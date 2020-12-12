using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Services;
using Fred.ImportExport.Models.Log;

namespace Fred.ImportExport.DataAccess.ExternalService
{
    public class LoggingAdministratorExternalService : ILoggingAdministratorExternalService
    {
        private readonly IImportExportServiceDescriptor importExportServiceDescriptor;
        private ExternalServiceEndpoint metadata;

        private ExternalServiceEndpoint EndpointMetadata => metadata ?? (metadata = importExportServiceDescriptor.GetRestEndpoint());
        protected string BaseUrl => EndpointMetadata.BaseUrl;
        protected RestClient RestClient => EndpointMetadata.RestClient;

        public LoggingAdministratorExternalService(IImportExportServiceDescriptor importExportServiceDescriptor)
        {
            this.importExportServiceDescriptor = importExportServiceDescriptor;
        }

        /// <summary>
        /// Change le niveau de log de l'API
        /// </summary>
        /// <param name="logConfigurationModel">nouvelle configuration de log</param>
        public async Task UpdateApiLogLevel(LogConfigurationModel logConfigurationModel)
        {
            try
            {
                string requestUri = string.Format(ExternalEndPoints.Admin_Change_Log_Level, BaseUrl);
                await RestClient.PostAndEnsureSuccessAsync(requestUri, logConfigurationModel);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        /// <summary>
        /// Methode pour recuperer le niveau de log de l'API
        /// </summary>
        /// <param name="regexRules">le critere de recherche de configuration de log</param>
        /// <returns>Le niveau de log</returns>
        public async Task<string> GetApiLogLevelAsync(string regexRules)
        {
            try
            {
                string requestUri = string.Format(ExternalEndPoints.Admin_Get_Log_Level, BaseUrl, regexRules);
                return await RestClient.GetAsync<string>(requestUri);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
