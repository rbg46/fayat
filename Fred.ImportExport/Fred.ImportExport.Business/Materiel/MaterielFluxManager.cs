using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework.Extensions;
using Fred.Framework.Services;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Materiel.ExportMaterielToSap;
using Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel;
using Hangfire;
using Hangfire.Server;
using static Fred.ImportExport.Business.Materiel.Common.EnumerationMateriel;

namespace Fred.ImportExport.Business.Materiel
{
    /// <summary>
    /// Gestionnaire des flux des materiels.
    /// </summary>
    public class MaterielFluxManager : AbstractMaterielImport
    {
        private readonly IExportMaterielToSapManager exportMaterielToSapManager;
        private readonly IServiceImportMateriel serviceImportMateriel;
        private readonly IGroupeRepository groupRepository;
        private readonly RestClient restClient;

        public override string Login => ConfigurationManager.AppSettings["Storm:Login"];
        public override string Password => ConfigurationManager.AppSettings["Storm:Password"];
        public override string WebApiStormUrl => ConfigurationManager.AppSettings["Storm:WebApiUrl"];
        public override string ImportJobId => ConfigurationManager.AppSettings["flux:materiel"];
        public override List<string> CodeSocieteComptables => SocieteCodeImportMaterielManager.GetAll(GroupCode.GRPRZL.ToIntValue());

        public MaterielFluxManager(
            IFluxManager fluxManager,
            ISocieteCodeImportMaterielManager societeCodeImportMaterielManager,
            IExportMaterielToSapManager exportMaterielToSapManager,
            IServiceImportMateriel serviceImportMateriel,
            IGroupeRepository groupRepository)
            : base(fluxManager, societeCodeImportMaterielManager)
        {
            this.exportMaterielToSapManager = exportMaterielToSapManager;
            this.serviceImportMateriel = serviceImportMateriel;
            this.groupRepository = groupRepository;

            restClient = new RestClient(Login, Password);
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[MATERIEL][FLUX_IE03] Import materiel depuis STORM")]
        public async Task ImportMaterielFromStorm(string date)
        {
            await JobRunnerApiRestHelper.PostAsync("ImportMaterielFromStorm", Constantes.CodeGroupeRZB, date);
        }

        public async Task ImportMaterielFromStormJobAsync(string date)
        {
            await serviceImportMateriel.ImportMaterielFromStormAsync(date, restClient, WebApiStormUrl, ImportJobId, CodeSocieteComptables);
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[MATERIEL][POINTAGE][FLUX_J3G$] Export pointage vers STORM du rapportId : {0}")]
        public async Task ExportPointageMaterielToStorm(int rapportId, PerformContext context)
        {
            var parameter = new ExportByRapportParameter { RapportId = rapportId, BackgroundJobId = context.BackgroundJob.Id };
            string groupCode = await groupRepository.GetGroupCodeByReportIdAsync(rapportId);

            await JobRunnerApiRestHelper.PostAsync("ExportPointageMaterielToStorm", groupCode, parameter);
        }

        public async Task ExportPointageMaterielToStormJobAsync(ExportByRapportParameter parameter)
        {
            int rapportId = parameter.RapportId;
            string backgroundJobId = parameter.BackgroundJobId;

            List<Exception> exceptions = await exportMaterielToSapManager.ExportPointageToStormAsync(rapportId, backgroundJobId);
            if (exceptions.Any())
            {
                throw exceptions.First();
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[MATERIEL][POINTAGE][FLUX_J3G$] Export pointage vers STORM (Liste de rapports)")]
        public async Task ExportPointageMaterielToStorm(List<int> rapportIds, PerformContext context)
        {
            var parameter = new ExportByRapportsParameter { RapportIds = rapportIds, BackgroundJobId = context.BackgroundJob.Id };
            string groupCode = await groupRepository.GetGroupCodeByReportIdsAsync(rapportIds);

            await JobRunnerApiRestHelper.PostAsync("ExportPointageMaterielListToStorm", groupCode, parameter);
        }

        public async Task ExportPointageMaterielListToStormJobAsync(ExportByRapportsParameter parameter)
        {
            List<int> rapportIds = parameter.RapportIds;
            string backgroundJobId = parameter.BackgroundJobId;

            await exportMaterielToSapManager.ExportPointageToStormAsync(rapportIds, backgroundJobId);
        }

        public override async Task ImportMaterielFromStormAsync(bool isFull)
        {
            var date = "1900-01-01";
            if (!isFull)
            {
                Flux = FluxManager.GetByCode(ImportJobId);
                DateTime? datetime = Flux.DateDerniereExecution;
                date = datetime.HasValue ? datetime.Value.ToString("yyyy-MM-dd") : "1900-01-01";
            }

            await ImportMaterielFromStorm(date);
        }
    }
}
