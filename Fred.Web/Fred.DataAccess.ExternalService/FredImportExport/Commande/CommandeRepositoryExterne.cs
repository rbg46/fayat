using System.Threading.Tasks;
using Fred.Entities.Models;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Commande
{
    /// <summary>
    /// Repository des services pour les commandes
    /// </summary>
    public class CommandeRepositoryExterne : BaseExternalRepositoy, ICommandeRepositoryExterne
    {
        public CommandeRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        public async Task<ResultModel<string>> ExportCommandeToSapAsync(int commandeId)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Commande_Get_ExportCommandeToSap}/{commandeId}";
                return await RestClient.GetAsync<ResultModel<string>>(requestUri);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        public async Task<ResultModel<string>> ExportCommandeAvenantToSapAsync(int commandeId, int numeroAvenant)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Commande_Get_ExportCommandeAvenantToSap}/{commandeId}/{numeroAvenant}";

                return await RestClient.GetAsync<ResultModel<string>>(requestUri);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
