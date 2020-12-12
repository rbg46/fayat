﻿namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Critères de recherche des établissements comptables
  /// </summary>
  public class SearchEtablissementComptableModel : SearchActiveModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur Adresse
    /// </summary>
    public bool Adresse { get; set; }
    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur Ville
    /// </summary>
    public bool Ville { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur CodePostal
    /// </summary>
    public bool CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur Pays
    /// </summary>
    public bool Pays { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur ModuleCommandeEnabled
    /// </summary>
    public bool ModuleCommandeEnabled { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur ModuleProductionEnabled
    /// </summary>
    public bool ModuleProductionEnabled { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur DateCreation
    /// </summary>
    public bool DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur DateModification
    /// </summary>
    public bool DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur DateSuppression
    /// </summary>
    public bool DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si on recherche sur IsDeleted
    /// </summary>
    public bool IsDeleted { get; set; }
  }
}