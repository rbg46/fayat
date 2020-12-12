using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Personnel
{
    /// <summary>
    /// Repository externe pour les personnels
    /// </summary>
    public class PersonnelRepositoryExterne : BaseExternalRepositoy, IPersonnelRepositoryExterne
    {
        public PersonnelRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Demande la mise a jour des personnels
        /// </summary>
        /// <param name="personnelIds">Liste de personnels a mettre a jours</param>
        public async Task UpdatePersonnelsAsync(List<int> personnelIds)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.Personnel_UpdatePersonnels, BaseUrl);
                await RestClient.PostAndEnsureSuccessAsync(requestUri, personnelIds);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        /// <summary>
        /// Permet de demander à hangfire un job sur l'export des réceptions intérimaires vers sap
        /// </summary>
        /// <param name="societesIds">liste des identifiants des sociétés</param>
        /// <param name="userId">Identifiant utilisateur</param>
        public async Task ExportReceptionInterimairesAsync(List<int> societesIds, int userId)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.Personnel_Post_ExportReceptionInterimairesToSap, BaseUrl, userId);
                await RestClient.PostAndEnsureSuccessAsync(requestUri, societesIds);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        /// <summary>
        /// Permet de demander à hangfire un job sur l'export des réceptions intérimaires vers sap avec list cis
        /// </summary>
        /// <param name="ciIds">liste des identifiants des cis</param>
        /// <param name="userId">Identifiant utilisateur</param>
        public async Task ExportReceptionInterimairesByCisAsync(List<int> ciIds, int userId)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.Personnel_Post_ExportReceptionInterimairesByCisToSap, BaseUrl, userId);
                await RestClient.PostAndEnsureSuccessAsync(requestUri, ciIds);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
