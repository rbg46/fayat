using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Fred.ImportExport.Business.Pointage.Personnel.PointagePersonnelEtl;
using Fred.ImportExport.Business.Rapport;
using Hangfire;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Web API des rapports
    /// </summary>
    public class RapportController : ApiControllerBase
    {
        private readonly RapportFluxManager rapportFluxMgr;
        private readonly IPointageFluxManager pointageFluxManager;

        public RapportController(RapportFluxManager rapportFluxMgr,
            IPointageFluxManager pointageFluxManager)
        {
            this.rapportFluxMgr = rapportFluxMgr;
            this.pointageFluxManager = pointageFluxManager;
        }

        /// <summary>
        /// Export des rapports vers Helios
        /// </summary>
        /// <param name="dateDebut">date début</param>
        /// <param name="dateFin">date fin</param>
        /// <param name="codeSociete">code société du CI</param>
        /// <param name="codeEtablissement">code établissement comptable du CI</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpGet]
        [Route("api/Rapport/ExportRapportToHelios/{dateDebut}/{dateFin}/{codeSociete}/{codeEtablissement?}")]
        public HttpResponseMessage ExportRapportsToHelios(DateTime dateDebut, DateTime dateFin, string codeSociete, string codeEtablissement = null)
        {
            return Get(() => rapportFluxMgr.ExportFesRapport(dateDebut, dateFin, codeSociete, codeEtablissement));
        }

        /// <summary>
        /// API PRIVEE FRED -> FRED IE
        /// Permet d'exporter les pointages des PERSONNEL vers SAP.
        /// </summary>
        /// <param name="rapportId"> Id du rapport validé</param>
        /// <returns>JOB ID</returns>
        [Authorize(Roles = "service")]
        [HttpGet]
        [Route("api/Rapport/ExportPointagePersonnelToSap/{rapportId}")]
        public HttpResponseMessage ExportPointagePersonnelToSap(int rapportId)
        {
            return Get(() => BackgroundJob.Enqueue(() => pointageFluxManager.ExportPointagePersonnelToSap(rapportId, null)));
        }

        /// <summary>
        /// API PRIVEE FRED -> FRED IE
        /// Permet d'exporter les pointages des PERSONNEL vers SAP.
        /// </summary>
        /// <param name="rapportIds">Liste d'Id de rapport validé</param>
        /// <returns>JOB ID</returns>
        [Authorize(Roles = "service")]
        [HttpPost]
        [Route("api/Rapport/ExportPointagePersonnelToSap")]
        public HttpResponseMessage ExportPointagePersonnelToSap([FromBody] List<int> rapportIds)
        {
            return Post(() => BackgroundJob.Enqueue(() => pointageFluxManager.ExportPointagePersonnelToSap(rapportIds, null)));
        }

        /// <summary>
        /// Export des rapports vers Tibco
        /// </summary>
        /// <param name="user">identifiant de l'utilisateur</param>
        /// <param name="simulation">flag, si oui ne pas tenir compte du vérrouillage</param>
        /// <param name="periode">date période</param>
        /// <param name="type_periode">semaine ou mois</param>
        /// <param name="societe">code société</param>
        /// <param name="etabs">liste des codes établissements comptable</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Rapport/ExportRapportsToTibco/{user}/{simulation}/{periode}/{type_periode}/{societe}/{etabs}")]
        public HttpResponseMessage ExportRapportsToTibco(int user, bool simulation, DateTime periode, string type_periode, string societe, string etabs)
        {
            return Get(() => pointageFluxManager.ExportPointagePersonnelToTibco(user, simulation, periode, type_periode, societe, etabs));
        }
    }
}
