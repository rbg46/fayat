using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.Models.CI
{
  public class CIRessourceModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CIRessource
    /// </summary>
    public int CiRessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CI.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ressource.
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI associé
    /// </summary>
    public virtual CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la ressource associée
    /// </summary>
    public virtual RessourceModel Ressource { get; set; }

    /// <summary>
    /// Obtient ou définit la consommation de la ressource
    /// </summary>
    public virtual decimal? Consommation { get; set; }
  }
}