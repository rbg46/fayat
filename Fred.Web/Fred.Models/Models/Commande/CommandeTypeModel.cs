namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Représente un type de commande
  /// </summary>
  public class CommandeTypeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un type de commande.
    /// </summary>
    public int CommandeTypeId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un type de commande.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un type de commande.
    /// </summary>
    public string Libelle { get; set; }
  }
}