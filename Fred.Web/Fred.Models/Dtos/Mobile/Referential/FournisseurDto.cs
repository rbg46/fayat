namespace Fred.Web.Dtos.Mobile.Referential
{
  /// <summary>
  /// Représente un fournisseur
  /// </summary>
  public class FournisseurDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un fournisseur.
    /// </summary>
    public int FournisseurId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un fournisseur.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un fournisseur.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }
  }
}