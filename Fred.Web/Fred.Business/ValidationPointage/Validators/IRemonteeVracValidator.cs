using FluentValidation;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Interface du valideur des Remontées Vrac
  /// </summary>
  public interface IRemonteeVracValidator : IValidator<RemonteeVracEnt>
  {
  }
}