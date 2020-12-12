using Fred.Web.Models.Utilisateur;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Visa
{
  /// <summary>
  /// Représente un visa d'une commande
  /// </summary>
  public class VisaModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un visa
    /// </summary>
    public int? VisaId { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID utilisateur de la commande
    /// </summary>
    [Required]
    public int? CommandeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID utilisateur du visa
    /// </summary>
    [Required]
    public int? UtilisateurId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre de l'utilisateur ayant apposé son visa
    /// </summary>
    public UtilisateurModel Utilisateur { get; set; } = null;

    /// <summary>
    /// Obtient ou définit la date d'un visa
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
  }
}