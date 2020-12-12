using FluentValidation;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Interface de validation d'une RessourceTache d'un budget
  /// </summary>
  public interface IRessourceTacheValidatorOld : IValidator<RessourceTacheEnt>
  {
  }
}