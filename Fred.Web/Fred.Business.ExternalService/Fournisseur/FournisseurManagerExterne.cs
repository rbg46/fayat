using System.Threading.Tasks;
using Fred.DataAccess.ExternalService.FredImportExport.Fournisseur;
using Fred.Framework.Exceptions;

namespace Fred.Business.ExternalService
{
    /// <summary>
    ///   Gestionnaire des Import fournisseurs
    /// </summary>
    public class FournisseurManagerExterne : IFournisseurManagerExterne
    {
        private readonly IFournisseurRepositoryExterne fournisseurRepository;

        public FournisseurManagerExterne(IFournisseurRepositoryExterne fournisseurRepository)
        {
            this.fournisseurRepository = fournisseurRepository;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteImport(string codeSocieteComptable)
        {
            try
            {
                // to do : Gestion des droits
                return await fournisseurRepository.ExecuteImportAsync(codeSocieteComptable);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }
    }
}
