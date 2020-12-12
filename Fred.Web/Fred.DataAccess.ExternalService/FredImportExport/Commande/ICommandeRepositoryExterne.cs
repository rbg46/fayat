using System.Threading.Tasks;
using Fred.Entities.Models;

namespace Fred.DataAccess.ExternalService.FredImportExport.Commande
{
    public interface ICommandeRepositoryExterne : IExternalRepository
    {
        Task<ResultModel<string>> ExportCommandeToSapAsync(int commandeId);

        Task<ResultModel<string>> ExportCommandeAvenantToSapAsync(int commandeId, int numeroAvenant);
    }
}
