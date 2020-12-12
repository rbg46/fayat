using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'une liste de ciId
    /// </summary>
    public class ImportFournisseurByFournisseurListAnaelSystemContextProvider : IImportFournisseurByFournisseurListAnaelSystemContextProvider
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">LOGGER</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportFournisseurContext<ImportFournisseurByIdsListInputs> GetContext(ImportFournisseurByIdsListInputs input, FournisseurImportExportLogger logger)
        {
            var result = new ImportFournisseurContext<ImportFournisseurByIdsListInputs>();

            //Implementer le contexte pour l'import par societe
            result.Input = input;

            return result;

        }
    }
}
