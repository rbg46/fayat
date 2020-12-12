
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  /// Rapport line majoration repository interface
  /// </summary>
  public interface IRapportLigneMajorationRepository : IRepository<RapportLigneMajorationEnt>
  {

    /// <summary>
    /// Find rapport majoration by rapport ligne id and majoration id
    /// </summary>
    /// <param name="rapportLigneId">Rapport ligne identifier</param>
    /// <param name="codeMajorationId">Code majoration identifier</param>
    /// <returns>Rapport ligne majoration</returns>
    RapportLigneMajorationEnt FindMajorationByMajorationId(int rapportLigneId, int codeMajorationId);
  }
}
