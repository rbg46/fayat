using Fred.Entities.Referential;

namespace Fred.Entities.Rapport
{
  /// <summary>
  /// Représente ou défini une majoration
  /// </summary>
  public abstract class PointageMajorationBase
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la ligne majoration du pointage
    /// </summary>
    public abstract int PointageMajorationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public abstract int PointageId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité CodeMajoration
    /// </summary>
    public abstract int CodeMajorationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeMajoration
    /// </summary>
    public abstract CodeMajorationEnt CodeMajoration { get; set; }

    /// <summary>
    ///   Obtient ou définit le l'heure majorée
    /// </summary>
    public abstract double HeureMajoration { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la ligne est en création
    /// </summary>
    public abstract bool IsCreated { get; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la ligne est en modification
    /// </summary>
    public abstract bool IsDeleted { get; set; }
  }
}
