using System;

namespace Fred.Web.Shared.Models.Moyen
{
  /// <summary>
  /// Représente un site
  /// </summary>
  public class SiteModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant du site
    /// </summary>
    public int SiteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code du site
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libélle du site
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le date de création du site
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit le code réference du site
    /// </summary>
    public string CodeRef
    {
      get
      {
        return Code;
      }
    }

    /// <summary>
    /// Obtient ou définit le libelle réference du site
    /// </summary>
    public string LibelleRef
    {
      get
      {
        return Libelle;
      }
    }
  }
}
