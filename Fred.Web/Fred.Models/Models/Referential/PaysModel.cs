using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Web.Models.Referential;

namespace Fred.Web.Models
{
  public class PaysModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit Identifiant de Pays.
    /// </summary>
    public int PaysId { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO de Pays.
    /// </summary>
    public string Iso2 { get; set; }

    /// <summary>
    /// Obtient ou définit Libellé de Pays.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Id du référentiel matétriel
    /// </summary>

    public string IdRef => this.PaysId.ToString();

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string LibelleRef => this.Label;

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string CodeRef => this.Iso2;
  }
}