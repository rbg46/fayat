using System.Configuration;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Stair.ImportSphinxEtl.Process;
using Hangfire;

namespace Fred.ImportExport.Business.Stair
{
    public class SphinxFluxManager : AbstractFluxManager
    {
        public string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:stair:import:sphinx"];

        public SphinxFluxManager(IFluxManager fluxManager)
            : base(fluxManager)
        {
            Flux = FluxManager.GetByCode(ImportJobId);
        }

        public void ExecuteImport()
        {
            BackgroundJob.Enqueue(() => ImportAsync());
        }

        public void ScheduleImport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ImportJobId, () => ImportAsync(), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ImportJobId}";
                var exception = new FredBusinessException(msg);
                NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        public async Task ImportAsync()
        {

            var sphinxEtl = new SphinxEtlProcess();
            sphinxEtl.Build();
            await sphinxEtl.ExecuteAsync();
        }
    }
}
