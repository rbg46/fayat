using System.Collections.Generic;

namespace Fred.Entities.Fonctionnalite
{
  /// <summary>
  /// Comparer pour FonctionnaliteEnt
  /// </summary>
  public class FonctionnaliteComparer : IEqualityComparer<FonctionnaliteEnt>
  {

    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="x">FonctionnaliteEnt de gauche</param>
    /// <param name="y">FonctionnaliteEnt de droite</param>
    /// <returns>true si egal</returns>
    public bool Equals(FonctionnaliteEnt x, FonctionnaliteEnt y)
    {
      return x.FonctionnaliteId == y.FonctionnaliteId && x.Mode == y.Mode;
    }

    /// <summary>
    /// GetHashCode
    /// </summary>
    /// <param name="obj">FonctionnaliteEnt dont on vaut le hash code</param>
    /// <returns>retourne le hash code</returns>
    public int GetHashCode(FonctionnaliteEnt obj)
    {
      return obj.FonctionnaliteId.GetHashCode();
    }
  }
}
