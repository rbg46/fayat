using System;
using System.Threading.Tasks;

namespace Fred.Business.ExternalService.CommandeLigne
{
    public interface ICommandeLigneManagerExterne
    {
        Task ExportManualLockLigneDeCommandeToSap(int commandeId, Func<Task> onLockOnFredWeb);

        Task ExportManualUnlockLigneDeCommandeToSap(int commandeId, Func<Task> onUnLockOnFredWeb);
    }
}
