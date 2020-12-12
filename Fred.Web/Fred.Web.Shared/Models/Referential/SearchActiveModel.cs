namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Critères de recherche d'un élément pouvant être activé
  /// </summary>
  public class SearchActiveModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur un code.
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur libellé.
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche un element actif ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}