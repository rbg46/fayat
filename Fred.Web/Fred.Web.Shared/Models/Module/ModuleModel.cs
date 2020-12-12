using Fred.Web.Models.Fonctionnalite;
using System;

namespace Fred.Web.Models.Module
{
  /// <summary>
  /// Représente un module
  /// </summary>
  public class ModuleModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un module.
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un module
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit la description d'un module
    /// </summary>   
    public string Description { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un module
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression d'un module
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des fonctionnalités d'un module
    /// </summary>
    public FonctionnaliteModel[] Fonctionnalites { get; set; }


  }
}