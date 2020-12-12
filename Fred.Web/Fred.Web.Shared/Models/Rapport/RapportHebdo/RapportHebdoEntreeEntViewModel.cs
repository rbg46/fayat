using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Rapport hebdo entree view model
  /// </summary>
  public class RapportHebdoEntreeEntViewModel
  {
    /// <summary>
    /// Ci id
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Ouvrier list id
    /// </summary>
    public IEnumerable<int> OuvrierListId { get; set; }
  }
}
