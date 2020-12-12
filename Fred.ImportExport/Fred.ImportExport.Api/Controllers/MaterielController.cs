using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.ImportExport.Business.Materiel;
using Fred.ImportExport.Business.Materiel.ImportMaterielEtl;
using Fred.ImportExport.Models.Materiel;
using Hangfire;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des materiels.
    /// </summary>
    [Authorize(Users = "userserviceStorm, userserviceFtp", Roles = "service")]
    public class MaterielController : ApiControllerBase
    {
        private readonly MaterielFluxManager materielFluxManager;
        private readonly ImportMaterielManager importMaterielManager;

        /// <summary>
        /// Gestionnaire de materiel
        /// </summary>
        /// <param name="materielFluxManager">flux manager</param>
        /// <param name="importMaterielManager">import materiel manager</param>
        public MaterielController(MaterielFluxManager materielFluxManager, ImportMaterielManager importMaterielManager)
        {
            this.materielFluxManager = materielFluxManager;
            this.importMaterielManager = importMaterielManager;
        }

        /// <summary>
        /// Permet d'importer les materiels depuis STORM
        /// depuis une date.
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="date">La date de modification. Format : yyyy-MM-dd</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpGet]
        [Route("api/Materiel/ImportMaterielFromStorm/{date}")]
        public async Task<IHttpActionResult> ImportMaterielFromStormAsync(string date)
        {
            return await PostTaskAsync(async () => await materielFluxManager.ImportMaterielFromStorm(date));
        }

        /// <summary>
        /// Permet d'exporter les pointages des materiels vers STORM.
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpGet]
        [Route("api/Materiel/ExportPointageMaterielToStorm/{rapportId}")]
        public HttpResponseMessage ExportPointageMaterielToStorm(int rapportId)
        {
            return Get(() => BackgroundJob.Enqueue(() => materielFluxManager.ExportPointageMaterielToStorm(rapportId, null)));
        }

        /// <summary>
        /// Permet d'exporter les pointages des materiels d'une liste de rapports
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport.</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Materiel/ExportPointageMaterielToStorm")]
        public HttpResponseMessage ExportPointageMaterielToStorm(List<int> rapportIds)
        {
            return Post(() => BackgroundJob.Enqueue(() => materielFluxManager.ExportPointageMaterielToStorm(rapportIds, null)));
        }

        /// <summary>
        /// Permet d'importer un materiel dans Fred.
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="materiel">Un materiel.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Materiel/ImportMateriel")]
        public async Task<IHttpActionResult> ImportMaterielsAsync(ImportMaterielModel materiel)
        {
            return await PostTaskAsync(async () => await importMaterielManager.ImportMaterielAsync(materiel));
        }
    }
}
