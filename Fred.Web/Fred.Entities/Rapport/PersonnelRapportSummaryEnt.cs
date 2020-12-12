namespace Fred.Entities.Rapport
{
  /// <summary>
  /// Class encapsule les informations des heures travaillées par un personnel en se basant sur les rapports de pointage
  /// </summary>
  public class PersonnelRapportSummaryEnt : RapportHebdoSummaryBase
  {
    /// <summary>
    /// Personnel id
    /// </summary>
    public int PersonnelId { get; set; }
  }
}
