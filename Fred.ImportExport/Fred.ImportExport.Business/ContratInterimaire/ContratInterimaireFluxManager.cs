using System;
using System.ComponentModel;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Log;
using Hangfire;

namespace Fred.ImportExport.Business.ContratInterimaire
{
    public class ContratInterimaireFluxManager : AbstractFluxManager
    {
        private readonly IImportExportLoggingService loggingService;
        private readonly IContratInterimaireFluxTibcoSystemService contratInterimareFluxTibcoSystemService;

        public ContratInterimaireFluxManager(
            IFluxManager fluxManager,
            IImportExportLoggingService loggingService,
            IContratInterimaireFluxTibcoSystemService contratInterimareFluxTibcoSystemService)
            : base(fluxManager)
        {
            this.loggingService = loggingService;
            this.contratInterimareFluxTibcoSystemService = contratInterimareFluxTibcoSystemService;
        }

        public void ExecuteImportByCodeFlux(string codeFlux)
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationProcessWithCodeFlux(codeFlux));
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
                throw new FredBusinessException(e.Message, e);
            }
        }

        public void ScheduleImportByCodeFlux(string codeFlux, string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationProcessWithCodeFlux(codeFlux), cron);
            }
            else
            {
                string msg = string.Format(FredImportExportBusinessResources.CronExpressionPasParametre, codeFlux);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
        }


        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT][CONTRAT_INTERIMAIRE] (TIBCO => FRED)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public void ImportationProcessWithCodeFlux(string codeFlux)
        {
            FluxEnt flux = FluxManager.GetByCode(codeFlux);
            contratInterimareFluxTibcoSystemService.ImportContratInterimairePixid(flux.DateDerniereExecution);

            flux.DateDerniereExecution = DateTime.UtcNow;
            FluxManager.Update(flux);
        }
    }
}
