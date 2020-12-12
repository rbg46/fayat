using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Depense
{
  /// <summary>
  /// Représente le statut d'une dépense.
  /// </summary>
  public class DepenseStatutRapprochementModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique du statut d'une dépense.
    /// </summary>
    public int DepenseStatutRapprochementId { get; set; }

    /// <summary>
    /// Obtient ou définit le libelle du statut de la dépense.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'ordre du statut d'une dépense.
    /// </summary>    
    public byte Ordre { get; set; }
  }
}