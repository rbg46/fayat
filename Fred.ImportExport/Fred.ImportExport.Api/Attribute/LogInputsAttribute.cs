using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Fred.ImportExport.Framework.Log;
using Newtonsoft.Json;

namespace Fred.ImportExport.Api.Attribute
{
    /// <summary>
    /// Custom attribut pour logger les inputs
    /// </summary>
    public class LogInputsAttribute : ActionFilterAttribute, ILogInputsAttribute
    {
        /// <summary>
        /// ImportExportLogManager
        /// </summary>
        private readonly IImportExportLoggingService loggingService;

        /// <summary>
        /// Constructeur
        /// </summary>
        public LogInputsAttribute()
        {
            try
            {
                loggingService = new ImportExportLoggingService();
            }
            catch (Exception)
            {
                //Je ne fais rien car un log ne doit pas declencher d'erreur.
            }
        }

        /// <summary>
        /// Prefix de la ligne de log
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Methode de log lors de l'execution de l'action
        /// </summary>
        /// <param name="actionContext">le contexte</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext?.ActionArguments != null)
                {
                    loggingService.LogDebug(
                        $"{Prefix} Action {actionContext.ActionDescriptor.ActionName} execute avec JSON {JsonConvert.SerializeObject(actionContext.ActionArguments)}"
                    );
                }
            }
            catch (Exception)
            {
                //Je ne fais rien car un log ne doit pas declencher d'erreur.
            }
        }
    }
}
