using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.BaremeExploitation;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.CommandeLigne.VME22;
using Fred.ImportExport.Business.Depense;
using Fred.ImportExport.Business.EcritureComptable.Interfaces;
using Fred.ImportExport.Business.Email.ActivitySummary;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.JournauxComptable;
using Fred.ImportExport.Business.Kilometre;
using Fred.ImportExport.Business.Materiel;
using Fred.ImportExport.Business.Materiel.ImportMaterielFaytTp;
using Fred.ImportExport.Business.OperationDiverse;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.Pointage.Personnel.PointagePersonnelEtl;
using Fred.ImportExport.Business.Reception.Migo;
using Fred.ImportExport.Business.ReceptionInterimaire;
using Fred.ImportExport.Business.Stair.ExportCI;
using Fred.ImportExport.Business.Stair.ExportPersonnel;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Rzb;
using Fred.ImportExport.Models.Commande;

namespace Fred.ImportExport.Api.Controllers
{
    [Authorize(Roles = "service")]
    [RoutePrefix("api/JobRunner")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class JobRunnerController : ApiController
    {
        private readonly CIFluxManager cIFluxManager;
        private readonly BaremeExploitationFluxManager baremeExploitationFluxManager;
        private readonly CommandeFluxManager commandeFluxManager;
        private readonly DepenseFluxManager depenseFluxManager;
        private readonly IEcritureComptableFluxManager ecritureComptableFluxManager;
        private readonly EmailActivitySummaryHanfireJob emailActivitySummaryHangfireJob;
        private readonly EtablissementComptableFluxManager etablissementComptableFluxManager;
        private readonly FournisseurFluxManager fournisseurFluxManager;
        private readonly JournauxComptableFluxManager journauxComptableFluxManager;
        private readonly KlmFluxManager klmFluxManager;
        private readonly MaterielFluxManager materielFluxManager;
        private readonly MaterielFluxFayatTpManager materielFluxFayatTpManager;
        private readonly PersonnelFluxManager personnelFluxManager;
        private readonly PersonnelFluxMultipleSocieteManager personnelFluxMultipleSocieteManager;
        private readonly PointageFluxManager pointageFluxManager;
        private readonly ReceptionInterimaireFluxManager receptionInterimaireFluxManager;
        private readonly MigoManager migoManager;
        private readonly StairCiFluxManager stairCiFluxManager;
        private readonly StairPersonnelFluxManager stairPersonnelFluxManager;
        private readonly ControleVracFesHangFireJob controleVracFesHangFireJob;
        private readonly RemonteeVracFesHangFireJob remonteeVracFesHangFireJob;
        private readonly ControleVracFonHangFireJob controleVracFonHangFireJob;
        private readonly RemonteeVracFonHangFireJob remonteeVracFonHangFireJob;
        private readonly ControleVracFtpHangFireJob controleVracFtpHangFireJob;
        private readonly RemonteeVracFtpHangFireJob remonteeVracFtpHangFireJob;
        private readonly ControleVrac controleVracRzbHangFireJob;
        private readonly RemonteeVrac remonteeVracRzbHangFireJob;
        private readonly RemonteePrimes remonteePrimes;
        private readonly Vme22FluxManager vme22FluxManager;

        public JobRunnerController(
            CIFluxManager cIFluxManager,
            BaremeExploitationFluxManager baremeExploitationFluxManager,
            CommandeFluxManager commandeFluxManager,
            DepenseFluxManager depenseFluxManager,
            IEcritureComptableFluxManager ecritureComptableFluxManager,
            EmailActivitySummaryHanfireJob emailActivitySummaryHangfireJob,
            EtablissementComptableFluxManager etablissementComptableFluxManager,
            FournisseurFluxManager fournisseurFluxManager,
            JournauxComptableFluxManager journauxComptableFluxManager,
            KlmFluxManager klmFluxManager,
            MaterielFluxManager materielFluxManager,
            MaterielFluxFayatTpManager materielFluxFayatTpManager,
            PersonnelFluxManager personnelFluxManager,
            PersonnelFluxMultipleSocieteManager personnelFluxMultipleSocieteManager,
            PointageFluxManager pointageFluxManager,
            ReceptionInterimaireFluxManager receptionInterimaireFluxManager,
            MigoManager migoManager,
            StairCiFluxManager stairCiFluxManager,
            StairPersonnelFluxManager stairPersonnelFluxManager,
            ControleVracFesHangFireJob controleVracFesHangFireJob,
            RemonteeVracFesHangFireJob remonteeVracFesHangFireJob,
            ControleVracFonHangFireJob controleVracFonHangFireJob,
            RemonteeVracFonHangFireJob remonteeVracFonHangFireJob,
            ControleVracFtpHangFireJob controleVracFtpHangFireJob,
            RemonteeVracFtpHangFireJob remonteeVracFtpHangFireJob,
            ControleVrac controleVracRzbHangFireJob,
            RemonteeVrac remonteeVracRzbHangFireJob,
            RemonteePrimes remonteePrimes,
            Vme22FluxManager vme22FluxManager)
        {
            this.cIFluxManager = cIFluxManager;
            this.baremeExploitationFluxManager = baremeExploitationFluxManager;
            this.commandeFluxManager = commandeFluxManager;
            this.depenseFluxManager = depenseFluxManager;
            this.ecritureComptableFluxManager = ecritureComptableFluxManager;
            this.emailActivitySummaryHangfireJob = emailActivitySummaryHangfireJob;
            this.etablissementComptableFluxManager = etablissementComptableFluxManager;
            this.fournisseurFluxManager = fournisseurFluxManager;
            this.journauxComptableFluxManager = journauxComptableFluxManager;
            this.klmFluxManager = klmFluxManager;
            this.materielFluxManager = materielFluxManager;
            this.materielFluxFayatTpManager = materielFluxFayatTpManager;
            this.personnelFluxManager = personnelFluxManager;
            this.personnelFluxMultipleSocieteManager = personnelFluxMultipleSocieteManager;
            this.pointageFluxManager = pointageFluxManager;
            this.receptionInterimaireFluxManager = receptionInterimaireFluxManager;
            this.migoManager = migoManager;
            this.stairCiFluxManager = stairCiFluxManager;
            this.stairPersonnelFluxManager = stairPersonnelFluxManager;
            this.controleVracFesHangFireJob = controleVracFesHangFireJob;
            this.remonteeVracFesHangFireJob = remonteeVracFesHangFireJob;
            this.controleVracFonHangFireJob = controleVracFonHangFireJob;
            this.remonteeVracFonHangFireJob = remonteeVracFonHangFireJob;
            this.controleVracFtpHangFireJob = controleVracFtpHangFireJob;
            this.remonteeVracFtpHangFireJob = remonteeVracFtpHangFireJob;
            this.controleVracRzbHangFireJob = controleVracRzbHangFireJob;
            this.remonteeVracRzbHangFireJob = remonteeVracRzbHangFireJob;
            this.remonteePrimes = remonteePrimes;
            this.vme22FluxManager = vme22FluxManager;
        }

        [HttpPost]
        [Route("ImportCisByCodeFlux")]
        public async Task<IHttpActionResult> ImportCisByCodeFluxAsync([FromBody] ImportationByCodeFluxParameter parameter)
        {
            await cIFluxManager.ImportationByCodeFluxJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateCis")]
        public async Task<IHttpActionResult> UpdateCisAsync([FromBody] ImportCisByCiListInputs parameter)
        {
            ImportResult importResult = await cIFluxManager.UpdateCIsJobAsync(parameter);

            return Ok(importResult);
        }

        [HttpPost]
        [Route("ImportBaremes")]
        public IHttpActionResult ImportBaremes([FromBody] ImportationByInputPathParameter parameter)
        {
            baremeExploitationFluxManager.ImportationProcessJob(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportCommandeToSap")]
        public async Task<IHttpActionResult> ExportCommandeToSapAsync([FromBody] int commandeId)
        {
            await commandeFluxManager.ExportCommandeToSapJobAsync(commandeId);

            return Ok();
        }

        [HttpPost]
        [Route("ExportCommandeAvenantToSap")]
        public async Task<IHttpActionResult> ExportCommandeAvenantToSapAsync([FromBody] ExportCommandeAvenantToSapParameter parameter)
        {
            await commandeFluxManager.ExportCommandeAvenantToSapJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportMaterielExterne")]
        public async Task<IHttpActionResult> ExportMaterielExterneAsync([FromBody] ExportBySocieteParameter parameter)
        {
            await depenseFluxManager.ExportMaterielExterneJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessRecurring")]
        public async Task<IHttpActionResult> ImportationProcessRecurringAsync([FromBody] ImportationProcessRecurringParameter parameter)
        {
            await ecritureComptableFluxManager.ImportationProcessRecurringJobAsync(parameter).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessRecurringRange")]
        public async Task<IHttpActionResult> ImportationProcessRecurringRangeAsync([FromBody] ImportationProcessRecurringRangeParameter parameter)
        {
            await ecritureComptableFluxManager.ImportationProcessRecurringRangeJobAsync(parameter).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationPartialProcessRecurringRange")]
        public async Task<IHttpActionResult> ImportationPartialProcessRecurringRangeAsync([FromBody] ImportationProcessRecurringRangeParameter parameter)
        {
            await ecritureComptableFluxManager.ImportationPartialProcessRecurringRangeJobAsync(parameter).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcess")]
        public async Task<IHttpActionResult> ImportationProcessAsync([FromBody] ImportationProcessParameter parameter)
        {
            await ecritureComptableFluxManager.ImportationProcessJobAsync(parameter).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessRange")]
        public async Task<IHttpActionResult> ImportationProcessRangeAsync([FromBody] ImportationProcessRangeParameter parameter)
        {
            await ecritureComptableFluxManager.ImportationProcessRangeJobAsync(parameter).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Route("SendEmail")]
        public async Task<IHttpActionResult> SendEmail()
        {
            emailActivitySummaryHangfireJob.SendEmail();

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessEtablissementComptable")]
        public async Task<IHttpActionResult> ImportationProcessAsync()
        {
            await etablissementComptableFluxManager.ImportationProcessJobAsync();

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessFournisseur")]
        public async Task<IHttpActionResult> ImportationProcessAsync([FromBody] ImportationFournisseurParameter parameter)
        {
            await fournisseurFluxManager.ImportationProcessJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessJournauxComptable")]
        public async Task<IHttpActionResult> ImportationProcessJournauxComptableAsync([FromBody] string fluxCode)
        {
            await journauxComptableFluxManager.ImportationProcessJobAsync(fluxCode);

            return Ok();
        }

        [HttpPost]
        [Route("ExportKlm")]
        public IHttpActionResult ExportKlm()
        {
            klmFluxManager.ExportJob();

            return Ok();
        }

        [HttpPost]
        [Route("ExportPointageMaterielToStorm")]
        public async Task<IHttpActionResult> ExportPointageMaterielToStormAsync([FromBody] ExportByRapportParameter parameter)
        {
            await materielFluxManager.ExportPointageMaterielToStormJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportPointageMaterielListToStorm")]
        public async Task<IHttpActionResult> ExportPointageMaterielListToStormAsync([FromBody] ExportByRapportsParameter parameter)
        {
            await materielFluxManager.ExportPointageMaterielListToStormJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ImportMaterielFromStorm")]
        public async Task<IHttpActionResult> ImportMaterielFromStormAsync([FromBody] string date)
        {
            await materielFluxManager.ImportMaterielFromStormJobAsync(date);

            return Ok();
        }

        [HttpPost]
        [Route("ImportMaterielFromStormFayatTp")]
        public async Task<IHttpActionResult> ImportMaterielFromStormFayatTpAsync([FromBody] string date)
        {
            await materielFluxFayatTpManager.ImportMaterielFromStormJobAsync(date);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationProcessWithCodeFlux")]
        public async Task<IHttpActionResult> ImportationProcessWithCodeFluxAsync([FromBody] ImportationByCodeFluxParameter parameter)
        {
            await personnelFluxManager.ImportationProcessWithCodeFluxJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("UpdatePersonnels")]
        public async Task<IHttpActionResult> UpdatePersonnelsAsync([FromBody] UpdatePersonnelsParameter parameter)
        {
            await personnelFluxManager.UpdatePersonnelsJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportPersonnelToTibco")]
        public IHttpActionResult ExportPersonnelToTibco([FromBody] bool byPassDate)
        {
            personnelFluxManager.ExportPersonnelToTibcoJob(byPassDate);

            return Ok();
        }

        [HttpPost]
        [Route("ImportationMultipleSocietes")]
        public async Task<IHttpActionResult> ImportationMultipleSocietesAsync([FromBody] ImportationByCodeFluxParameter parameter)
        {
            await personnelFluxMultipleSocieteManager.ImportationMultipleSocietesJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportPointagePersonnelToSap")]
        public async Task<IHttpActionResult> ExportPointagePersonnelToSapAsync([FromBody] ExportByRapportParameter parameter)
        {
            await pointageFluxManager.ExportPointagePersonnelToSapJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportPointagePersonnelListToSap")]
        public async Task<IHttpActionResult> ExportPointagePersonnelToSapAsync([FromBody] ExportByRapportsParameter parameter)
        {
            await pointageFluxManager.ExportPointagePersonnelToSapJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportReceptionInterimaire")]
        public async Task<IHttpActionResult> ExportReceptionInterimaireAsync([FromBody]ExportByCIParameter parameter)
        {
            await receptionInterimaireFluxManager.ExportReceptionInterimaireJob(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportMigoReceptionToSapForSociete")]
        public async Task<IHttpActionResult> ExportMigoReceptionToSapForSocieteAsync([FromBody]ExportReceptionToSapForSocieteParameter parameter)
        {
            await migoManager.ExportReceptionToSapForSocieteJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportStairCi")]
        public IHttpActionResult ExportStairCi()
        {
            stairCiFluxManager.ExportJob();

            return Ok();
        }

        [HttpPost]
        [Route("ExportStairPersonnel")]
        public IHttpActionResult ExportStairPersonnel()
        {
            stairPersonnelFluxManager.ExportJob();

            return Ok();
        }

        [HttpPost]
        [Route("ControleVracFesJob")]
        public async Task<IHttpActionResult> ControleVracFesJob([FromBody] ControleVracJobParameter parameter)
        {
            await controleVracFesHangFireJob.ControleVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("RemonteeVracFesJob")]
        public async Task<IHttpActionResult> RemonteeVracFesJobAsync([FromBody] RemonteeVracJobParameter parameter)
        {
            await remonteeVracFesHangFireJob.RemonteeVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ControleVracFonJob")]
        public async Task<IHttpActionResult> ControleVracFonJob([FromBody] ControleVracJobParameter parameter)
        {
            await controleVracFonHangFireJob.ControleVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("RemonteeVracFonJob")]
        public async Task<IHttpActionResult> RemonteeVracFonJobAsync([FromBody] RemonteeVracJobParameter parameter)
        {
            await remonteeVracFonHangFireJob.RemonteeVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ControleVracFtpJob")]
        public async Task<IHttpActionResult> ControleVracFtpJobAsync([FromBody] ControleVracJobWithEntityParameter parameter)
        {
            await controleVracFtpHangFireJob.ControleVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("RemonteeVracFtpJob")]
        public async Task<IHttpActionResult> RemonteeVracFtpJobAsync([FromBody] RemonteeVracJobParameter parameter)
        {
            await remonteeVracFtpHangFireJob.RemonteeVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ControleVracRzbJob")]
        public async Task<IHttpActionResult> ControleVracRzbJobAsync([FromBody] ControleVracJobParameter parameter)
        {
            await controleVracRzbHangFireJob.ControleVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("RemonteeVracRzbJob")]
        public async Task<IHttpActionResult> RemonteeVracRzbJobAsync([FromBody] RemonteeVracJobParameter parameter)
        {
            await remonteeVracRzbHangFireJob.RemonteeVracJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("RemonteePrimesJob")]
        public async Task<IHttpActionResult> RemonteePrimesJob([FromBody] RemonteePrimesJobParameter parameter)
        {
            await remonteePrimes.RemonteePrimesJobAsync(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ImportCommandeStorm")]
        public IHttpActionResult ImportCommandeStorm([FromBody] CommandeSapModel parameter)
        {
            commandeFluxManager.ImportCommandeStormJob(parameter);

            return Ok();
        }

        [HttpPost]
        [Route("ExportManualUnlockLigneDeComandeToSap")]
        public async Task<IHttpActionResult> ExportManualUnlockLigneDeComandeToSap([FromBody] ExportManualLockUnlockLigneDeCommandeParameters parameters)
        {
            await vme22FluxManager.ExportManualLockUnlockLigneDeCommandeToSapJob(parameters);

            return Ok();
        }
    }
}
