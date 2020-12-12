using FluentValidation;
using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
  /// <summary>
  ///   Interface du valideur des Lots de far
  /// </summary>
  public interface ILotFarValidator : IValidator<LotFarEnt>
  {
  }
}