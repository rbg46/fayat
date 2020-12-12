using System.Net.Http;
using System.Web.Http;
using Fred.ImportExport.Framework.Log;
using Fred.ImportExport.Models.Log;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller pour administrer l'API
    /// </summary>
    public class AdminController : ApiControllerBase
    {
        private readonly IImportExportLoggingService loggingService;

        public AdminController(IImportExportLoggingService loggingService)
        {
            this.loggingService = loggingService;
        }
        /// <summary>
        /// Permet de changer le niveau de log de l'API
        /// </summary>
        /// <param name="logConfigurationModel">la nouvelle configuration de log souhaitee</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Authorize(Roles = "service")]
        [Route("api/Admin/ChangeLogLevel")]
        public HttpResponseMessage ChangeLogLevel([FromBody] LogConfigurationModel logConfigurationModel)
        {
            return Post(() =>
            {
                if (logConfigurationModel != null
                    && !string.IsNullOrEmpty(logConfigurationModel.LogLevel)
                    && !string.IsNullOrEmpty(logConfigurationModel.RegexRules))
                {
                    loggingService.SetLogLevel(logConfigurationModel.LogLevel, logConfigurationModel.RegexRules);
                }

            });
        }

        /// <summary>
        /// Renvoie le niveau de log minimum de l'API
        /// </summary>
        /// <param name="regexRules">le critere de recherche de configuration de log</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [Authorize(Roles = "service")]
        [Route("api/Admin/GetMinLogLevel/{regexRules}")]
        public HttpResponseMessage GetMinLogLevel(string regexRules)
        {
            return Get(() =>
            {
                if (!string.IsNullOrEmpty(regexRules))
                {
                    return loggingService.GetMinLogLevelFor(regexRules);
                }

                return null;
            });
        }
    }
}
