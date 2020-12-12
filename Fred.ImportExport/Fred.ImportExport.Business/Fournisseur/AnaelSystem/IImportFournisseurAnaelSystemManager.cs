using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem
{
    /// <summary>
    /// Gere l'import des Fournisseurs
    /// </summary>
    public interface IImportFournisseurAnaelSystemManager : IService
    {
        /// <summary>
        /// Importation des fournisseurs par liste de fournisseurs
        /// </summary>
        /// <param name="importFournisseurByIdsListInputs">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportFournisseurByFournisseurIdsAsync(ImportFournisseurByIdsListInputs importFournisseurByIdsListInputs);

        /// <summary>
        /// Importation des fournisseurs par fichier excel
        /// </summary>
        /// <param name="importFournisseursByExcelInput">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportFournisseurByExcelAsync(ImportFournisseursByExcelInputs importFournisseursByExcelInput);
    }
}
