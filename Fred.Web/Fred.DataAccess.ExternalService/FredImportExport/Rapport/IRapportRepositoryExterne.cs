using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Rapport
{
    public interface IRapportRepositoryExterne : IExternalRepository
    {
        Task ExportPointagePersonnelToSapAsync(int rapportId);

        Task ExportPointagePersonnelToSapAsync(List<int> rapportIds);

        Task ExportPointagePersonnelToTibcoAsync(ExportPointagePersonnelFilterModel filter);
    }
}