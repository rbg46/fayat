using Fred.Web.Dtos.Mobile.Depense;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Dtos.Mobile.Commande
{
  /// <summary>
  /// Représente une ligne de commande
  /// </summary>
  public class CommandeLigneDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ligne de commande.
    /// </summary>
    public int CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ligne de commande.
    /// </summary>
    public int CommandeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une ligne de commande.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la ressource d'une ligne de commande.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des réceptions d'une ligne de commande
    /// </summary>
    public DepenseDto[] Receptions { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une ligne de commande.
    /// </summary>
    [Required]
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le montant d'une ligne de commande.
    /// </summary>
    [Required]
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire d'une ligne de commande.
    /// </summary>
    [Required]
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité d'une ligne de commande.
    /// </summary>
    public string Unite { get; set; }

    /// <summary>
    /// Obtient ou définit le montant d'une ligne de commande.
    /// </summary>
    public decimal MontantHT { get; set; }

    /// <summary>
    /// Obtient le montant total réceptionné d'une ligne de commande
    /// </summary>
    public decimal MontantHTReceptionne { get; set; }

    /// <summary>
    /// Obtient la quantité réceptionnée de la ligne de commande
    /// </summary>
    public decimal QuantiteReceptionnee { get; set; }

    /// <summary>
    /// Obtient le solde de la commande
    /// </summary>
    public decimal MontantHTSolde { get; set; }
  }
}