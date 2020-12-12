using System.Threading.Tasks;
using Fred.Web.Shared.Models.Commande;

namespace Fred.DataAccess.ExternalService.FredImportExport.CommandeLigne
{
    public interface ICommandeLigneRepositoryExterne
    {
        Task ExportManualUnLockLigneDeCommandeToSapAsync(ExportManualLockUnlockLigneDeCommandeToSapModel exportManualLockUnlockLigneDeCommandeToSapModel);
        Task ExportManualLockLigneDeCommandeToSapAsync(ExportManualLockUnlockLigneDeCommandeToSapModel exportManualLockUnlockLigneDeCommandeToSapModel);
    }
}
