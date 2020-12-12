using Fred.Web.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Web.Models.Role
{
  /// <summary>
  /// Représente un RoleModule (association entre un rôle et un module)
  /// </summary>
  public class RoleModuleModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un groupe.
    /// </summary>
    [Required]
    public int ModuleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un groupe.
    /// </summary>
    [Required]
    public int RoleId { get; set; }

    //public string ModuleLabel { get; set; }

    public virtual ModuleModel Module { get; set; }
    public virtual RoleModel Role { get; set; }
  }
}
