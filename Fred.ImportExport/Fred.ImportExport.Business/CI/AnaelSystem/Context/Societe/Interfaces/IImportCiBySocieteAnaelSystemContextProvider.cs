using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    ///  Service qui fournit les données necessaires a l'import des cis a partir d'une societe
    /// </summary>
    public interface IImportCiBySocieteAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">logger</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        ImportCiContext<ImportCisBySocieteInputs> GetContext(ImportCisBySocieteInputs input, CiImportExportLogger logger);

    }
}
