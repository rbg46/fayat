using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.CI
{
  public class CITypeParticipationModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique du CI associé.
    /// </summary>
    public int TypeParticipationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la prime associée.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le code unique de la prime associée.
    /// </summary>
    public string Code { get; set; }
  }
}