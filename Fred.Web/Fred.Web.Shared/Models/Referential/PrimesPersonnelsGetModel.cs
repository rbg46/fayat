using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Referential
{
  /// <summary>
  /// List prime personnel 
  /// </summary>
  public class PrimesPersonnelsGetModel
  {
    /// <summary>
    /// Get or set date pointage
    /// </summary>
    public DateTime DatePointage { get; set; }

    /// <summary>
    /// Get or set list des identifiers des personnels
    /// </summary>
    public List<int> PersonnelIdList { get; set; }
  }
}
