using Fred.Web.Models.Referential.Light;

namespace Fred.Web.Models.Budget
{
  /// <summary>
  ///   Représente Une liaison ressourTache et Devise
  /// </summary>
  public class RessourceTacheDeviseModelOld
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une entité RessourceTacheDevise
    /// </summary>
    public int RessourceTacheDeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la RessourceTache
    /// </summary>
    public int RessourceTacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la RessourceTache
    /// </summary>
    public RessourceTacheModelOld RessourceTache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise
    /// </summary>
    public DeviseLightModel Devise { get; set; }

    /// <summary>
    /// Recopie du prix unitaire de la ressource si personnalisé
    /// </summary>
    public double? PrixUnitaire { get; set; }
  }
}