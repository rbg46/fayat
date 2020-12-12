using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Context
{
    /// <summary>
    /// Service qui recupere les données necessaires a l'import
    /// </summary>
    public interface IImportCiByApiContextProvider : IFredIEService
    {
        /// <summary>
        /// Recupere les données necessaires a l'import
        /// </summary>
        /// <param name="input">Les entrants</param>
        /// <returns>Les données necessaires a l'import</returns>
        ImportCiByWebApiContext GetContext(ImportCisByApiInputs input);
    }
}
