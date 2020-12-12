using Fred.Web.Models.Referential;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Societe
{
  /// <summary>
  /// Représente l'association entre une devise et une societe
  /// </summary>
  public class SocieteDeviseModel
  {
    /// <summary>
    ///   Identifiant unique de l'entité SocieteDevise
    /// </summary>
    public int SocieteDeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une société.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la valeur indiquant si la devise rattaché est la devise de référence pour la societe
    /// </summary>
    public bool DeviseDeReference { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI associé
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    ///   Obtient ou définit la société associé
    /// </summary>
    public SocieteModel Societe { get; set; }

  }
}
