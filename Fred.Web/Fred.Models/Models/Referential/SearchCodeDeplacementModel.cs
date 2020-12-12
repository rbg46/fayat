using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente une recherche de société
  /// </summary>
  public class SearchCodeDeplacementModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur un code de code déplacement.
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur libellé de code déplacement.
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le kilométrage minimum d'un code déplacement.
    /// </summary>
    public bool KmMini { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le kilométrage maximum d'un code déplacement.
    /// </summary>
    public bool KmMaxi { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un code déplacement en IVD automatique ou non.
    /// </summary>
    public bool IVDAuto { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un code déplacement en IGD ou non.
    /// </summary>
    public bool IGD { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un code déplacement soumis à indemnité forfaitaire ou non.
    /// </summary>
    public bool IndemniteForfaitaire { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un code déplacement actif ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}
