namespace Fred.Web.Dtos.Mobile.Referential
{
  /// <summary>
  /// Dto d'un type de dépense.
  /// </summary>
  public class TypeDepenseDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un type de dépense.
    /// </summary>
    public int TypeDepenseId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un type de dépense.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un type de dépense.
    /// </summary>
    public string Libelle { get; set; }
  }
}