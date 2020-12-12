namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Prime Rapport hebdo model
  /// </summary>
  public class PrimeRapportHebdoModel
  {
    /// <summary>
    /// Obtient ou definit rapport identifier
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    /// Obtient ou definit rapport ligne identifier
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou definit personnel identifier
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou definit ci identifier
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou definit prime identifier
    /// </summary>
    public int PrimeId { get; set; }

    /// <summary>
    /// Obtient ou definit type du prime
    /// </summary>
    public bool IsPrimeJournaliere { get; set; }

    /// <summary>
    /// Obtient ou definit si le prime est checked
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou definit heure prime
    /// </summary>
    public double? HeurePrime { get; set; }

    /// <summary>
    /// Obtient ou definit jour du pointage
    /// </summary>
    public int DayOfWeek { get; set; }
  }
}
