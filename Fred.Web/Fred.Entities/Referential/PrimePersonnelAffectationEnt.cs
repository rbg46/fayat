using System.Collections.Generic;

namespace Fred.Entities.Referential
{
  /// <summary>
  /// personnel prime
  /// </summary>
  public class PrimePersonnelAffectationEnt
  {
    /// <summary>
    /// Personnel identifier
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    /// List prime affected
    /// </summary>
    public List<PrimeAffectationEnt> PrimeList { get; set; }
  }
}
