using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Web.Models.CI
{
  /// <summary>
  /// Représente un RoleModule (association entre un rôle et un module)
  /// </summary>
  public class CICodeMajorationModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un groupe.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un groupe.
    /// </summary>
    public int CodeMajorationId { get; set; }

    /// <summary>
    /// Obtient ou définit le module associé
    /// </summary>
    public virtual CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit le rôle associé
    /// </summary>
    public virtual CodeMajorationModel CodeMajoration { get; set; }
  }
}
