using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;
using System.Collections.Generic;

namespace Fred.Web.Models.Affectation
{
  /// <summary>
  /// Affecation class model 
  /// </summary>
  public class AffectationModel
  {
    /// <summary>
    /// Obtient ou definit l'identifiant unique d'une affectation 
    /// </summary>
    public int AffectationId { get; set; }

    /// <summary>
    /// Obtient ou definit le role delegue de l'affectation 
    /// </summary>
    public bool IsDelegue { get; set; }

    /// <summary>
    /// Obtient ou definit l'identifiant unique du CI d'une affectation
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient le CI de l'affectation
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou definit l'identifiant unique du personnel
    /// </summary>
    public int PersonnelId { get; set; }
    /// <summary>
    /// Obtient ou definit le personnel affecté
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///  Obtient ou définit la liste des astreintes
    /// </summary>
    public ICollection<AstreinteModel> Astreintes { get; set; }

    /// <summary>
    /// check if this affection the default or not 
    /// </summary>
    public bool IsDefault { get; set; }
  }
}
