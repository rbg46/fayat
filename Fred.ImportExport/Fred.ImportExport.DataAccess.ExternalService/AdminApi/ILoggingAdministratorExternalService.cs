using System.Threading.Tasks;
using Fred.ImportExport.Models.Log;

namespace Fred.ImportExport.DataAccess.ExternalService
{
    /// <summary>
    /// Service mettant a jour les logs de Fred IE API
    /// </summary>
    public interface ILoggingAdministratorExternalService : IFredIEExternalService
    {
        /// <summary>
        /// Change le niveau de log de l'API
        /// </summary>
        /// <param name="logConfigurationModel">nouvelle configuration de log</param>
        Task UpdateApiLogLevel(LogConfigurationModel logConfigurationModel);

        /// <summary>
        /// Methode pour recuperer le niveau de log de l'API
        /// </summary>
        /// <param name="regexRules">le critere de recherche de configuration de log</param>
        /// <returns>Le niveau de log</returns>
        Task<string> GetApiLogLevelAsync(string regexRules);
    }
}
