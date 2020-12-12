using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Web.Shared.Models.Commande;

namespace Fred.DataAccess.ExternalService.FredImportExport.CommandeLigne
{
    public class CommandeLigneRepositoryExterne : BaseExternalRepositoy, ICommandeLigneRepositoryExterne
    {
        public CommandeLigneRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        public async Task ExportManualLockLigneDeCommandeToSapAsync(ExportManualLockUnlockLigneDeCommandeToSapModel exportManualLockUnlockLigneDeCommandeToSapModel)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.CommandeLigne_Lock, BaseUrl);

                await RestClient.PostAndEnsureSuccessAsync(requestUri, exportManualLockUnlockLigneDeCommandeToSapModel);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        public async Task ExportManualUnLockLigneDeCommandeToSapAsync(ExportManualLockUnlockLigneDeCommandeToSapModel exportManualLockUnlockLigneDeCommandeToSapModel)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.CommandeLigne_UnLock, BaseUrl);

                await RestClient.PostAndEnsureSuccessAsync(requestUri, exportManualLockUnlockLigneDeCommandeToSapModel);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

    }
}
