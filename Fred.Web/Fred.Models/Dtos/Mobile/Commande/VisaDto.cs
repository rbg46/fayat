using System;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Dtos.Mobile.Commande
{
  /// <summary>
  /// Représente un visa d'une commande
  /// </summary>
  public class VisaDto : DtoBase
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
    /// Obtient ou définit la date d'un visa
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
  }
}