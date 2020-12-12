namespace Fred.Entities.Models
{
  /// <summary>
  /// Model pour la classe Result<typeparamref name="T"/>
  /// </summary>
  /// <typeparam name="T">Type de resultat</typeparam>
  public class ResultModel<T>
  {

    /// <summary>
    /// Indique le success.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Retourne l'erreur.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// valeur du resultat ok.
    /// </summary>
    public T Value { get; set; }
  }
}
