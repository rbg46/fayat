namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Majoration personnel ci Class
  /// </summary>
  public class MajorationPersonnelCiModel
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
    /// Obtient ou definit majoration code identifier
    /// </summary>
    public int MajorationCodeId { get; set; }

    /// <summary>
    /// Obtient ou definit heure majoration
    /// </summary>
    public double? HeureMajoration { get; set; }

    /// <summary>
    /// Obtient ou definit jour du pointage
    /// </summary>
    public int DayOfWeek { get; set; }
  }
}
