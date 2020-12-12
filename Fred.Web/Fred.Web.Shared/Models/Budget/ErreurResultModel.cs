namespace Fred.Web.Shared.Models.Budget
{
  /// <summary>
  /// Représente un model qui peut contenir une erreur.
  /// </summary>
  public class ErreurResultModel : ResultModelBase
  {
    /// <summary>
    /// Erreur ou null si pas d'erreur.
    /// </summary>
    public string Erreur { get; set; }
  }
}
