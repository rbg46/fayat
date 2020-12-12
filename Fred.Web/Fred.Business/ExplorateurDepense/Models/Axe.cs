using System;

namespace Fred.Business.ExplorateurDepense
{
  /// <summary>
  ///   Représente les deux axes de recherches
  /// </summary>
  [Serializable]
  public class Axe
  {
    /// <summary>
    ///   Obtient ou définit l'axe 1 de recherche
    /// </summary>
    public ExplorateurAxe Axe1 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'axe 2 de recherche
    /// </summary>
    public ExplorateurAxe Axe2 { get; set; }
  }
}
