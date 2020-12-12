using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.DataAccess.ExternalService.FredImportExport.Materiel
{
    public interface IMaterielRepositoryExterne : IExternalRepository
    {
        Task ExportPointageMaterielToStormAsync(int rapportId);

        Task ExportPointageMaterielToStormAsync(List<int> rapportIds);
    }
}