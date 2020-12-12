using Fred.Web.Models.Search;

namespace Fred.Web.Models.Societe
{
  /// <summary>
  /// Représente une recherche de société
  /// </summary>
  public class SearchSocieteModel : AbstractSearchModel
  {

    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public override string ValueText { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code condensé
    /// </summary>
    public bool Code { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code société paye
    /// </summary>
    public bool CodeSocietePaye { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code société comptable
    /// </summary>
    public bool CodeSocieteComptable { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le libellé de la société
    /// </summary>
    public bool Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur l'Adresse d'une société
    /// </summary>
    public bool Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur la ville d'une société
    /// </summary>
    public bool Ville { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le code postal d'une société
    /// </summary>
    public bool CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur le numéro SIRET d'une société
    /// </summary>
    public bool SIRET { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur la valeur indiquant si une société est externe au groupe ou non.
    /// </summary>
    public bool Externe { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si une société est active ou non.
    /// </summary>
    public bool Active { get; set; }
  }
}
