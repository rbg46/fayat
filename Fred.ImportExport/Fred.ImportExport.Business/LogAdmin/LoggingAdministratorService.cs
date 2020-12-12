using System;
using System.Threading.Tasks;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.Framework.Log;
using Fred.ImportExport.Models.Log;

namespace Fred.ImportExport.Business
{
    /// <summary>
    /// Service d'administration des logs
    /// </summary>
    public class LoggingAdministratorService : ILoggingAdministratorService
    {
        private readonly ILoggingAdministratorExternalService loggingAdminApiService;
        private readonly IImportExportLoggingService loggingService;

        public LoggingAdministratorService(ILoggingAdministratorExternalService loggingAdminApiService, IImportExportLoggingService loggingService)
        {
            this.loggingAdminApiService = loggingAdminApiService;
            this.loggingService = loggingService;
        }

        /// <summary>
        /// Update the log level of the API
        /// </summary>
        /// <param name="logConfigurationModel">nouvelle configuration de log</param>
        public void UpdateApiLogLevel(LogConfigurationModel logConfigurationModel)
        {
            try
            {
                if (logConfigurationModel != null && !string.IsNullOrEmpty(logConfigurationModel.LogLevel))
                {
                    loggingAdminApiService.UpdateApiLogLevel(logConfigurationModel);
                }
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
            }
        }

        /// <summary>
        /// Get the log level of the API
        /// </summary>
        /// <param name="regexRules">le critere de recherche de configuration de log</param>
        /// <returns>Le niveau de log</returns>
        public async Task<string> GetApiLogLevelAsync(string regexRules)
        {
            try
            {
                if (!string.IsNullOrEmpty(regexRules))
                {
                    return await loggingAdminApiService.GetApiLogLevelAsync(regexRules);
                }
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
            }

            return null;
        }
    }
}
