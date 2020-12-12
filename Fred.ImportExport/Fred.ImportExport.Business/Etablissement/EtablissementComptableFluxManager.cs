using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.HangfireFilter;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Etablissement;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Framework.Log;
using Hangfire;

namespace Fred.ImportExport.Business
{
    public class EtablissementComptableFluxManager : AbstractFluxManager
    {
        private readonly IImportExportLoggingService loggingService;
        private readonly IEtablissementComptableFluxService etablissementComptableService;
        private readonly IFluxRepository fluxRepository;

        public EtablissementComptableFluxManager(
            IFluxManager fluxManager,
            IImportExportLoggingService loggingService,
            IEtablissementComptableFluxService etablissementComptableService,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.loggingService = loggingService;
            this.etablissementComptableService = etablissementComptableService;
            this.fluxRepository = fluxRepository;
        }

        public void ExecuteImport(string codeFlux)
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationProcess(codeFlux));
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
                throw new FredBusinessException(e.Message, e);
            }
        }

        public void ScheduleImportByCodeFlux(string cron, string codeFlux)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationProcess(codeFlux), cron);
            }
            else
            {
                string msg = string.Format(FredImportExportBusinessResources.CronExpressionPasParametre, codeFlux);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Etablissements Comptables (ANAEL => FRED)")]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        public async Task ImportationProcess(string codeFlux)
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(codeFlux);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessEtablissementComptable", groupCode);
        }

        public async Task ImportationProcessJobAsync()
        {
            await etablissementComptableService.ImportEtablissementComptableAsync();
        }
    }
}
