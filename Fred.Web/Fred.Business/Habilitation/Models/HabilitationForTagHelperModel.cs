using System.Collections.Generic;

namespace Fred.Business.Habilitation.Models
{
  /// <summary>
  /// Object de mappage
  /// </summary>
  public class HabilitationForTagHelperModel
  {
    /// <summary>
    /// ctor
    /// </summary>
    public HabilitationForTagHelperModel()
    {
      Permissions = new List<PermissionForTagHelperModel>();
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
    public IEnumerable<PermissionForTagHelperModel> Permissions { get; set; }


    /// <summary>
    ///  Liste de persmissions contextuelles
    /// </summary>
    public IEnumerable<PermissionForTagHelperModel> PermissionsContextuelles { get; set; }

    /// <summary>
    /// Retourne l'id de la societe rattache au personnel
    /// </summary>
    public int? SocieteId { get; set; }
  }
}