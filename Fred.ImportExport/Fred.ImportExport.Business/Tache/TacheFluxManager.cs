using System.Threading.Tasks;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Process;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Models.Tache;

namespace Fred.ImportExport.Business.Tache
{
    public class TacheFluxManager : AbstractFluxManager
    {
        public TacheFluxManager(IFluxManager fluxManager)
            : base(fluxManager)
        {
        }

        public async Task ImportTacheAsync(TacheModel tache)
        {
            var importMaterielProcess = new ImportTacheProcess();

            importMaterielProcess.Init(tache);
            importMaterielProcess.Build();
            await importMaterielProcess.ExecuteAsync();
        }
    }
}