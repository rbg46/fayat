using RazorEngine;
using RazorEngine.Templating;

namespace Fred.Framework.Templating
{
    /// <summary>
    /// Service de templating
    /// </summary>
    public class TemplatingService
    {
        /// <summary>
        /// Générer un page html à partir d'un modèle et d'un template
        /// </summary>
        /// <typeparam name="T">Type du modèle</typeparam>
        /// <param name="templateName">Nom du template</param>
        /// <param name="model">Le modèle utilisé</param>
        /// <returns>La page html générée</returns>
        public string GenererHtmlFromTemplate<T>(string templateName, T model)
        {
            // Générer le html correspondant au model
            var htmlContent = Engine.Razor.Run(templateName, typeof(T), model);

            return htmlContent;
        }
    }
}
