using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.DataAccess.Flux
{
    public class FluxRepository : AbstractRepository<FluxEnt>, IFluxRepository
    {
        private readonly ImportExportContext context;

        public FluxRepository(ImportExportContext context)
          : base(context)
        {
            this.context = context;
        }

        public async Task<string> GetGroupCodeByFluxCodeAsync(string fluxCode)
        {
            return await context.Flux.Where(t => t.Code == fluxCode).Select(t => t.GroupCode).SingleOrDefaultAsync();
        }

        public async Task<string> GetCodeStartingWithBySocieteCodeAsync(string code, string societeCode)
        {
            return await context.Flux
                .Where(t => t.Code.ToLower().StartsWith(code.ToLower()) && t.SocieteCode == societeCode)
                .Select(t => t.Code)
                .SingleOrDefaultAsync();
        }
    }
}
