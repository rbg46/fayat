using Fred.Business;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'un fichier excel
    /// </summary>
    public interface IImportFournisseurByExcelAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">LOGGER</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        ImportFournisseurContext<ImportFournisseursByExcelInputs> GetContext(ImportFournisseursByExcelInputs input, FournisseurImportExportLogger logger);
    }
}
