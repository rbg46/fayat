using Fred.ImportExport.Api.App_Start;
using System.Web.Http;

namespace Fred.ImportExport.Api
{
  /// <summary>
  /// Define the configuration of the Fred.IE API
  /// </summary>
  public static class WebApiConfig
  {

    /// <summary>
    /// Configure les options du serveur dans HttpConfiguration
    /// </summary>
    /// <param name="configuration">Asp.Net HttpConfiguration object</param>
    public static void Register(HttpConfiguration configuration)
    {
      //// Autorisation d'accès à l'API pour les sites reférencés ci-dessous
      ////EnableCorsAttribute cors;
      ////cors = new EnableCorsAttribute("http://localhost:6870,http://fred-dev.fci.lan,http://fred-inte.fci.lan,http://fred-preprod.fci.lan", "*", "GET,POST,PUT,DELETE");
      ////configuration.EnableCors(cors);


      // Applique la configuration du versionning
      VersioningConfig.ApplyVersioning(configuration);


      // Applique la configuration des routes
      RouteConfig.ConfigureRoute(configuration);
    }
  }
}
