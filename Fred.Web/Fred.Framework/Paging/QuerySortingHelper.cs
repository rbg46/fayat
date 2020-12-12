using System.Linq;
using System.Linq.Dynamic;

namespace Fred.Framework
{
  /// <summary>
  ///   La classe QuerySortingHelper
  /// </summary>
  public static class QuerySortingHelper
  {
    /// <summary>
    ///   Insère les informations de tri du contexte dans une requête.
    /// </summary>
    /// <typeparam name="T">Type de l'entité référence.</typeparam>
    /// <param name="query">Requête à mettre à jour.</param>
    /// <returns>La requête mise à jour.</returns>
    public static IQueryable<T> SetSortingInfo<T>(IQueryable<T> query) where T : class, new()
    {
      if (SortingContext.Sorts != null)
      {
        query = query.OrderBy(SortingContext.Sorts.Field + " " + SortingContext.Sorts.Dir);
      }

      return query;
    }
  }
}