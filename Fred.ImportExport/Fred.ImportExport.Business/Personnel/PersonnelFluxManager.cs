using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Log;
using Hangfire;

namespace Fred.ImportExport.Business
{
    public class PersonnelFluxManager : AbstractFluxManager
    {
        private readonly IImportExportLoggingService loggingService;
        private readonly IPersonnelFluxAnaelSystemService personnelFluxAnaelSystemService;
        private readonly IPersonnelFluxTibcoSystemService personnelFluxTibcoSystemService;
        private readonly IGroupeRepository groupRepository;
        private readonly IFluxRepository fluxRepository;

        public string ExportJobId => ConfigurationManager.AppSettings["flux:export:personnel:fes:figgo"];

        public PersonnelFluxManager(
            IFluxManager fluxManager,
            IImportExportLoggingService loggingService,
            IPersonnelFluxAnaelSystemService personnelFluxAnaelSystemService,
            IPersonnelFluxTibcoSystemService personnelFluxTibcoSystemService,
            IGroupeRepository groupRepository,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.loggingService = loggingService;
            this.personnelFluxAnaelSystemService = personnelFluxAnaelSystemService;
            this.personnelFluxTibcoSystemService = personnelFluxTibcoSystemService;
            this.groupRepository = groupRepository;
            this.fluxRepository = fluxRepository;
        }

        public void ExecuteImportByCodeFlux(bool bypassDate, string codeFlux)
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationProcessWithCodeFlux(bypassDate, codeFlux));
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
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationProcessWithCodeFlux(false, codeFlux), cron);
            }
            else
            {
                string msg = string.Format(FredImportExportBusinessResources.CronExpressionPasParametre, codeFlux);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT][FLUX_PERSONNEL] (ANAEL => FRED)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ImportationProcessWithCodeFlux(bool bypassDate, string codeFlux)
        {
            var parameter = new ImportationByCodeFluxParameter { BypassDate = bypassDate, CodeFlux = codeFlux };
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(codeFlux);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessWithCodeFlux", groupCode, parameter);
        }

        public async Task ImportationProcessWithCodeFluxJobAsync(ImportationByCodeFluxParameter parameter)
        {
            bool bypassDate = parameter.BypassDate;
            string codeFlux = parameter.CodeFlux;

            await personnelFluxAnaelSystemService.ImportationByCodeFluxAsync(codeFlux, bypassDate);
        }

        public void UpdatePersonnelsByListIds(List<int> personnelIds)
        {
            try
            {
                BackgroundJob.Enqueue(() => UpdatePersonnels(personnelIds));
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT_PERSONNEL_ON_UPDATE] PERSONNEL (FRED => SAP)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task<ImportResult> UpdatePersonnels(List<int> personnelIds)
        {
            var parameter = new UpdatePersonnelsParameter { PersonnelIds = personnelIds };
            string groupCode = await groupRepository.GetGroupCodeByStaffIdsAsync(personnelIds);

            return await JobRunnerApiRestHelper.PostAsync<ImportResult>("UpdatePersonnels", groupCode, parameter);
        }

        public async Task<ImportResult> UpdatePersonnelsJobAsync(UpdatePersonnelsParameter parameter)
        {
            List<int> personnelIds = parameter.PersonnelIds;

            return await personnelFluxAnaelSystemService.ImportationByPersonnelsIdsAsync(personnelIds);
        }

        public void ExecuteExportPersonnelToTibco(bool byPassDate)
        {
            try
            {
                BackgroundJob.Enqueue(() => ExportPersonnelToTibco(byPassDate));
            }
            catch (Exception e)
            {
                loggingService.LogError(e.Message);
                throw new FredBusinessException(e.Message, e);
            }
        }

        public void ScheduleExportPersonnelToTibco(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ExportJobId, () => ExportPersonnelToTibco(false), cron);
            }
            else
            {
                string msg = string.Format(FredImportExportBusinessResources.CronExpressionPasParametre);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[EXPORT_PERSONNEL_FES] [FIGGO] PERSONNEL (FRED => TIBCO)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ExportPersonnelToTibco(bool byPassDate)
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(ExportJobId);

            await JobRunnerApiRestHelper.PostAsync("ExportPersonnelToTibco", groupCode, byPassDate);
        }

        public void ExportPersonnelToTibcoJob(bool byPassDate)
        {
            FluxEnt flux = FluxManager.GetByCode(ExportJobId);
            personnelFluxTibcoSystemService.ExportPersonnelToTibco(byPassDate, flux.DateDerniereExecution);

            flux.DateDerniereExecution = DateTime.Now;
            FluxManager.Update(flux);
        }
    }
}
