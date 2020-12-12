using System.Collections.Generic;
using System.Web.Http;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.ReceptionInterimaire;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des imports Personnel.
    /// </summary>
    [Authorize(Roles = "service")]
    public class PersonnelController : ApiControllerBase
    {
        private readonly PersonnelFluxManager personnelFluxManager;
        private readonly ReceptionInterimaireFluxManager receptionInterimaireFluxManager;

        public PersonnelController(PersonnelFluxManager personnelFluxManager, ReceptionInterimaireFluxManager receptionInterimaireFluxManager)
        {
            this.personnelFluxManager = personnelFluxManager;
            this.receptionInterimaireFluxManager = receptionInterimaireFluxManager;
        }

        /// <summary>
        /// Mise a jour de personnel demander par Fred web, demande anael info et envoie a SAP si c'est necessaire
        /// </summary>
        /// <param name="personnelIds">Liste de personnel ids</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Personnel/UpdatePersonnels")]
        public IHttpActionResult UpdatePersonnels([FromBody] List<int> personnelIds)
        {
            personnelFluxManager.UpdatePersonnelsByListIds(personnelIds);

            return Ok();
        }

        /// <summary>
        /// Export des réceptions des intérimaires vers SAP
        /// </summary>
        /// <param name="utilisateurId">Identifiant utilisateur</param>
        /// <param name="societesIds">Liste de identifiants des sociétés</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/personnel/ExportReceptionInterimairesToSap/{utilisateurId}")]
        public IHttpActionResult ExportReceptionInterimairesToSap(int utilisateurId, [FromBody]List<int> societesIds)
        {
            receptionInterimaireFluxManager.ExecuteExport(societesIds, utilisateurId);

            return Ok();
        }

        /// <summary>
        /// Export des réceptions des intérimaires vers SAP by ci
        /// </summary>
        /// <param name="utilisateurId">Identifiant utilisateur</param>
        /// <param name="ciIds">Liste de identifiants des cis</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/personnel/ExportReceptionInterimairesByCisToSap/{utilisateurId}")]
        public IHttpActionResult ExportReceptionInterimairesByCisToSap(int utilisateurId, [FromBody] List<int> ciIds)
        {
            receptionInterimaireFluxManager.ExecuteExportByCis(ciIds, utilisateurId);

            return Ok();
        }
    }
}
