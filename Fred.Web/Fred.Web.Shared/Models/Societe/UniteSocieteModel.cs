using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;

namespace Fred.Entities.Societe
{
  /// <summary>
  ///   Représente une société
  /// </summary>
  public class UniteSocieteModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une unité / société.
    /// </summary>
    public int UniteSocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id d'une unité
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit d'une unité
    /// </summary>
    public UniteModel Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id d'une société
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit la société 
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    ///   Obtient ou définit le type
    /// </summary>
    public int Type { get; set; }
  }
}