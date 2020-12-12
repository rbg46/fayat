using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Classe nécessaire au transit vers l'API via HTTP pour passer une donnée rapport et tache
  /// </summary>
  public class RapportAndTacheModel
  {
    /// <summary>
    /// Obtient ou définit un rapport
    /// </summary>
    public RapportModel Rapport { get; set; }

    /// <summary>
    /// Obtient ou définit une tache
    /// </summary>
    public TacheModel Tache { get; set; }
  }
}