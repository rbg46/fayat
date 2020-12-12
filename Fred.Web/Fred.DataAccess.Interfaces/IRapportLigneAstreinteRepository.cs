
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Définition d'une ligne de rapport sur une astreinte
  /// </summary>
  public interface IRapportLigneAstreinteRepository : IRepository<RapportLigneAstreinteEnt>
  {
    /// <summary>
    /// Find rapport ligne prime by rapport ligne id and astreinte id
    /// </summary>
    /// <param name="rapportLigneId">Rapport ligne identifier</param>
    /// <param name="astreinteId">astreinte identifier</param>
    /// <returns>Rapport ligne astreinte</returns>
    RapportLigneAstreinteEnt FindAstreinte(int rapportLigneId, int astreinteId);

    /// <summary>
    /// Delete Astreinte by Id
    /// </summary>
    /// <param name="astreinteId">Identifier of astreinte</param>
    void DeleteAstreintesById(int astreinteId);
  }
}
