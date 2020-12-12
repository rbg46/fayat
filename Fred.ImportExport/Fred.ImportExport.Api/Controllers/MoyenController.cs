using System.Threading.Tasks;
using System.Web.Http;
using Fred.ImportExport.Business.Moyen;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Common;
using Fred.ImportExport.Models.Moyen;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des Moyens.
    /// </summary>
    public class MoyenController : ApiControllerBase
    {
        private readonly MoyenFluxManager moyenFluxMgr;

        public MoyenController(MoyenFluxManager moyenFluxMgr)
        {
            this.moyenFluxMgr = moyenFluxMgr;
        }

        /// <summary>
        /// Permet d'importer un moyen dans Fred.
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="moyen">Un moyen.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Moyen/ImportMoyen")]
        [Authorize(Users = "userserviceFes", Roles = "service")]
        public async Task<IHttpActionResult> ImportMoyensAsync(MoyenModel moyen)
        {
            return await PostTaskAsync(async () => await moyenFluxMgr.ImportMoyenAsync(moyen));
        }

        /// <summary>
        /// Permet de faire l'export du pointage des moyens
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="model">Model de l'export des pointages</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Moyen/ExportPointageMoyen")]
        [Authorize(Roles = "service")]
        public async Task<IHttpActionResult> ExportPointageMoyenAsync(ExportPointageMoyenModel model)
        {
            EnvoiPointageMoyenResultModel result = await moyenFluxMgr.ExportPointageMoyenAsync(model);
            return Ok(result);
        }
    }
}
