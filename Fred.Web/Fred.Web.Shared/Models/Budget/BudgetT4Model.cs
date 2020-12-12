using Fred.Entities.Budget;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Budget
{
  /// <summary>
  /// Représente un budget T4.
  /// </summary>
  public class BudgetT4Model
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une tâche T4 dans un budget.
    /// </summary>
    public int BudgetT4Id { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI auquel se budget appartient
    /// </summary>
    public int BudgetId { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public BudgetEnt Budget { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI auquel se budget appartient
    /// </summary>
    public int T4Id { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public TacheEnt T4 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI auquel se budget appartient
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public UniteEnt Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
    /// </summary>
    public decimal QuantiteARealiser { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
    /// </summary>
    public decimal MontantSousDetail { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
    /// </summary>
    public decimal MontantT4 { get; set; }
  }
}
