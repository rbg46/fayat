using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;

namespace Fred.ImportExport.Api.App_Start
{
    /// <summary>
    /// Défini les options de configuration de Swagger
    /// Swagger permet d'exposer la documentation des APIs de Fred.IE
    /// Code Source : https://github.com/domaindrivendev/Swashbuckle
    /// Site : https://swagger.io/
    /// Swagger est accessible à l'url suivante : http://[host]:[Port]/Swagger
    /// </summary>
    internal static class SwaggerConfig
    {

        /// <summary>
        /// Demarre Swagger
        /// </summary>
        /// <param name="configuration">Asp.Net Global Configuration</param>
        internal static void Configure(HttpConfiguration configuration)
        {
            configuration.EnableSwagger("doc/{apiVersion}", SwaggerOptions())
                         .EnableSwaggerUi(SwaggerUiOptions());
        }

        /// <summary>
        /// Défini les options de swagger
        /// </summary>
        /// <returns>SwaggerDocsConfig</returns>
        private static System.Action<SwaggerDocsConfig> SwaggerOptions()
        {
            return swagger =>
            {
                // build a swagger document and endpoint for each discovered API version
                ConfigureMultipleVersion(swagger);

                // add a custom operation filter which sets default values
                swagger.OperationFilter<SwaggerDefaultValues>();


                ////swagger.SingleApiVersion("V1", "V1");

                // integrate xml comments
                swagger.IncludeXmlComments(XmlCommentsFilePath());
            };
        }

        /// <summary>
        /// Défini les options de l'interface graphique de Swagger
        /// </summary>
        /// <returns>SwaggerUiConfig</returns>
        private static System.Action<SwaggerUiConfig> SwaggerUiOptions()
        {
            return swagger =>
            {
                swagger.EnableDiscoveryUrlSelector();
                swagger.DocumentTitle("Fred API Documentation");
            };
        }

        /// <summary>
        /// Configure Swagger pour gérer l'affichage des différentes versions de l'API
        /// En utilisant l'object ApiExplorer, recherche les attributs des différents controlleurs,
        /// Indique s'ils sont dépréciés
        /// Récupère la version de l'API si elle est versionnée.
        /// Crédit : https://github.com/Microsoft/aspnet-api-versioning/blob/master/samples/webapi/SwaggerWebApiSample/Startup.cs
        /// </summary>
        /// <param name="swagger">object de configufation de Swagger</param>
        private static void ConfigureMultipleVersion(SwaggerDocsConfig swagger)
        {
            swagger.MultipleApiVersions(
                (apiDescription, version) => apiDescription.GetGroupName() == version,
                info =>
                {
                    foreach (var group in VersioningConfig.ApiExplorer.ApiDescriptions)
                    {
                        var description = "Fred Import Export RestFull API";

                        if (group.IsDeprecated)
                        {
                            description += " Cette version des API est dépréciée.";
                        }

                        info.Version(group.Name, $"Fred I.E. API {group.ApiVersion}")
                            .Contact(c => c.Name("Fayat IT").Email("fayatit@fayat.com"))
                            .Description(description)
                            .License(l => l.Name("Fayat").Url("https://Fayat.com"))
                            .TermsOfService("Corporate");
                    }
                });
        }

        /// <summary>
        /// Recherche le fichier de documentation xml généré à partir du code
        /// Ce fichier est défini dans les propriétés du projet dans l'onglet Build / Sortie / Fichier de documentation xml
        /// Il contient les attributs summary, param etc défini sur les controlleurs pour l'afficher dans Swagger, d'où l'importance de la doc...
        /// </summary>
        /// <returns>Une chaine contenant le chemin vers le fichier de documentation Xml</returns>
        internal static string XmlCommentsFilePath()
        {
            var basePath = System.AppDomain.CurrentDomain.RelativeSearchPath;
            var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }

        /// <summary>
        /// Redirige [localhost]:[port] vers swagger ui
        /// </summary>
        /// <param name="configuration">Global Configuration</param>
        internal static void RouteHomePageToSwagger(HttpConfiguration configuration)
        {
            // redirige la page d'accueil vers la documentation Swagger
            configuration.Routes.MapHttpRoute(
             name: "swagger_root",
             routeTemplate: string.Empty,
             defaults: null,
             constraints: null,
             handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));
        }
    }
}
