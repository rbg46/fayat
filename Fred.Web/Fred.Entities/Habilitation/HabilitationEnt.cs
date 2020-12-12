using Fred.Entities.Permission;
using System.Collections.Generic;

namespace Fred.Entities.Habilitation
{
  /// <summary>
  /// HabilitationEnt
  /// </summary>
  public class HabilitationEnt
  {
    /// <summary>
    /// ctor
    /// </summary>
    public HabilitationEnt()
    {     
      Permissions = new List<PermissionEnt>();
    }

    /// <summary>
    /// UtilisateurId
    /// </summary>
    public int UtilisateurId { get; set; }

    /// <summary>
    /// IsSuperAdmin
    /// </summary>
    public bool IsSuperAdmin { get; set; }      

    /// <summary>
    /// Liste de persmissions globales
    /// </summary>
    public IEnumerable<PermissionEnt> Permissions { get; set; }

    /// <summary>
    /// Liste de persmissions contextuelles
    /// </summary>
    public IEnumerable<PermissionEnt> PermissionsContextuelles { get; set; }

    /// <summary>
    /// Retourne l'id de la societe rattache au personnel
    /// </summary>
    public int? SocieteId { get; set; }
  }
}
