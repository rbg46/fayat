using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Valideur des Controles de pointage
  /// </summary>
  public class ControlePointageValidator : AbstractValidator<ControlePointageEnt>, IControlePointageValidator
  {
    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="ControlePointageValidator" />.
    /// </summary>    
    public ControlePointageValidator()
    {
      RuleFor(x => x.AuteurCreationId).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_AuteurCreationObligatoire);

      RuleFor(x => x.DateDebut).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_DateDebutObligatoire);

      RuleFor(x => x.LotPointageId).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_LotPointageIdObligatoire);

      RuleFor(x => x.Statut).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_StatutObligatoire);

      RuleFor(x => x.TypeControle).NotEmpty().NotNull().WithMessage(FeatureValidationPointage.VPManager_TypeControleObligatoire);
    }

    /// <summary>
    ///   Valider le contrôle de pointage
    /// </summary>
    /// <param name="instance">Contrôle de pointage</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(ControlePointageEnt instance)
    {
      return base.Validate(instance);
    }
  }
}