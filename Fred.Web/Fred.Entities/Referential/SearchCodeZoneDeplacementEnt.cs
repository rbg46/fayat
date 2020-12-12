namespace Fred.Entities.Referential
{
  /// <summary>
  ///   Représente une recherche de code zone deplacement
  /// </summary>
  public class SearchCodeZoneDeplacementEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le code condensé
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur l'id de la société
    /// </summary>
    public bool SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libellé du code déplacement
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si un codeAbsence est actif ou non.
    /// </summary>
    public bool IsActif { get; set; }
  }
}