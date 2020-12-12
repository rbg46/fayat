using Fred.Entities.Import;

namespace Fred.Business.Import
{
  /// <summary>
  ///  Définit du gestionnaire des systèmes d'import.
  /// </summary>
  public interface ISystemeImportManager : IManager<SystemeImportEnt>
  {
    /// <summary>
    /// Permet de récupérer un système d'import.
    /// </summary>
    /// <param name="code">Le code.</param>
    /// <returns>un système d'import.</returns>
    SystemeImportEnt GetSystemeImport(string code);
  }
}
