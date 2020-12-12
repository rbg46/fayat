using Fred.Web.Models.Role;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un module.
  /// </summary>
  public class SeuilValidationModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un seuil de validation
    /// </summary>
    public int SeuilValidationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la devise associée
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit la devise associée au seuil de validation
    /// </summary>
    public DeviseModel Devise { get; set; } = null;

    /// <summary>
    /// Obtient ou définit le montant du seuil
    /// </summary>
    public int Montant { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du rôle associé
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// Obtient ou définit le rôle associé au seuil de validation
    /// </summary>
    public RoleModel Role { get; set; } = null;
  }
}
