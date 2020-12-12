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
  public class RapportLigneAndPrimeModel
  {
    /// <summary>
    /// Obtient ou définit une ligne de rapport
    /// </summary>
    public RapportLigneModel RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit une prime
    /// </summary>
    public PrimeModel Prime { get; set; }
  }
}