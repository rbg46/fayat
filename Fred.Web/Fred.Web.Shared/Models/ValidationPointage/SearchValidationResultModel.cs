using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{
  /// <summary>
  ///   Classe représentant le résultat d'un recherche d'erreur Contrôle ou Remontée Vrac
  /// </summary>
  /// <typeparam name="T">Type d'erreurs</typeparam>
  public class SearchValidationResultModel<T> where T : class
  {
    /// <summary>
    ///   Liste des erreurs
    /// </summary>
    public IEnumerable<PersonnelErreurModel<T>> Erreurs { get; internal set; }

    /// <summary>
    /// Nombre total d'element sans le paging
    /// </summary>
    public int TotalPersonnelCount { get; internal set; }

    /// <summary>
    /// Nombre total d'erreurs pour tous les personnels (non paginés)
    /// </summary>
    public int TotalErreurCount { get; set; }

  }
}
