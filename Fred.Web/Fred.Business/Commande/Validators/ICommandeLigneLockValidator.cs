using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
    public interface ICommandeLigneLockValidator : IValidator<CommandeLigneEnt>
    {
    }
}
