using System.ComponentModel;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Personnel.Etl.Process;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;
using Hangfire;

namespace Fred.ImportExport.Business.Personnel
{
    public class PersonnelFluxMultipleSocieteManager : AbstractFluxManager
    {
        private const string ErrorNoEtlFound = "[IMPORT][ERROR] Personnels (ANAEL => FRED) : Aucun ETL trouvé pour ce job. ";
        private const string ErrorNoCorrespondance = "[IMPORT][ERROR] Personnels (ANAEL => FRED) : Aucune correspondances trouvées.";

        private readonly PersonnelEtlFactory personnelEtlFactory;
        private readonly IFluxRepository fluxRepository;

        public PersonnelFluxMultipleSocieteManager(IFluxManager fluxManager, PersonnelEtlFactory personnelEtlFactory, IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.personnelEtlFactory = personnelEtlFactory;
            this.fluxRepository = fluxRepository;
        }

        public void ScheduleImportAllSocieteInSameTime(string cron, string codeFlux)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationMultipleSocietes(false, codeFlux), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {codeFlux}";
                ThrowError(msg);
            }
        }

        public void ExecuteImportAllSocieteInSameTime(bool byPassDate, string codeFlux)
        {
            BackgroundJob.Enqueue(() => ImportationMultipleSocietes(byPassDate, codeFlux));
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT][FLUX_PERSONNEL_MULTIPLES_SOCIETES] (ANAEL => FRED)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ImportationMultipleSocietes(bool bypassDate, string codeFlux)
        {
            var parameter = new ImportationByCodeFluxParameter { BypassDate = bypassDate, CodeFlux = codeFlux };
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(codeFlux);

            await JobRunnerApiRestHelper.PostAsync("ImportationMultipleSocietes", groupCode, parameter);
        }

        public async Task ImportationMultipleSocietesJobAsync(ImportationByCodeFluxParameter parameter)
        {
            bool bypassDate = parameter.BypassDate;
            string codeFlux = parameter.CodeFlux;

            FluxPersonnelCorrespondance correspondance = TableauCorrespondances.GetCorrespondance(codeFlux, null);
            if (correspondance != null)
            {
                FluxEnt flux = FluxManager.GetByCode(codeFlux);
                var personnelFluxParameter = new PersonnelEtlParameter(bypassDate, flux, correspondance.SqlScriptPath);
                IPersonnelEtl etl = personnelEtlFactory.GetEtl(correspondance, personnelFluxParameter);
                if (etl != null)
                {
                    etl.Init(personnelFluxParameter, FluxManager);
                    etl.Build();
                    await etl.ExecuteAsync();
                }
                else
                {
                    ThrowError(ErrorNoEtlFound);
                }
            }
            else
            {
                ThrowError(ErrorNoCorrespondance);
            }
        }

        private void ThrowError(string msg)
        {
            var exception = new FredBusinessException(msg);
            NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);
            throw exception;
        }
    }
}
