using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Classe nécessaire au transit vers l'API via HTTP pour passer une donnée rapportLigne et periode de duplication
  /// </summary>
  public class RapportLigneToDuplicateModel
  {
    /// <summary>
    /// Obtient ou définit une ligne de rapport
    /// </summary>
    public RapportLigneModel RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit la date de début de duplication
    /// </summary>
    public DateTime DateDebut { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fin de duplication
    /// </summary>
    public DateTime DateFin { get; set; }
  }
}