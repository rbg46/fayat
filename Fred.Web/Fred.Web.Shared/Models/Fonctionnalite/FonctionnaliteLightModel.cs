namespace Fred.Web.Models.Fonctionnalite
{


  /// <summary>
  /// Représente une fonctionnalité
  /// </summary>
  public class FonctionnaliteLightModel
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
    ///   Obtient ou définit la description d'un module
    /// </summary>   
    public string Description { get; set; }


  }
}