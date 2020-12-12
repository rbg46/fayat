using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
    public class CommandeLigneUnlockValidator : AbstractValidator<CommandeLigneEnt>, ICommandeLigneUnlockValidator
    {
        public CommandeLigneUnlockValidator()
        {
            RuleFor(l => l.IsVerrou).Equal(true).WithMessage(CommandeResources.CommandeLigneAlreadyUnlocked);
        }
    }
}
