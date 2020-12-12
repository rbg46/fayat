using System.Web.Http;

namespace Fred.ImportExport.Api.App_Start
{

  /// <summary>
  /// Rassemble la configuration des routes de Fred IE API
  /// </summary>
  internal static class RouteConfig
  {

    /// <summary>
    /// Configure les routes de Fred IE API
    /// </summary>
    /// <param name="configuration">Asp.Net Global Configuration</param>
    internal static void ConfigureRoute(HttpConfiguration configuration)
    {
      // Route vers les Api
      configuration.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      // Route la page d'accueil vers l'interface Swagger
      SwaggerConfig.RouteHomePageToSwagger(configuration);
    }
  }
}