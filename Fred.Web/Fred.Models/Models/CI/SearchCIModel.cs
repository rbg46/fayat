using Fred.Web.Models.Search;
using System;

namespace Fred.Web.Models.CI
{
  /// <summary>
  /// Représente une ci
  /// </summary>
  public class SearchCIModel : AbstractSearchModel
  {
    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public override string ValueText { get; set; }

    //#region Champs de recherche

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le code.
    ///// </summary>
    //public bool Code { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le libelle.
    ///// </summary>
    //public bool Libelle { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le code de la société dont dépend le CI.
    ///// </summary>
    //public bool SocieteCode { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le libelle de la société dont dépend le CI.
    ///// </summary>
    //public bool SocieteLibelle { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le code de l'établissement dont dépend le CI.
    ///// </summary>
    //public bool EtablissementCode { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si on recherche sur le libelle de l'établissement dont dépend le CI.
    ///// </summary>
    //public bool EtablissementLibelle { get; set; }

    //#endregion

    #region Critères

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'on veut filtrer sur les SEP.
    /// </summary>
    public bool IsSEP { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'on veut ramener aussi les CI clôturés.
    /// </summary>
    public bool ClotureOk { get; set; } = false;

    /// <summary>
    /// Obtient ou définit la date d'ouverture minimum du CI.
    /// </summary>
    public DateTime DateOuvertureFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'ouverture maximum du CI.
    /// </summary>
    public DateTime DateOuvertureTo { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fermeture minimum du CI.
    /// </summary>
    public DateTime DateFermetureFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fermeture maximum du CI.
    /// </summary>
    public DateTime DateFermetureTo { get; set; }

    #endregion

    #region Tris

    /// <summary>
    /// Permet de savoir si la requête de recherche 
    /// </summary>
    public bool? TriAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le code.
    /// </summary>
    public bool? CodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le libelle.
    /// </summary>
    public bool? LibelleAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le code de la société dont dépend le CI.
    /// </summary>
    public bool? SocieteCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le libelle de la société dont dépend le CI.
    /// </summary>
    public bool? SocieteLibelleAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le code de l'établissement dont dépend le CI.
    /// </summary>
    public bool? EtablissementCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on tri sur le libelle de l'établissement dont dépend le CI.
    /// </summary>
    public bool? EtablissementLibelleAsc { get; set; }

    #endregion
  }
}