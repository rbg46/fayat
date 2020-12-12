using Fred.Web.Models.Societe;
using System;

namespace Fred.Web.Models.CI
{
  public class CISocieteModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CI.
    /// </summary>
    public int CiSocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un CI.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une société.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la quote-part de la société partenaire.
    /// </summary>
    public byte QuotePart { get; set; }

    /// <summary>
    /// Obtient ou définit le type de participation de la société partenaire.
    /// </summary>
    public int TypeParticipationId { get; set; }

    /// <summary>
    /// Obtient ou définit le type de participation de la société partenaire.
    /// </summary>
    public CITypeParticipationModel TypeParticipation { get; set; }

    /// <summary>
    /// Obtient ou définit le CI associé
    /// </summary>
    public virtual CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la société associé
    /// </summary>
    public virtual SocieteModel Societe { get; set; }
  }
}