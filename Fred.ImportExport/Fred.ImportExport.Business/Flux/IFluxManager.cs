using System.Collections.Generic;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Flux
{
    public interface IFluxManager
    {
        List<FluxEnt> GetAll();
        FluxEnt GetByCode(string codeFlux);
        bool Update(FluxEnt flux);
        void UpdateActive(int id, bool isActif);
    }
}
