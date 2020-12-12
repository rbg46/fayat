namespace Fred.Entities.Referential
{
  /// <summary>
  ///   Représente une recherche de code absence
  /// </summary>
  public class SearchCodeAbsenceEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le code condensé
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le libellé de la société
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur l'Intemperie
    /// </summary>
    public bool Intemperie { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le code TauxDecote
    /// </summary>
    public bool TauxDecote { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresDefautETAM
    /// </summary>
    public bool NBHeuresDefautETAM { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresMinETAM
    /// </summary>
    public bool NBHeuresMinETAM { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresMaxETAM
    /// </summary>
    public bool NBHeuresMaxETAM { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresDefautCO
    /// </summary>
    public bool NBHeuresDefautCO { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresMinCO
    /// </summary>
    public bool NBHeuresMinCO { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur le NBHeuresMaxCO
    /// </summary>
    public bool NBHeuresMaxCO { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si un codeAbsence est actif ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}