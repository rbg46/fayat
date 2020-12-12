using FluentValidation;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Interface du valideur des Controles de pointage
  /// </summary>
  public interface IControlePointageValidator : IValidator<ControlePointageEnt>
  {
  }
}