using System.Collections.Generic;

namespace Fred.ImportExport.Business.ValidationPointage
{
  /// <summary>
  /// Représente les pointages chatier et les primes de RVG.
  /// </summary>
  public class RvgPointagesAndPrimes
  {
    /// <summary>
    /// Obtient les pointages chantier.
    /// </summary>
    public List<RvgPointage> Pointages { get; private set; } = new List<RvgPointage>();

    /// <summary>
    /// Obtient les primes.
    /// </summary>
    public List<RvgPrime> Primes { get; private set; } = new List<RvgPrime>();
  }
}
