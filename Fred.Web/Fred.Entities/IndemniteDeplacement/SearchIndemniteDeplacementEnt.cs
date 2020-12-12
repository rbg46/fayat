namespace Fred.Entities.IndemniteDeplacement
{
  /// <summary>
  ///   Représente une recherche d'une indemnite de deplacement
  /// </summary>
  public class SearchIndemniteDeplacementEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libelle du CI
    /// </summary>
    public bool Ci { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libelle du code déplacement
    /// </summary>
    public bool CodeDeplacement { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libelle du code zone deplacement
    /// </summary>
    public bool CodeZoneDeplacement { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si une indemnite de deplacement est
    ///   active ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}