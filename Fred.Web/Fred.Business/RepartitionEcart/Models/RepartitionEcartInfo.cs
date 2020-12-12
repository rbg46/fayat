using System.Collections.Generic;

namespace Fred.Business.RepartitionEcart.Models
{
  /// <summary>
  /// RepartitionEcartInfo
  /// </summary>
  public class RepartitionEcartInfo
  {
    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// CodeTacheParDefault
    /// </summary>
    public string CodeTacheParDefault { get; set; }

    /// <summary>
    /// CodeRessourceEcart
    /// </summary>
    public string CodeRessourceEcart { get; set; }


    /// <summary>
    /// ChapitresCodes
    /// </summary>
    public List<string> ChapitresCodes { get; set; }

    /// <summary>
    /// Le libelle de la repartition
    /// </summary>
    public string RepartitionLibelle { get; internal set; }

    /// <summary>
    /// L'idex
    /// </summary>
    public int RowIndex { get; internal set; }

    /// <summary>
    /// Le libelle de l'od d'ecart
    /// </summary>
    public string OdLibelle { get; internal set; }

    /// <summary>
    /// L'identifiant de famille d'OD
    /// </summary>
    public string CodeOdFamilly { get; internal set; }
  }
}
