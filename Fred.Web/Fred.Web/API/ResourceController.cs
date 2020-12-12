using Fred.Business.Utilisateur;
using Fred.Web.Shared.App_LocalResources;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Web.Http;

namespace Fred.Web.API
{
  [Obsolete("Utiliser plutôt RenderResources que Scripts.Render(api/resources/<FeatureName>)")]
  public class ResourceController : ApiControllerBase
  {
    private readonly string resourceBuilderRoot = new StringBuilder()
          .AppendLine("if (!angular.isDefined(resources)) {")
          .AppendLine("   var resources = { };")
          .AppendLine("}")
          .AppendLine("angular.extend(resources, {").ToString();

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ResourceController" />.
    /// </summary>
    public ResourceController()
    {
    }

    /// <summary>
    /// GET api/resource/{resourceName}
    /// Récupère la ressource .resx et la convertit en javascript
    /// </summary>
    /// <param name="resourceName">Nom de la ressource</param>
    /// <param name="societyCode">Code de la société de l'utilisateur connecté</param>
    /// <returns>Retourne les ressources à afficher</returns>
    [HttpGet]
    [Route("api/resource/{resourceName}/{societyCode?}")]
    public HttpResponseMessage GetResource(string resourceName, string societyCode = null)
    {
      try
      {
        StringBuilder sb = new StringBuilder(resourceBuilderRoot);
        
        Tuple<ResourceSet, ResourceSet> rs = ResourceHelper.GetResourceSet(resourceName, societyCode);
        ResourceSet rsBase = rs.Item1, rsSociety = rs.Item2;
        string key, value;
        foreach (DictionaryEntry entry in rsBase)
        {
          key = (string)entry.Key;
          value = (string)(rsSociety?.GetObject(key) ?? entry.Value);

          sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
        }
        sb.Append("});");
        StringContent content = new StringContent(sb.ToString(), Encoding.UTF8, "text/javascript");

        return new HttpResponseMessage() { Content = content };
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }
  }
}