using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
    public class CommandeLigneLockValidator : AbstractValidator<CommandeLigneEnt>, ICommandeLigneLockValidator
    {
        public CommandeLigneLockValidator()
        {
            RuleFor(l => l.IsVerrou).Equal(false).WithMessage(CommandeResources.CommandeLigneAlreadyLocked);
        }
    }
}
