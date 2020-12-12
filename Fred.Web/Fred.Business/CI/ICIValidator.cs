using FluentValidation;
using Fred.Entities.CI;

namespace Fred.Business.CI
{
  /// <summary>
  ///   Interface du valideur des CI
  /// </summary>
  public interface ICIValidator : IValidator<CIEnt>
  {
  }
}