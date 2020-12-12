using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Fred.Business.Template.Models.CommandeValidation;
using Fred.Framework.Templating;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Fred.Web
{
    public static class BundlesRazorEngine
    {
        private static readonly string TemplatesPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["templates:folder"]);

        public static void Initialize()
        {
            var config = new TemplateServiceConfiguration();
            config.Language = Language.CSharp;
            config.TemplateManager = new ResolvePathTemplateManager(new[] { TemplatesPath });
            config.CachingProvider = new DefaultCachingProvider();

            var service = RazorEngineService.Create(config);

            Compile(service);
        }

        private static void Compile(IRazorEngineService service)
        {
            // Compiler et mettre en cache les templates
            service.Compile(TemplateNames.EmailCommandeValidation, typeof(CommandeValidationTemplateModel));
            service.Compile(TemplateNames.EmailCommandeImpression, typeof(CommandeValidationTemplateModel));

            Engine.Razor = service;
        }
    }
}
