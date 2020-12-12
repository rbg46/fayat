using Fred.ImportExport.Business.Hangfire.Parameters;

namespace Fred.ImportExport.Business.CommandeLigne.VME22
{
    public interface IVme22FluxManager
    {
        void EnqueueExportManualLockUnlockLigneDeCommande(ExportManualLockUnlockLigneDeCommandeParameters parameters);
    }
}
