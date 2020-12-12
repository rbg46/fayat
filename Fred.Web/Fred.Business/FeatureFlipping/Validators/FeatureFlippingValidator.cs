using FluentValidation;
using Fred.Entities.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;
using System;

namespace Fred.Business.FeatureFlipping.Validators
{
  /// <summary>
  /// Validator des FeaturesFlipping
  /// </summary>
  public class FeatureFlippingValidator : AbstractValidator<FeatureFlippingEnt>, IFeatureFlippingValidator
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FeatureFlippingValidator" />.
    /// </summary>
    public FeatureFlippingValidator()
    {

      //Règles de gestion techniques
      AddChildTechnicalRules();

      //Règles de gestion métiers
      AddBusinessRules();

      //Règles de gestion en cascade.
      AddChildRules();
    }

    /// <summary>
    /// Règles de gestion techniques
    /// </summary>
    private void AddChildRules()
    {
      // N/A
    }

    /// <summary>
    /// Règles de gestion métiers
    /// </summary>
    private void AddBusinessRules()
    {
      RuleFor(feature => feature.Name).NotEmpty().WithMessage(FeatureFeatureFlipping.FeatureFlipping_Error_NomVide);

    }

    /// <summary>
    /// Règles de gestion en cascade.
    /// </summary>
    private void AddChildTechnicalRules()
    {
      // N/A
    }

  }
}
