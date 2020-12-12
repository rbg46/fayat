namespace Fred.Web.Models.Personnel
{
  /// <summary>
  /// Représente un type de rattachement
  /// </summary>
  public class TypeRattachementModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un type de rattachement.
    /// </summary>
    public int TypeRattachementId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un type de rattachement.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un type de rattachement.
    /// </summary>
    public string Libelle { get; set; }
  }
}