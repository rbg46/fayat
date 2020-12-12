using Fred.Entities.Import;

namespace Fred.Business.Import
{
  /// <summary>
  ///  Définit du gestionnaire des systèmes externes.
  /// </summary>
  public interface ISystemeExterneManager : IManager<SystemeExterneEnt>
  {
    /// <summary>
    /// Permet de récupérer un système externe.
    /// </summary>
    /// <param name="code">Le code.</param>
    /// <returns>Le système externe.</returns>
    SystemeExterneEnt GetSystemeExterne(string code);
  }
}
