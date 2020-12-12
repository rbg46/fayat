using System.Net.Http;
using System.Web.Http;
using Fred.Business.OrganisationFeature;

namespace Fred.Web.API
{   
    /// <summary>
    /// Controller de récuperation de l'état (activé ou désactivé) d'une feature par rapport à une organisation
    /// </summary>
    public class OrganisationRelatedFeatureController : ApiControllerBase
    {
        private readonly IOrganisationRelatedFeatureService organisationRelatedFeatureService;

        public OrganisationRelatedFeatureController(IOrganisationRelatedFeatureService organisationRelatedFeatureService)
        {
            this.organisationRelatedFeatureService = organisationRelatedFeatureService;
        }

        /// <summary>
        /// récuperation de l'état (activé ou désactivé) d'une feature par rapport à l'organisation de l'utilisateur courant
        /// </summary>
        /// <param name="featureKey">la clé de la feature</param>
        /// <param name="defaultValue">la valeur par défaut à retourner si aucune clé n'est présente</param>
        /// <returns>Activé ou désactivée</returns>
        [HttpGet]
        [Route("api/OrganisationRelatedFeature/IsEnabledForCurrentUser/{featureKey}/{defaultValue}")]
        public HttpResponseMessage IsEnabledForCurrentUser(string featureKey, bool defaultValue)
        {
            return Get(() =>
            {
                return organisationRelatedFeatureService.IsEnabledForCurrentUser(featureKey, defaultValue);
            });
        }
    }
}
