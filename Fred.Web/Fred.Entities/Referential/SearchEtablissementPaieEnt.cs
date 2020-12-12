namespace Fred.Entities.Referential
{
  /// <summary>
  ///   Représente une recherche de code déplacement
  /// </summary>
  public class SearchEtablissementPaieEnt
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur un code de code déplacement.
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche sur libellé de code déplacement.
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche
    /// </summary>
    public bool Adresse { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche
    /// </summary>
    public bool Latitude { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche
    /// </summary>
    public bool Longitude { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche
    /// </summary>
    public bool GestionIndemnites { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche
    /// </summary>
    public bool HorsRegion { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche un code déplacement actif ou non.
    /// </summary>
    public bool Actif { get; set; }
  }
}