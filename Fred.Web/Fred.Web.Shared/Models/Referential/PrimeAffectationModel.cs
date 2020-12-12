namespace Fred.Web.Shared.Models.Referential
{
  /// <summary>
  /// Prime affectation
  /// </summary>
  public class PrimeAffectationModel
  {
    /// <summary>
    /// Obtient le code du prime
    /// </summary>
    public string CodePrime { get; set; }

    /// <summary>
    ///  Obtient Le jour d'affectation 
    /// </summary>
    public int AffectationDay { get; set; }

    /// <summary>
    /// Obtient ou definit le Ci identifier
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou definit si le prime est affecté ou non 
    /// </summary>
    public bool IsAffected { get; set; }
  }
}

