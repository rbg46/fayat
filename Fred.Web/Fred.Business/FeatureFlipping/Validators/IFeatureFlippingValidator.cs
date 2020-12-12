using FluentValidation;
using Fred.Entities.FeatureFlipping;

namespace Fred.Business.FeatureFlipping.Validators
{

  /// <summary>
  ///   Interface du valideur des Features Flipping
  /// </summary>
  public interface IFeatureFlippingValidator : IValidator<FeatureFlippingEnt>
  {

  }
}
