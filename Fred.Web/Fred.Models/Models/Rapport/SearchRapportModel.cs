using Fred.Web.Models.Search;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Classe représentant les critères de recherches des rapports
  /// </summary>
  public class SearchRapportModel : AbstractSearchModel
  {
    public override string ValueText { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur ayant effectué la recherche
    /// </summary>
    public int DemandeurId { get; set; }

    /// <summary>
    /// Obtient ou définit une varibla indiquant si l'utilisateur connecté possède le rôle gestionnaire de paie
    /// </summary>
    public bool isUtilisateurGsp { get; set; }

    #region Champs de recherche
    /// <summary>
    /// Obtient ou défini une valeur indiquant si on recherche sur le code du CI
    /// </summary>
    public bool CiCode { get; set; }

    /// <summary>
    /// Obtient ou défini une valeur indiquant si on recherche sur le Libelle du CI
    /// </summary>
    public bool CiLibelle { get; set; }

    /// <summary>
    /// Obtient ou défini une valeur indiquant si on recherche sur le nom du rédacteur
    /// </summary>
    public bool Redacteur { get; set; }

    /// <summary>
    /// Obtient ou défini une valeur indiquant si on recherche sur nom du valideur
    /// </summary>
    public bool Valideur { get; set; }

    #endregion

    #region Critères
    /// <summary>
    /// Obtient ou défini une valeur indiquant l'identifiant du statut des rapport à afficher
    /// </summary>
    public string StatutCode { get; set; } 

    /// <summary>
    /// Obtient ou définit le critère définissant que les rapports de tout statut doivent être récupéré
    /// </summary>
    public bool ToutStatut { get; set; }

    /// <summary>
    /// Obtient ou définit le critère définissant que les rapports de statut valide doivent être récupéré
    /// </summary>
    public bool StatutValide { get; set; }

    /// <summary>
    /// Obtient ou définit le critère définissant que les rapports de statut vérouillé doivent être récupéré
    /// </summary>
    public bool StatutVerrouille { get; set; }

    /// <summary>
    /// Obtient ou définit le critère définissant que les rapports non vérouillés doivent être récupéré
    /// </summary>
    public bool StatutNonVerrouille { get; set; }

    /// <summary>
    /// Obtient ou défini une valeur indiquant la date de rapport minimum 
    /// </summary>
    public DateTime DateRapportMin { get; set; }
    /// <summary>
    /// Obtient ou défini une valeur indiquant la date de rapport maximum
    /// </summary>
    public DateTime DateRapportMax { get; set; }

    /// <summary>
    /// Obtient ou définit une variable indiquant si le rôle de l'utilisateur connecté est gestionnaire de paye
    /// </summary>
    public bool IsGSP { get; set; }
    #endregion

    #region Tris
    /// <summary>
    /// Obtient ou définit une valeur indiquant l'ordre du tri sur le code du CI
    /// </summary>
    public bool? CiCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant l'ordre du tri sur le numéro du rapport
    /// </summary>
    public bool? NumeroRapportAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant l'ordre du tri sur le statut des rapports
    /// </summary>
    public bool? StatutAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant l'ordre du tri sur les dates des rapports
    /// </summary>
    public bool? DateRapportAsc { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des champs sur lequel s'applique le tri
    /// </summary>
    public IDictionary<string,bool?> SortFields { get; set; }
    #endregion
  }
}