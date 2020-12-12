using System;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.DataAccess.ExternalService.FredImportExport.Moyen
{
    public interface IMoyenRepositoryExterne : IExternalRepository
    {
        Task<EnvoiPointageMoyenResultModel> ExportPointageMoyenAsync(DateTime startDate, DateTime endDate);
    }
}