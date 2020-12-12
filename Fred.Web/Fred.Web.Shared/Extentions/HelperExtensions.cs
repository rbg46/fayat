using System.Collections.Generic;
using System.Linq;

namespace Fred.Web.Shared.Extentions
{
  /// <summary>
  /// static class to contain helper static methods
  /// </summary>
  public static class HelperExtensions
  {
    /// <summary>
    /// Is null or empty pour les enumerable 
    /// </summary>
    /// <typeparam name="T">Type generic</typeparam>
    /// <param name="enumerable">IEnumerable de T</param>
    /// <returns>True si la liste is null ou vide .</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable == null)
      {
        return true;
      }

      // Si notre enumerable est une liste par exemple l'utilisation du count est plus performant .
      ICollection<T> collection = enumerable as ICollection<T>;
      if (collection != null)
      {
        return collection.Count == 0;
      }
      return !enumerable.Any();
    }
  }
}
