using Fred.Entities.Referential;

namespace Fred.Entities.Rapport
{
  /// <summary>
  ///   Représente ou défini un pointage
  /// </summary>
  public abstract class PointagePrimeBase
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la ligne prime du pointage
    /// </summary>
    public abstract int PointagePrimeId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public abstract int PointageId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de la prime
    /// </summary>
    public abstract int PrimeId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité Prime
    /// </summary>
    public abstract PrimeEnt Prime { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la prime est checkée
    /// </summary>
    public abstract bool IsChecked { get; set; }

    /// <summary>
    ///   Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
    /// </summary>
    public abstract double? HeurePrime { get; set; }

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