using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Framework.Extensions;
using Fred.Framework.Services;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel;
using Hangfire;
using static Fred.ImportExport.Business.Materiel.Common.EnumerationMateriel;

namespace Fred.ImportExport.Business.Materiel.ImportMaterielFaytTp
{
    public class MaterielFluxFayatTpManager : AbstractMaterielImport
    {
        private readonly IServiceImportMateriel serviceImportMateriel;
        private readonly RestClient restClient;

        public override string Login => ConfigurationManager.AppSettings["Storm:Login:Groupe:GFTP"];
        public override string Password => ConfigurationManager.AppSettings["Storm:Password:Groupe:GFTP"];
        public override string WebApiStormUrl => ConfigurationManager.AppSettings["Storm:WebApiUrl:Groupe:GFTP"];
        public override string ImportJobId => ConfigurationManager.AppSettings["flux:import:materiel:fayattp"];
        public override List<string> CodeSocieteComptables => SocieteCodeImportMaterielManager.GetAll(GroupCode.GRPFAYAT.ToIntValue());

        public MaterielFluxFayatTpManager(
            IFluxManager fluxManager,
            ISocieteCodeImportMaterielManager societeCodeImportMaterielManager,
            IServiceImportMateriel serviceImportMateriel)
            : base(fluxManager, societeCodeImportMaterielManager)
        {
            this.serviceImportMateriel = serviceImportMateriel;

            restClient = new RestClient(Login, Password);
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[MATERIEL][FLUX_IE03] Import materiel depuis BRIDGE")]
        public async Task ImportMaterielFromStorm(string date, RestClient restClient, string webApiStormUrl, string importJobId, List<string> codeSocieteComptables)
        {
            await JobRunnerApiRestHelper.PostAsync("ImportMaterielFromStormFayatTp", Constantes.CodeGroupeFTP, date);
        }

        public async Task ImportMaterielFromStormJobAsync(string date)
        {
            await serviceImportMateriel.ImportMaterielFromStormAsync(date, restClient, WebApiStormUrl, ImportJobId, CodeSocieteComptables);
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

            await ImportMaterielFromStorm(date, restClient, WebApiStormUrl, ImportJobId, CodeSocieteComptables);
        }
    }
}
