using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Valideur des Remontées Vrac
  /// </summary>
  public class RemonteeVracValidator : AbstractValidator<RemonteeVracEnt>, IRemonteeVracValidator
  {
    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="RemonteeVracValidator" />.
    /// </summary>    
    public RemonteeVracValidator()
    {
      RuleFor(x => x.AuteurCreationId).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_AuteurCreationObligatoire);

      RuleFor(x => x.Periode).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_PeriodeObligatoire);
    }

    /// <summary>
    ///   Valideur
    /// </summary>
    /// <param name="instance">Lot de pointage</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(RemonteeVracEnt instance)
    {
      return base.Validate(instance);
    }
  }
}