using Fred.Web.Models.Search;

namespace Fred.Web.Models.Referential
{
  public class SearchFournisseurModel : AbstractSearchModel
  {

    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public override string ValueText { get; set; }

    #region Critères

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche un code déplacement actif ou nonSIREN.
    /// </summary>
    public string SIREN { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche une Ville.
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche un Departement.
    /// </summary>
    public string Departement { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche un fournisseur locatier.
    /// </summary>
    public bool Locatier { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si on recherche un fournisseur ETT.
    /// </summary>
    public bool ETT { get; set; }
    #endregion
  }
}