using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Valideur des Lots de pointage
  /// </summary>
  public class LotPointageValidator : AbstractValidator<LotPointageEnt>, ILotPointageValidator
  {
    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="LotPointageValidator" />.
    /// </summary>    
    public LotPointageValidator()
    {
      RuleFor(x => x.AuteurCreationId).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_AuteurCreationObligatoire);

      RuleFor(x => x.Periode).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_PeriodeObligatoire);
    }

    /// <summary>
    ///   Valider le lot de pointage
    /// </summary>
    /// <param name="instance">Lot de pointage</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(LotPointageEnt instance)
    {
      return base.Validate(instance);
    }
  }
}