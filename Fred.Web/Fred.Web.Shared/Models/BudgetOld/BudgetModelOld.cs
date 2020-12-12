using System.Collections.Generic;

namespace Fred.Web.Models.Budget
{
  /// <summary>
  ///   Représente un budget
  /// </summary>
  public class BudgetModelOld
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un budget.
    /// </summary>
    public int BudgetId { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des révisions d'un budget
    /// </summary>
    public ICollection<BudgetRevisionModelOld> BudgetRevisions { get; set; }
  }
}