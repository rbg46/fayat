namespace Fred.Web.Models.Search
{
  /// <summary>
  /// Interface à implémenter pour les objets de recherche contenant une valeur textuelle
  /// </summary>
  public interface ISearchValueModel
  {
    /// <summary>
    /// Valeur textuelle recherchée
    /// </summary>
    string ValueText { get; set; }
  }
}