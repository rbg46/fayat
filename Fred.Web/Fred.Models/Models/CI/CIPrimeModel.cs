using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Web.Models.CI
{
  /// <summary>
  /// Représente un CIPrime (association entre un CI et une prime)
  /// </summary>
  public class CIPrimeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique du CIPrime
    /// </summary>
    public int CiPrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du CI associé.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la prime associée.
    /// </summary>
    public int PrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI associé
    /// </summary>
    public virtual CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la prime associée
    /// </summary>
    public virtual PrimeModel Prime { get; set; }
  }
}
