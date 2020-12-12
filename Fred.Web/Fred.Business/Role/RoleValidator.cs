using FluentValidation;
using FluentValidation.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Role;
using Fred.Framework.Extensions;

namespace Fred.Business.Role
{
    public class RoleValidator : AbstractValidator<RoleEnt>, IRoleValidator
    {
        public RoleValidator(IUnitOfWork uow, IRoleRepository roleRepository)
        {
            ISeuilValidationValidator seuilValidationValidator = new SeuilValidationValidator(uow, roleRepository);

            When(role => role.RoleId == 0, () =>
            {
                RuleFor(role => role)
                    .Must(code => !roleRepository.IsExistingRoleByCodeNomFamilierAndSocieteId(code.CodeNomFamilier, code.SocieteId))
                    .WithMessage(BusinessResources.RoleDejaExistant);
            });

            RuleFor(c => c.CodeNomFamilier)
              .NotEmpty()
              .WithMessage(BusinessResources.CodeObligatoire)
              .NotNull()
              .WithMessage(BusinessResources.CodeObligatoire)
              .Must(c => !c.ContainsSpecialChar())
              .WithMessage(BusinessResources.CaracteresSpeciauxInterditLibelle);

            RuleFor(l => l.Libelle)
              .NotEmpty()
              .WithMessage(BusinessResources.LibelleObligatoire)
              .NotNull()
              .WithMessage(BusinessResources.LibelleObligatoire)
              .Must(l => !l.ContainsSpecialChar())
              .WithMessage(BusinessResources.CaracteresSpeciauxInterditCode);

            RuleFor(r => r.SeuilsValidation).SetCollectionValidator(c => seuilValidationValidator);
        }

        public new ValidationResult Validate(RoleEnt instance)
        {
            return base.Validate(instance);
        }
    }
}
