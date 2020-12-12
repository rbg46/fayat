using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.Entities.JobStatut;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.EcritureComptable.Interfaces;
using Fred.ImportExport.Business.OperationDiverse;
using Fred.ImportExport.Models.CloturePeriode;
using Fred.ImportExport.Models.EcritureComptable;
using Hangfire.Common;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Asp.Net des transferts de Ecriture comptable d'Anael vers Fred
    /// N'utilise pas le versionning
    /// </summary>
    [Authorize(Roles = "service")]
    public class EcritureComptableController : ApiControllerBase
    {
        private readonly IEcritureComptableFluxManager ecritureComptableFluxManager;

        public EcritureComptableController(IEcritureComptableFluxManager ecritureComptableFluxManager)
        {
            this.ecritureComptableFluxManager = ecritureComptableFluxManager;
        }

        /// <summary>
        /// Demande le status du job des ecritures comptables
        /// </summary>     
        /// <param name="userId">L'idenfiant de l'utilisateur.</param>
        /// <param name="societeId">L'idenfiant de la société.</param>
        /// <param name="ciId">L'idenfiant du CI.</param>
        /// <param name="dateComptable">La date comptable.</param>
        /// <returns>l'etat</returns>
        [HttpGet]
        [Route("api/EcritureComptable/CheckImport/{userId}/{societeId}/{ciId}/{dateComptable}")]
        public HttpResponseMessage CheckImportEcritureComptables(int userId, int societeId, int ciId, DateTime dateComptable)
        {
            return Get(() =>
            {
                Predicate<Job> predicateSelector = ecritureComptableFluxManager.GetPredicateForCheckRunningJob(societeId, ciId, dateComptable);
                return JobInfoProvider.GetJobStatus(nameof(EcritureComptableFluxManager), nameof(EcritureComptableFluxManager.ImportationProcess), predicateSelector);
            });
        }

        /// <summary>
        /// Execute l'import des ecritures comptables
        /// </summary>
        /// <param name="userId">L'idenfiant de l'utilisateur.</param>
        /// <param name="societeId">L'idenfiant de la société.</param>
        /// <param name="ciId">L'idenfiant du CI.</param>
        /// <param name="dateComptable">La date comptable.</param> 
        /// <returns>l'etat</returns>
        [HttpPost]
        [Route("api/EcritureComptable/Import/{userId}/{societeId}/{ciId}/{dateComptable}")]
        public async Task<IHttpActionResult> Import(int userId, int societeId, int ciId, DateTime dateComptable)
        {
            Predicate<Job> predicateSelector = ecritureComptableFluxManager.GetPredicateForCheckRunningJob(societeId, ciId, dateComptable);
            JobStatutModel info = JobInfoProvider.GetJobStatus(nameof(EcritureComptableFluxManager), nameof(EcritureComptableFluxManager.ImportationProcess), predicateSelector);
            if (info.IsEnqueued && info.IsRunning)
            {
                return Ok(info);
            }

            JobStatutModel jobStatus = await ecritureComptableFluxManager.ExecuteImportAsync(userId, societeId, ciId, dateComptable, string.Empty);
            return Ok(jobStatus);
        }

        /// <summary>
        /// Execute l'import des ecritures comptables pour une période données
        /// </summary>
        /// <param name="userId">L'idenfiant de l'utilisateur.</param>
        /// <param name="societeCode">Code de la société.</param>
        /// <param name="societeId">L'idenfiant de la société.</param>
        /// <param name="ciId">L'idenfiant du CI.</param>
        /// <param name="dateComptableDebut">Date de début</param> 
        /// <param name="dateComptableFin">Date de fin</param> 
        /// <returns>l'etat</returns>
        [HttpPost]
        [Route("api/EcritureComptable/Import/{userId}/{societeCode}/{societeId}/{ciId}/{dateComptableDebut}/{dateComptableFin}")]
        public async Task<IHttpActionResult> ImportAsync(int userId, string societeCode, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            Predicate<Job> predicateSelector = ecritureComptableFluxManager.GetPredicateForCheckRunningJob(societeId, ciId, dateComptableDebut, dateComptableFin);
            JobStatutModel info = JobInfoProvider.GetJobStatus(nameof(EcritureComptableFluxManager), nameof(EcritureComptableFluxManager.ImportationProcess), predicateSelector);
            if (info.IsEnqueued && info.IsRunning)
            {
                return Ok(info);
            }
            JobStatutModel jobStatus = await ecritureComptableFluxManager.ExecuteImportAsync(userId, societeId, ciId, dateComptableDebut, dateComptableFin);
            return Ok(jobStatus);
        }

        /// <summary>
        /// Execute l'import des ecritures comptables pour un Json
        /// </summary>
        /// <param name="ecritureComptables">Flux Json</param>
        /// <returns>Json de retour</returns>
        [OverrideAuthorization]
        [Authorize(Users = "userserviceFtp", Roles = "service")]
        [HttpPost]
        [Route("api/EcritureComptable/ImportJson")]
        public async Task<IHttpActionResult> ImportEcritureComptable(List<EcritureComptableFayatTpImportModel> ecritureComptables)
        {
            return Ok(await ecritureComptableFluxManager.ImportEcritureComptableAsync(ecritureComptables).ConfigureAwait(false));
        }

        /// <summary>
        /// Retourne les ecritures comptables depuis Anael pour une liste de CI
        /// </summary>
        /// <param name="cloturePeriodeModel"><see cref="CloturePeriodeIEModel"/></param>
        /// <returns>Enumerable des ecritures comptables</returns>
        [HttpPost]
        [Route("api/EcritureComptable/ImportEcrituresComptablesFromAnael/")]
        public async Task<IHttpActionResult> GetEcrituresComptablesFromAnaelAsync(CloturePeriodeIEModel cloturePeriodeModel)
        {
            return Ok(await ecritureComptableFluxManager.ImportEcrituresComptablesFromAnaelAsync(cloturePeriodeModel).ConfigureAwait(false));
        }
    }
}
