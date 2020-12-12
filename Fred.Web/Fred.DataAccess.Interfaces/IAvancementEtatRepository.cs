
using Fred.Entities.Budget.Avancement;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Référentiel de données pour les workflows d'avancement.
  /// </summary>
  public interface IAvancementEtatRepository : IRepository<AvancementEtatEnt>
  {
    /// <summary>
    /// Retourne l'état d'un avancement
    /// </summary>
    /// <param name="code">Code de l'état</param>
    /// <returns>L'état d'un avancement</returns>
    AvancementEtatEnt GetByCode(string code);
  }
}
