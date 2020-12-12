using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.CI.AnaelSystem
{
    /// <summary>
    /// Gere l'import des CIs
    /// </summary>
    public interface IImportCiAnaelSystemManager : IService
    {
        /// <summary>
        /// Importation des cis par societe
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportCiBySocieteAsync(ImportCisBySocieteInputs input);

        /// <summary>
        /// Importation des cis par liste de cis
        /// </summary>
        /// <param name="importCisByCiListInputs">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportCiByCiIdsAsync(ImportCisByCiListInputs importCisByCiListInputs);

        /// <summary>
        /// Importation des cis par fichier excel
        /// </summary>
        /// <param name="importCisByExcelInput">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportCiByExcelAsync(ImportCisByExcelInputs importCisByExcelInput);
    }
}
