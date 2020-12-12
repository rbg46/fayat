using System.Collections.Generic;

namespace Fred.Web.Models
{
  /// <summary>
  ///   Model axe d'exploration
  /// </summary>
  public class ExplorateurAxeModel
  {
    /// <summary>
    ///   Obtient ou définit le code de l'axe
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé de l'axe
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit le Montant HT
    /// </summary>
    public decimal MontantHT { get; set; }

    /// <summary>
    ///   Obtient ou définit le type de l'axe
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///   Obtient ou définit le sous axe d'exploration
    /// </summary>
    public IEnumerable<ExplorateurAxeModel> SousExplorateurAxe { get; set; }
  }
}
