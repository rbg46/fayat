using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.CodeZoneDeplacement
{
  /// <summary>
  /// Représente une recherche de code zone deplacement
  /// </summary>
  public class SearchCodeZoneDeplacementModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code zone deplacement
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur l'id de la société
    /// </summary>
    public bool SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le libellé du code zone deplacement
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si un code zone delpacement est actif ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}
