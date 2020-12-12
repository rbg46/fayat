namespace Fred.Entities.Search
{
  /// <summary>
  /// Interface contenant les propriétés standards de recherche
  /// </summary>
  public interface ISearchableEnt
  {
    /// <summary>
    /// Identifiant de la société
    /// </summary>
    int SocieteId { get; set; }

    /// <summary>
    /// Code de l'entité
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// Libellé de l'entité
    /// </summary>
    string Libelle { get; set; }
  }
}
