using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Fred.ImportExport.Business.Facturation;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des facturations
    /// </summary>
    [Authorize(Users = "userserviceStorm, userserviceFtp", Roles = "service")]
    public class FacturationController : ApiControllerBase
    {
        private readonly FacturationFluxManager facturationFluxManager;

        public FacturationController(FacturationFluxManager facturationFluxManager)
        {
            this.facturationFluxManager = facturationFluxManager;
        }

        /// <summary>
        /// Permet d'importer des facturations dans Fred.
        /// </summary>
        /// <param name="facturationModels">Une liste de facturation.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Facturation/ImportFacturation")]
        public HttpResponseMessage ImportFacturation(List<FacturationSapModel> facturationModels)
        {
            return Post(() => facturationFluxManager.ImportFacturation(facturationModels));
        }

        /// <summary>
        /// Permet d'importer des annulations far
        /// </summary>
        /// <param name="facturationModels">Une liste de facturation.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Facturation/ImportAnnulationFar")]
        public HttpResponseMessage ImportAnnulationFar(List<FacturationSapModel> facturationModels)
        {
            return Post(() => facturationFluxManager.ImportAnnulationFar(facturationModels));
        }

        /// <summary>
        /// Permet d'importer des avoirs
        /// </summary>
        /// <param name="facturationModels">Une liste de facturation.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Facturation/ImportAvoirSansCommande")]
        public HttpResponseMessage ImportAvoirSansCommande(List<FacturationSapModel> facturationModels)
        {
            return Post(() => facturationFluxManager.ImportAvoirSansCommande(facturationModels));
        }

        /// <summary>
        /// Permet d'importer la date de transfert des FAR pour une liste de CI.
        /// </summary>
        /// <param name="dateTransfertFarModel">Une liste de model de facturation.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Facturation/ImportDateTransfertFar")]
        public HttpResponseMessage ImportDateTransfertFar(DateTransfertFarModel dateTransfertFarModel)
        {
            return Post(() => facturationFluxManager.ImportDateTransfertFar(dateTransfertFarModel));
        }
    }
}