using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Referential;

namespace Fred.Entities.Societe
{
  /// <summary>
  ///   Représente une SocieteDevise (association entre une societe et une devise)
  /// </summary>
  public class SocieteDeviseEnt
  {
    /// <summary>
    ///   Identifiant unique de l'entité SocieteDevise
    /// </summary>
    public int SocieteDeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une societe.
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
    ///   Obtient ou définit la societe associé
    /// </summary>
    public virtual SocieteEnt Societe { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise associé
    /// </summary>
    public virtual DeviseEnt Devise { get; set; }
  }
}