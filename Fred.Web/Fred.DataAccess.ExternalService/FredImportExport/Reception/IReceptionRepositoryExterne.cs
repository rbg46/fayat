using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;

namespace Fred.DataAccess.ExternalService.FredImportExport.Reception
{
    public interface IReceptionRepositoryExterne : IExternalRepository
    {
        Task<List<ResultModel<DepenseFluxResponseModel>>> ExportReceptionListToSapAsync(IEnumerable<int> receptionIds);
    }
}