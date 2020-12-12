using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Entities.Models;
using Fred.Framework.Models;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Models.Commande;

namespace Fred.ImportExport.Api.Controllers
{

    /// <summary>
    /// Controller Web API des commandes
    /// </summary>    
    public class CommandeController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly CommandeFluxManager commandeFluxManager;

        public CommandeController(IMapper mapper, CommandeFluxManager commandeFluxManager)
        {
            this.mapper = mapper;
            this.commandeFluxManager = commandeFluxManager;
        }

        /// <summary>
        /// Permet d'exporter une commande vers SAP
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpGet]
        [Authorize(Users = "userserviceStorm, userserviceFTP", Roles = "service")]
        [Route("api/Commande/ExportCommandToSap/{commandeId}")]
        public async Task<IHttpActionResult> ExportCommandeToSap(int commandeId)
        {
            try
            {
                Result<string> exportResult = await commandeFluxManager.EnqueueExportCommandeJobAsync(commandeId);
                return Ok(mapper.Map<ResultModel<string>>(exportResult));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Permet d'exporter un avenant de commande vers SAP
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpGet]
        [Authorize(Users = "userserviceStorm, userserviceFTP", Roles = "service")]
        [Route("api/Commande/ExportCommandAvenantToSap/{commandeId}/{numeroAvenant}")]
        public async Task<IHttpActionResult> ExportCommandeAvenantToSap(int commandeId, int numeroAvenant)
        {
            try
            {
                Result<string> exportResult = await commandeFluxManager.EnqueueExportCommandeAvenantJobAsync(commandeId, numeroAvenant);
                return Ok(mapper.Map<ResultModel<string>>(exportResult));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Permet d'importer une commande STORM dans Fred.
        /// </summary>
        /// <param name="commande">La commande.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Authorize(Users = "userserviceStorm, userserviceFTP", Roles = "service")]
        [Route("api/Commande/ImportCommandeStorm")]
        public async Task<IHttpActionResult> ImportCommandeStormAsync(CommandeSapModel commande)
        {
            return await PostTaskAsync(async () => await commandeFluxManager.ImportCommandeStorm(commande));
        }
    }
}
