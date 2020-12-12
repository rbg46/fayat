using System.Collections.Generic;

namespace Fred.Entities.ValidationPointage
{
  /// <summary>
  ///   Classe représentant le résultat d'un recherche d'erreur Contrôle ou Remontée Vrac
  /// </summary>
  /// <typeparam name="T">Type d'erreurs</typeparam>
  public class SearchValidationResult<T> where T : class
  {
    /// <summary>
    ///   Liste des erreurs
    /// </summary>
    public IEnumerable<PersonnelErreur<T>> Erreurs { get; set; }

    /// <summary>
    /// Nombre total d'element sans le paging
    /// </summary>
    public int TotalPersonnelCount { get; set; }

    /// <summary>
    /// Nombre total d'erreurs pour tous les personnels (non paginés)
    /// </summary>
    public int TotalErreurCount { get; set; }
  }
}
