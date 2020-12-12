using System.Threading.Tasks;
using Fred.ImportExport.Models.Log;

namespace Fred.ImportExport.Business
{
    /// <summary>
    /// Service d'administration des logs
    /// </summary>
    public interface ILoggingAdministratorService : IFredIEService
    {
        /// <summary>
        /// Update the log level of the API
        /// </summary>
        /// <param name="logConfigurationModel">nouvelle configuration de log</param>
        void UpdateApiLogLevel(LogConfigurationModel logConfigurationModel);

        /// <summary>
        /// Get the log level of the API
        /// </summary>
        /// <param name="regexRules">le critere de recherche de configuration de log</param>
        /// <returns>Le niveau de log</returns>
        Task<string> GetApiLogLevelAsync(string regexRules);
    }
}
