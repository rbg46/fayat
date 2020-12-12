using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel;
using Hangfire;
using NLog;

namespace Fred.ImportExport.Business.Materiel
{
    public abstract class AbstractMaterielImport : AbstractFluxManager
    {
        public abstract string Login { get; }
        public abstract string Password { get; }
        public abstract string WebApiStormUrl { get; }
        public abstract string ImportJobId { get; }
        public abstract List<string> CodeSocieteComptables { get; }

        protected ISocieteCodeImportMaterielManager SocieteCodeImportMaterielManager { get; set; }

        protected AbstractMaterielImport(IFluxManager fluxManager, ISocieteCodeImportMaterielManager societeCodeImportMaterielManager)
            : base(fluxManager)
        {
            SocieteCodeImportMaterielManager = societeCodeImportMaterielManager;
        }

        public void ExecuteImport(bool isFull)
        {
            BackgroundJob.Enqueue(() => ImportMaterielFromStormAsync(isFull));
        }

        public void ScheduleImport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ImportJobId, () => ImportMaterielFromStormAsync(false), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ImportJobId}";
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        public abstract Task ImportMaterielFromStormAsync(bool isFull);
    }
}
