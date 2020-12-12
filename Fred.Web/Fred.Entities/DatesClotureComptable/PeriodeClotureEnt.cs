namespace Fred.Entities.DatesClotureComptable
{
  /// <summary>
  /// Entité non sauvegardée en base de donnée.
  /// Permet de retourner au front une periode en indiquant si la periode est cloturée.
  /// </summary>
  public class PeriodeClotureEnt
  {

    /// <summary>
    /// Obtient ou définit l'année.
    /// </summary>
    public int Annee { get; set; }

    /// <summary>
    /// Obtient ou définit le mois.
    /// </summary>
    public int Mois { get; set; }

    /// <summary>
    /// Champ calculé
    /// </summary>
    public bool IsClosed { get; set; }

  }
}
