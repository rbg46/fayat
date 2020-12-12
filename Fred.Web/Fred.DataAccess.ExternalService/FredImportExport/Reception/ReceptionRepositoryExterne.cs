using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Services;

namespace Fred.DataAccess.ExternalService.FredImportExport.Reception
{
    /// <summary>
    /// Repository des services pour les commandes
    /// </summary>
    public class ReceptionRepositoryExterne : BaseExternalRepositoy, IReceptionRepositoryExterne
    {
        public ReceptionRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        ///   Envoi des réceptions à SAP
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions à envoyer à SAP</param>
        /// <returns>True si l'envoie c'est bien passé sinon False.</returns>
        public async Task<List<ResultModel<DepenseFluxResponseModel>>> ExportReceptionListToSapAsync(IEnumerable<int> receptionIds)
        {
            try
            {
                string requestUri = $"{BaseUrl}/{WebApiEndPoints.Reception_Post_ExportReceptionListToSap}";
                return await RestClient.PostAndEnsureSuccessAsync<List<ResultModel<DepenseFluxResponseModel>>>(requestUri, receptionIds);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
