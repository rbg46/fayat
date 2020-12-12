using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential
{
  public class SearchMaterielModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le Code
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le Libelle
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur Adresse
    /// </summary>

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur Actif
    /// </summary>
    public bool Actif { get; set; }
  }
}