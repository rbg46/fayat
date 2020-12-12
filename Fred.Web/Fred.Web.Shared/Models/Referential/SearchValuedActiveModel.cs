using Fred.Web.Models.Search;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Paramètres de recherche spécifiant la valeur recherchée
  /// </summary>
  public class SearchValueAndSocietyActiveModel : SearchActiveModel, ISearchValueModel
  {
    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public string ValueText { get; set; }

    /// <summary>
    /// Indique s'il faut chercher la valeur exacte
    /// </summary>
    public bool SearchExactly { get; set; }

    /// <summary>
    /// Identifiant de la société pour laquelle on recherche les résultats
    /// </summary>
    public int? SocieteId { get; set; }
  }
}
