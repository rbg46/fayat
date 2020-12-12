using System.Collections.Generic;

namespace Fred.Framework.Services.Google
{


  /// <summary>
  ///   Définition du format de sortie du géodecodig Google
  /// </summary>
  public class GeocodeResult
  {
    /// <summary>
    ///   Obtient ou définit Résultat
    /// </summary>
    public List<Result> Results { get; set; }

    /// <summary>
    ///   Obtient ou définit le status de la réponse (ok)
    /// </summary>
    public string Status { get; set; }
  }
}
