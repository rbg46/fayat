using Microsoft.Web.Http.Description;
using Microsoft.Web.Http.Routing;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Fred.ImportExport.Api.App_Start
{

  /// <summary>
  /// Configure le versionning
  /// Voir https://github.com/Microsoft/aspnet-api-versioning/wiki
  /// Voir https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md#12-versioning
  /// </summary>
  internal static class VersioningConfig
  {

    /// <summary>
    /// Objet permettant la découverte dynamique des Api et permettant de récupérer les métadatas des Api.
    /// Utilisé notamment par Swagger pour afficher la documentation des Api.
    /// </summary>
    internal static VersionedApiExplorer ApiExplorer { get; private set; }


    /// <summary>
    /// Défini les options utilisées pour le versionning et les applique à la configuration
    /// Doit être appelée dans WebApiConfig.Register
    /// </summary>
    /// <param name="configuration">Asp.Net Global configuration</param>
    internal static void ApplyVersioning(HttpConfiguration configuration)
    {
      // Le versionning utilisé dans les Api Fred.IE pour les services exposés à des systèmes externes utilise le système par "Url Path"
      // C'est à dire que le numéro de version est ajouté dans le chemin vers l'Api
      // Par exemple api/moncontroleur devient api/v2/moncontroleur
      // Ce chemin est ajouté dynamiquement en spécifiant l'attribut [ApiVersion] dans le controleur.
      // Lors de la création d'une nouvelle version, il est conseillé de marquer l'ancienne comme dépréciée en utilisant l'attribut [ApiVersion("0.9", Deprecated = true)]

      // Le versionning n'est pas obligatoire, donc un controleur peut ne pas spécifier de version
      // Dans ce cas, il a une version par défaut "V1", mais cette version n'a pas besoin d'être spécifiée dans l'URL
      // Donc, pour un client, utiliser api/V1/moncontroleursansversion = api/moncontroleursansversion
      // par contre, api/V2/moncontroleursansversion, renverra une 404


      // Modifie les routes pour ajouter V1, V2... dans le chemin
      var constraintResolver = new DefaultInlineConstraintResolver()
      {
        ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }
      };


      // Configure le versionning
      configuration.AddApiVersioning(o =>
      {
        o.ReportApiVersions = true;                   // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
        o.AssumeDefaultVersionWhenUnspecified = true; // Permet d'avoir des controlleurs sans versionning, typiquement les controlleurs utilisés uniquement par Fred.Web
      });


      // Applique le routage du versionning
      configuration.MapHttpAttributeRoutes(constraintResolver);
    }




    /// <summary>
    /// Configure la découverte des Api pour la documentation en prenant en compte le versioning
    /// </summary>
    /// <param name="configuration">Asp.Net Global configuration</param>
    internal static void ApplyApiExplorer(HttpConfiguration configuration)
    {
      // add the versioned IApiExplorer and capture the strongly-typed implementation (e.g. VersionedApiExplorer vs IApiExplorer)
      // note: the specified format code will format the version as "'v'major[.minor][-status]"
      ApiExplorer = configuration.AddVersionedApiExplorer(
          options =>
          {
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;

          });
    }
  }



}