using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.Fournisseur
{
    /// <summary>
    ///   Repository FournisseurRepository
    /// </summary>
    public class FournisseurRepositoryExterne : BaseExternalRepositoy, IFournisseurRepositoryExterne
    {
        public FournisseurRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        ///   Exécute l'import des fournisseurs
        /// </summary>
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <returns>Vrai si la requête a bien été lancée</returns>
        /// <exception cref="FredRepositoryException">Erreur si problème de requête vers Fred Import/Export</exception>
        public async Task<bool> ExecuteImportAsync(string codeSocieteComptable)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.Import_Get_Fournisseur, BaseUrl) + codeSocieteComptable;
                return await RestClient.GetAsync<bool>(requestUri);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
