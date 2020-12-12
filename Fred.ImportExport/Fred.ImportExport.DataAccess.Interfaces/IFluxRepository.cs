using System.Linq;
using System.Threading.Tasks;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.DataAccess.Interfaces
{
    public interface IFluxRepository
    {
        IQueryable<FluxEnt> Get();

        FluxEnt GetById(int id);

        Task<string> GetCodeStartingWithBySocieteCodeAsync(string code, string societeCode);

        Task<string> GetGroupCodeByFluxCodeAsync(string fluxCode);

        void Update(FluxEnt flux);
    }
}