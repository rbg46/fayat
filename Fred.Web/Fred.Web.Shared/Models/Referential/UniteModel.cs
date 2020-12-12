namespace Fred.Web.Models.Referential
{
  /// <summary>
  ///   UniteModel
  /// </summary>
  public class UniteModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une Unité.
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une Unité.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une Unité.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé.
    /// </summary>
    public string CodeLibelle { get; set; }


    /// <summary>
    /// Obtient ou définit le libelle du référentiel
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel
    /// </summary>
    public string CodeRef => this.Code;

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'unité
    /// </summary>
    public int IdRef => this.UniteId;
  }
}