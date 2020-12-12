using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.ImportExport.Business.Tache;
using Fred.ImportExport.Models.Tache;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des Taches.
    /// </summary>
    [Authorize(Users = "userserviceFes", Roles = "service")]
    public class TacheController : ApiControllerBase
    {
        private readonly TacheFluxManager tacheFluxManager;

        public TacheController(TacheFluxManager tacheFluxManager)
        {
            this.tacheFluxManager = tacheFluxManager;
        }

        /// <summary>
        /// Permet d'importer une tache dans Fred.
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="tache">Un tache.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Tache/ImportTache")]
        public async Task<IHttpActionResult> ImportTachesAsync(TacheModel tache)
        {
            return await PostTaskAsync(async () => await tacheFluxManager.ImportTacheAsync(tache));
        }
    }
}
