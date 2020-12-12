using Fred.Web.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential
{
  public class SearchNatureModel : AbstractSearchModel
  {
    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public override string ValueText { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur un code de code déplacement.
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur libellé de code déplacement.
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un code déplacement actif ou non.
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la société sur laquelle on recherche les natures
    /// </summary>
    public int? SocieteId { get; set; }
  }
}