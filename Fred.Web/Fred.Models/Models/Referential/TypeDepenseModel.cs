namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Modèle d'un type de dépense.
  /// </summary>
  public class TypeDepenseModel
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

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle
    {
      get
      {
        return this.Code + " - " + this.Libelle;
      }
    }
  }
}