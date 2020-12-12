using Fred.Web.Models.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Module
{
  /// <summary>
  /// Représente une fonctionnalité
  /// </summary>
  public class FonctionnaliteModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une fonctionnalité.
    /// </summary>
    public int FonctionnaliteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un module.
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet module attaché à une fonctionnalité
    /// </summary>
    public ModuleModel Module { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une fonctionnalité
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une fonctionnalité.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la fonctionnalité est hors organisation ou non
    /// </summary>
    public bool HorsOrga { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression d'une fonctionnalité
    /// </summary>
    public DateTime? DateSuppression { get; set; }
  }
}