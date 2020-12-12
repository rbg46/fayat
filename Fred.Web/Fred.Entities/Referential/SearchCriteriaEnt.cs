using Fred.Entities.Search;
using System;
using System.Linq.Expressions;

namespace Fred.Entities.Referential
{
  /// <summary>
  /// Critères de recherche
  /// </summary>
  /// <typeparam name="T">Type de l'objet recherché</typeparam>
  public class SearchCriteriaEnt<T> : AbstractSearch
    where T : ISearchableEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le code
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libellé
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si l'objet est actif ou non.
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la société sur laquelle on recherche
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    ///   Permet de récupérer le prédicat de recherche de l'objet.
    /// </summary>
    /// <returns>Retourne la condition de recherche de l'objet</returns>
    public Expression<Func<T, bool>> GetPredicateWhere()
    {
      if (string.IsNullOrEmpty(ValueText))
      {
        return p => !SocieteId.HasValue || p.SocieteId == SocieteId.Value;
      }

      return p => (SocieteId == null || p.SocieteId == SocieteId)
                  && (p.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                    || p.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0);
    }
  }
}