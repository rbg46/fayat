using FluentValidation;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Interface de validation d'un BudgetRevision
  /// </summary>
  public interface IBudgetRevisionValidatorOld : IValidator<BudgetRevisionEnt>
  {
  }
}