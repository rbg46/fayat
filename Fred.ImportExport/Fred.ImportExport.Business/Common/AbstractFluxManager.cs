using System.Text;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Common
{
    public abstract class AbstractFluxManager
    {
        protected const int ConcurrentExecutionTimeoutInSeconds = 30 * 60;

        protected IFluxManager FluxManager { get; }
        protected FluxEnt Flux { get; set; }

        protected AbstractFluxManager(IFluxManager fluxManager)
        {
            FluxManager = fluxManager;
            Flux = new FluxEnt();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"    - Code : {Flux.Code}");
            builder.AppendLine($"    - Titre : {Flux.Titre}");
            builder.AppendLine($"    - Description : {Flux.Description}");
            builder.AppendLine($"    - Société : {Flux.SocieteCode}");
            builder.AppendLine($"    - Date de dernière exécution : {(Flux.DateDerniereExecution.HasValue ? Flux.DateDerniereExecution.Value.ToString("dd/MM/yyyy HH:mm:ss") : "Aucune")}");

            return builder.ToString();
        }
    }
}
