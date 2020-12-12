
namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un code fonction
  /// </summary>
  public class CodeFonctionModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un code fonction.
    /// </summary>
    public int CodeFonctionId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un code fonction.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un code fonction.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }
  }
}