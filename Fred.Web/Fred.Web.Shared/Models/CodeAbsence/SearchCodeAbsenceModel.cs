using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.CodeAbsence
{
  /// <summary>
  /// Représente une recherche de société
  /// </summary>
  public class SearchCodeAbsenceModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code condensé
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le libellé de la société
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si une société est active ou non.
    /// </summary>
    public bool Active { get; set; }
  }
}
