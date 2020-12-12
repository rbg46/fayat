using System.Collections.Generic;

namespace Fred.Entities.Budget
{
  /// <summary>
  /// Comparateur de RessourceTacheEnt
  /// </summary>
  public class RessourceTacheComparer : IEqualityComparer<RessourceTacheEnt>
  {
    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="x">RessourceTacheEnt de gauche</param>
    /// <param name="y">RessourceTacheEnt de droite</param>
    /// <returns>true si egal</returns>
    public bool Equals(RessourceTacheEnt x, RessourceTacheEnt y)
    {
      return x.RessourceTacheId == y.RessourceTacheId;
    }
    /// <summary>
    /// GetHashCode
    /// </summary>
    /// <param name="obj">RessourceTacheEnt dont on vaut le hash code</param>
    /// <returns>retourne le hash code</returns>
    public int GetHashCode(RessourceTacheEnt obj)
    {
      return obj.RessourceTacheId.GetHashCode();
    }
  }
}
