using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.HangfireFilter;
using Fred.ImportExport.Business.CI;
using Fred.ImportExport.Business.CI.AnaelSystem;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.WebApi;
using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Models.Ci;
using Hangfire;
using NLog;
using ImportResultCI = Fred.ImportExport.Business.CI.AnaelSystem.Context.Common.ImportResult;

namespace Fred.ImportExport.Business
{
    public class CIFluxManager : AbstractFluxManager
    {
        private readonly IImportCiWebApiSystemService importCiWebApiSystemManager;
        private readonly ICiFluxAnaelSystemService ciFluxAnaelSystemService;
        private readonly IImportCiAnaelSystemManager importCiAnaelSystemManager;
        private readonly IGroupeRepository groupRepository;
        private readonly IFluxRepository fluxRepository;

        public CIFluxManager(
            IFluxManager fluxManager,
            IImportCiWebApiSystemService importCiWebApiSystemManager,
            ICiFluxAnaelSystemService ciFluxAnaelSystemService,
            IImportCiAnaelSystemManager importCiAnaelSystemManager,
            IGroupeRepository groupRepository,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.importCiWebApiSystemManager = importCiWebApiSystemManager;
            this.ciFluxAnaelSystemService = ciFluxAnaelSystemService;
            this.importCiAnaelSystemManager = importCiAnaelSystemManager;
            this.groupRepository = groupRepository;
            this.fluxRepository = fluxRepository;
        }

        public void ExecuteImportByCodeFlux(bool bypassDate, string codeFlux)
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationByCodeFlux(bypassDate, codeFlux));
            }
            catch (Exception e)
            {
                var exception = new FredBusinessException(e.Message, e);
                LogManager.GetCurrentClassLogger().Error(exception);
                throw exception;
            }
        }

        public void ToggleScheduleImportByCodeFlux(bool activate, string cron, string codeFlux)
        {
            if (!activate)
            {
                RecurringJob.RemoveIfExists(codeFlux);
                return;
            }

            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationByCodeFlux(false, codeFlux), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {codeFlux}";
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT_CI] CI ETL (ANAEL => FRED)")]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ImportationByCodeFlux(bool bypassDate, string codeFlux)
        {
            var parameter = new ImportationByCodeFluxParameter { CodeFlux = codeFlux, BypassDate = bypassDate };
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(codeFlux);

            await JobRunnerApiRestHelper.PostAsync("ImportCisByCodeFlux", groupCode, parameter);
        }

        public async Task ImportationByCodeFluxJobAsync(ImportationByCodeFluxParameter parameter)
        {
            string codeFlux = parameter.CodeFlux;
            bool bypassDate = parameter.BypassDate;

            await ciFluxAnaelSystemService.ImportationByCodeFluxAsync(codeFlux, bypassDate);
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT_CI_ON_UPDATE] CI ETL (ANAEL => FRED)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        public async Task<ImportResultCI> UpdateCis(List<int> ciIds)
        {
            var parameter = new ImportCisByCiListInputs { CiIds = ciIds };
            string groupCode = await groupRepository.GetGroupCodeByCiIdsAsync(ciIds);

            return await JobRunnerApiRestHelper.PostAsync<ImportResultCI>("UpdateCis", groupCode, parameter);
        }

        public async Task<ImportResultCI> UpdateCIsJobAsync(ImportCisByCiListInputs parameter)
        {
            return await importCiAnaelSystemManager.ImportCiByCiIdsAsync(parameter);
        }

        public void ImportCis(List<WebApiCiModel> cis)
        {
            importCiWebApiSystemManager.ImportCisByApi(new ImportCisByApiInputs { WebApiCis = cis });
        }

        public void ImportCi(WebApiCiModel ci)
        {
            var cis = new List<WebApiCiModel> { ci };

            ImportCis(cis);
        }
    }
}
