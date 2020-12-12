using Fred.Web.Shared.Models.Moyen;
using System.Collections.Generic;

namespace Fred.Web.Shared.Comparer
{
  /// <summary>
  /// Classe pour comparer deux objets de types MoyenImmatriculationModel
  /// </summary>
  public class MoyenImmatriculationComparer : IEqualityComparer<MoyenImmatriculationModel>
  {
    /// <summary>
    /// Vérifie l'égalité entre deux objets MoyenImmatriculationModel
    /// </summary>
    /// <param name="x">Premier objet MoyenImmatriculationModel</param>
    /// <param name="y">Deuxiéme objet MoyenImmatriculationModel</param>
    /// <returns></returns>
    public bool Equals(MoyenImmatriculationModel x, MoyenImmatriculationModel y)
    {
      if (x == null || y == null)
      {
        return false;
      }

      return x.Immatriculation == y.Immatriculation;
    }

    /// <summary>
    /// Get hash code
    /// </summary>
    /// <param name="obj">l'objet courant</param>
    /// <returns>Hashcode de l'objet</returns>
    public int GetHashCode(MoyenImmatriculationModel obj)
    {
      if (obj == null) return 0;

      if (obj.Immatriculation != null)
      {
        return obj.Immatriculation.GetHashCode();
      }

      return 0;
    }
  }
}
