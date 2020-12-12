using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Rapport hebdo entree view model
  /// </summary>
  public class RapportHebdoEntreeViewModel
  {
    /// <summary>
    /// Monday date
    /// </summary>
    public DateTime Mondaydate { get; set; }

    /// <summary>
    /// Rapport hebdo entree list
    /// </summary>
    public List<RapportHebdoEntreeEntViewModel> RapportHebdoEntreeList { get; set; }
  }
}
