namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Modèle du statut de la commande.
  /// </summary>
  public class StatutCommandeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un statut de commande.
    /// </summary>
    public int StatutCommandeId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un statut de commande.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un statut de commande.
    /// </summary>
    public string Libelle { get; set; }
  }
}