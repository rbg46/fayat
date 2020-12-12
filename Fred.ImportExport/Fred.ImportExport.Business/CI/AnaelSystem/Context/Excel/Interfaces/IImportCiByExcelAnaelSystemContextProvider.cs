using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'un fichier excel
    /// </summary>
    public interface IImportCiByExcelAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">LOGGER</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        ImportCiContext<ImportCisByExcelInputs> GetContext(ImportCisByExcelInputs input, CiImportExportLogger logger);
    }
}
