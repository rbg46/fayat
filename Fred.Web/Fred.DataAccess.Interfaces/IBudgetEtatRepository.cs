
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Référentiel de données pour les barèmes exploitation CI.
  /// </summary>
  public interface IBudgetEtatRepository : IRepository<BudgetEtatEnt>
  {
    /// <summary>
    /// Retourne l'état d'un budget
    /// </summary>
    /// <param name="code">Code de l'état</param>
    /// <returns>L'état d'un budget</returns>
    BudgetEtatEnt GetByCode(string code);
  }
}
