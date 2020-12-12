using Fred.Web.Models;

namespace Fred.Web.Shared.Models
{
  /// <summary>
  ///   Représente les deux axes de recherches
  /// </summary>
  public class AxeModel
  {
    /// <summary>
    ///   Obtient ou définit l'axe 1 de recherche
    /// </summary>
    public ExplorateurAxeModel Axe1 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'axe 2 de recherche
    /// </summary>
    public ExplorateurAxeModel Axe2 { get; set; }
  }
}
