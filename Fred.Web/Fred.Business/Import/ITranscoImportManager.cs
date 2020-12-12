using Fred.Entities.Import;

namespace Fred.Business.Import
{
  /// <summary>
  ///  Définit du gestionnaire des transco d'import.
  /// </summary>
  public interface ITranscoImportManager : IManager<TranscoImportEnt>
  {
    /// <summary>
    /// Permet de récupérer une transco d'import.
    /// </summary>
    /// <param name="codeExterne">Le code externe.</param>
    /// <param name="societeId">L'identifiant de la société.</param>
    /// <param name="systemeImportId">L'identifiant du système d'import.</param>
    /// <returns>Une transco d'import.</returns>
    TranscoImportEnt GetTranscoImport(string codeExterne, int societeId, int systemeImportId);
  }
}
