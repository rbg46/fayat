namespace Fred.Web.Shared.Models.Commande.List
{
  public class CIForCommandeListModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ci.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ci.
    /// </summary>   
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une ci.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }
  }
}
