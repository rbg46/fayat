using Fred.Web.Models.Referential;

namespace Fred.Web.Models.CI
{
  public class CIDeviseModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CIDevise
    /// </summary>
    public int CiDeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CI.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI associé
    /// </summary>
    public virtual CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la devise associé
    /// </summary>
    public virtual DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit la devise associé
    /// </summary>
    public virtual bool Reference { get; set; }
  }
}