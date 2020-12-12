using FluentValidation;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Interface du valideur des Lots de pointage
  /// </summary>
  public interface ILotPointageValidator : IValidator<LotPointageEnt>
  {
  }
}