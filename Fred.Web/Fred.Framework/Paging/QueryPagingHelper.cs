using System.Linq;

namespace Fred.Framework
{
  /// <summary>
  ///   La classe QueryPagingHelper
  /// </summary>
  public static class QueryPagingHelper
  {
    /// <summary>
    ///   Execute le filtrage par page d'une requête suivant les informations de pagination renseignées.
    /// </summary>
    /// <typeparam name="T">Type de l'entité référence.</typeparam>
    /// <param name="query">Requête à mettre à jour.</param>
    /// <returns>La requête mise à jour.</returns>
    public static IQueryable<T> ApplyPaging<T>(IQueryable<T> query) where T : class, new()
    {
      int skip = 0;

      if (!PagingContext.Page.HasValue || PagingContext.Page <= 0)
      {
        return query;
      }

      if (!PagingContext.Take.HasValue || PagingContext.Take <= 0)
      {
        return query;
      }

      skip = (PagingContext.Page.Value - 1) * PagingContext.Take.Value;

      if (skip != 0)
      {
        query = query.Skip(skip);
      }

      if (PagingContext.Take.HasValue)
      {
        query = query.Take(PagingContext.Take.Value);
      }

      return query;
    }

    /// <summary>
    ///   Execute le filtrage par défilement d'une requête suivant les informations de pagination renseignées.
    /// </summary>
    /// <typeparam name="T">Type de l'entité référence.</typeparam>
    /// <param name="query">Requête à mettre à jour.</param>
    /// <returns>La requête mise à jour.</returns>
    public static IQueryable<T> ApplyScrollPaging<T>(IQueryable<T> query) where T : class, new()
    {
      int take = 0;

      if (!PagingContext.Page.HasValue || PagingContext.Page <= 0)
      {
        return query;
      }

      if (!PagingContext.Take.HasValue || PagingContext.Take <= 0)
      {
        return query;
      }

      take = PagingContext.Page.Value * PagingContext.Take.Value;

      if (PagingContext.Take.HasValue)
      {
        query = query.Take(take);
      }

      return query;
    }
  }
}