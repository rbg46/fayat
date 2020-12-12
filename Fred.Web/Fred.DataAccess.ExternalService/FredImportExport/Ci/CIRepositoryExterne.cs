using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Ci
{
    /// <summary>
    /// Repository externe pour les cis
    /// </summary>
    public class CIRepositoryExterne : BaseExternalRepositoy, ICIRepositoryExterne
    {
        public CIRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Demande la mise a jour des cis
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="ciIds">Liste de cis a mettre a jours</param>
        public async Task UpdateCisAsync(int utilisateurId, List<int> ciIds)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.CI_UpdateCis, BaseUrl);
                await RestClient.PostAndEnsureSuccessAsync(requestUri, ciIds);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
