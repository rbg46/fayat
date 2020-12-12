namespace Fred.Entities.Referential
{
  /// <summary>
  ///   Représente une recherche de matériel
  /// </summary>
  public class SearchMaterielEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le Code
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le Libelle
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur IsDeleted
    /// </summary>
    public bool Actif { get; set; }
  }
}