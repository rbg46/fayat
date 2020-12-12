using Fred.Entities;

namespace Fred.Business.Habilitation.Models
{
  /// <summary>
  /// Model pour decrire les persmission dans le tag helper pour les habilitations
  /// </summary>
  public class PermissionForTagHelperModel
  {
    /// <summary>
    /// id
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// clé permettant l'unicité d'un permission
    /// exemple : "menu.show.budget.index"
    /// </summary>   
    public string PermissionKey { get; set; }

    /// <summary>
    /// Type de permission par exemple affichage des menu
    /// </summary>
    public int PermissionType { get; set; }

    /// <summary>
    /// Type de permission par exemple affichage des menu
    /// </summary>
    public FonctionnaliteTypeMode Mode { get; set; }

  }
}
