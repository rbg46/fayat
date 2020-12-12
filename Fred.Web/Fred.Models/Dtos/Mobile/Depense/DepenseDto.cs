using System;
using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Dtos.Mobile.Depense
{
  /// <summary>
  /// Représente une dépense
  /// </summary>
  public class DepenseDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une dépense.
    /// </summary>
    public int DepenseId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ligne de commande.
    /// </summary>
    public int? CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI d'une dépense.
    /// </summary>
    public int? CiId { get; set; }

    /// <summary>
    /// Obtient ou définit le fournisseur d'une dépense.
    /// </summary>
    public int? FournisseurId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une dépense.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? TypeDepenseId { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité d'une dépense.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire d'une dépense.
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité d'une dépense.
    /// </summary>
    public string Unite { get; set; }

    /// <summary>
    /// Obtient ou définit la date de la dépense.
    /// </summary>
    public DateTime? Date { get; set; }
  }
}
