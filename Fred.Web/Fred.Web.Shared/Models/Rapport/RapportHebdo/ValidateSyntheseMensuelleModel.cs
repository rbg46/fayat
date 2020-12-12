using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Synthese mensuelle validation Model
  /// </summary>
  public class ValidateSyntheseMensuelleModel
  {
    /// <summary>
    /// List des identifiants ETAM et IAC
    /// </summary>
    public IEnumerable<int> PersonnelIdList { get; set; }

    /// <summary>
    /// month date
    /// </summary>
    public DateTime FirstDayInMonth { get; set; }
  }
}
