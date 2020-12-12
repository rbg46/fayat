using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface du valideur ICommandeEnergieValidator
    /// </summary>
    public interface ICommandeEnergieValidator : IValidator<CommandeEnt>
    {
    }
}
