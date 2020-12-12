using Fred.Business;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context
{
  /// <summary>
  /// Service qui fournit les données necessaires a l'import des cis
  /// </summary>
  public interface IImportFournisseurByFournisseurListAnaelSystemContextProvider : IService
  {
    /// <summary>
    /// Fournit les données necessaires a l'import des cis
    /// </summary>
    /// <param name="input">Données d'entrées</param>
    /// <param name="logger">logger</param>       
    /// <returns>les données necessaires a l'import des cis</returns>
    ImportFournisseurContext<ImportFournisseurByIdsListInputs> GetContext(ImportFournisseurByIdsListInputs input, FournisseurImportExportLogger logger);
  }
}
