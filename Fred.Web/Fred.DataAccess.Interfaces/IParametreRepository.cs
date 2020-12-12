
using Fred.Entities;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les parametre.
  /// </summary>
  public interface IParametreRepository : IRepository<ParametreEnt>
  {
    /// <summary>
    /// Retourne un paramètre.
    /// </summary>
    /// <param name="parametreId">Identifiant du paramètre.</param>
    /// <param name="groupeId">Le groupe si le paramètre en dépend, sinon null.</param>
    /// <returns>le paramètre ou null s'il n'existe pas.</returns>
    ParametreEnt Get(ParametreId parametreId, int? groupeId = null);
  }
}
