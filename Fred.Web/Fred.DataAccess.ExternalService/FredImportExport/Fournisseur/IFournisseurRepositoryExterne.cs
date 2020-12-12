using System.Threading.Tasks;

namespace Fred.DataAccess.ExternalService.FredImportExport.Fournisseur
{
    public interface IFournisseurRepositoryExterne : IExternalRepository
    {
        Task<bool> ExecuteImportAsync(string codeSocieteComptable);
    }
}