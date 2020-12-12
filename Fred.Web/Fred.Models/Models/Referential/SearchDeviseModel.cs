using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente une recherche de devise
  /// </summary>
  public class SearchDeviseModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche une devise par son IsoCode
    /// </summary>
    public bool IsoCode { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche une devise par son IsoNombre
    /// </summary>
    public bool IsoNombre { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche une devise par son symbole
    /// </summary>
    public bool Symbole { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche une devise par son libellé
    /// </summary>
    public bool Libelle { get; set; }
  }
}