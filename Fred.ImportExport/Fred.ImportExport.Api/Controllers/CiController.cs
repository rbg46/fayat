using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Fred.Entities.Organisation.Tree;
using Fred.ImportExport.Api.Attribute;
using Fred.ImportExport.Api.Attribute.Cache.SynchronizationFredWeb;
using Fred.ImportExport.Business;
using Fred.ImportExport.Models.Ci;
using Hangfire;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des imports CI.
    /// </summary>
    public class CiController : ApiControllerBase
    {
        private readonly CIFluxManager ciFluxMgr;

        public CiController(CIFluxManager ciFluxMgr)
        {
            this.ciFluxMgr= ciFluxMgr;
        }

        /// <summary>
        /// Permet d'importer une liste de CI
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="cis">La liste de CI.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [LogInputs(Prefix = "[IMPORT_CIS_API] ")]
        [HttpPost]
        [Authorize(Users = "userserviceFes, userserviceFtp ", Roles = "service")]
        [Route("api/Ci/ImportCis")]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        public HttpResponseMessage ImportCis(List<WebApiCiModel> cis)
        {
            return Post(() => ciFluxMgr.ImportCis(cis));
        }

        /// <summary>
        /// Permet d'importer un CI.
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="ci">Un CI.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [LogInputs(Prefix = "[IMPORT_CI_API] ")]
        [HttpPost]
        [Authorize(Users = "userserviceFes, userserviceFtp ", Roles = "service")]
        [Route("api/Ci/ImportCi")]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        public HttpResponseMessage ImportCi(WebApiCiModel ci)
        {
            return Post(() => ciFluxMgr.ImportCi(ci));
        }


        /// <summary>
        /// Mise a jour de ci demander par Fred web, demande anael info et envoie a SAP si c'est necessaire
        /// </summary>
        /// <param name="ciIds">Liste de ci ids</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Authorize(Users = "userserviceStorm", Roles = "service")]
        [Route("api/Ci/UpdateCis")]
        public HttpResponseMessage UpdateCis([FromBody] List<int> ciIds)
        {
            return Post(() => BackgroundJob.Enqueue(() => ciFluxMgr.UpdateCis(ciIds)));
        }
    }
}
