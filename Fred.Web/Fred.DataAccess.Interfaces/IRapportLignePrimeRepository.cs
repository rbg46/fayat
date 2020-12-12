
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Définition d'une ligne de rapport sur une prime
  /// </summary>
  public interface IRapportLignePrimeRepository : IRepository<RapportLignePrimeEnt>
  {

    /// <summary>
    /// Find rapport ligne prime by rapport ligne id and prime id
    /// </summary>
    /// <param name="rapportLigneId">Rapport ligne identifier</param>
    /// <param name="primeId">Prime identifier</param>
    /// <returns>Rapport ligne prime</returns>
    RapportLignePrimeEnt FindPrime(int rapportLigneId, int primeId);
  }
}