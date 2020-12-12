using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential.Light
{
  public class UniteLightModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une Unité.
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une Unité.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une Unité.
    /// </summary>
    public string Libelle { get; set; }

  }
}