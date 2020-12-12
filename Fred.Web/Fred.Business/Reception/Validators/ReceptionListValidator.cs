using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Validators
{
    public class ReceptionListValidator : AbstractValidator<ReceptionListForValidate>, IUpdateReceptionListValidator
    {
        private ICommandeLigneLockingService commandeLigneLockingService;


        public ReceptionListValidator(ICommandeLigneLockingService commandeLigneLockingService)
        {
            this.commandeLigneLockingService = commandeLigneLockingService;

            RuleFor(x => x.Receptions).Must(CanAddOrUpdateReceptionsOnCommandeLignes).WithMessage(DepenseResources.CannotAddReceptionOnCommandeLigne);
        }

        private bool CanAddOrUpdateReceptionsOnCommandeLignes(List<DepenseAchatEnt> depenseAchats)
        {
            var commandeLigneIds = depenseAchats.Select(x => x.CommandeLigneId.Value).ToList();

            return commandeLigneLockingService.CanAddOrUpdateReceptionsOnCommandeLignes(commandeLigneIds);

        }

        public new ValidationResult Validate(ReceptionListForValidate instance)
        {
            return base.Validate(instance);
        }

    }
}
