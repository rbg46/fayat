using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis
    /// </summary>
    public interface IImportCiByCiListAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">logger</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        ImportCiContext<ImportCisByCiListInputs> GetContext(ImportCisByCiListInputs input, CiImportExportLogger logger);
    }
}
