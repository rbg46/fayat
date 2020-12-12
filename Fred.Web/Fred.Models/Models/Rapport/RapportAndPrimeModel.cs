using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Classe nécessaire au transit vers l'API via HTTP pour passer une donnée rapport et prime
  /// </summary>
  public class RapportAndPrimeModel
  {
    /// <summary>
    /// Obtient ou définit un rapport
    /// </summary>
    public RapportModel Rapport { get; set; }

    /// <summary>
    /// Obtient ou définit une prime
    /// </summary>
    public PrimeModel Prime { get; set; }
  }
}