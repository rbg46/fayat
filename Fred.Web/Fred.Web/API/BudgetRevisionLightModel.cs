namespace Fred.Web.API
{
  public class BudgetRevisionLightModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une revision d'un budget.
    /// </summary>    
    public int BudgetRevisionId { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant le statut du budget,
    ///   relatif à l'enum eStatutBudget
    /// </summary>    
    public int Statut { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du budget auquelle cette tâche appartient
    /// </summary>
    public int BudgetId { get; set; }

  }
}