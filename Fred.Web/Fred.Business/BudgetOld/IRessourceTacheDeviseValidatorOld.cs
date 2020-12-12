using FluentValidation;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Interface de validation d'une RessourceTacheDevise d'un budget
  /// </summary>
  public interface IRessourceTacheDeviseValidatorOld : IValidator<RessourceTacheDeviseEnt>
  {
  }
}