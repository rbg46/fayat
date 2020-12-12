using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    ///   Interface de validation lors de la suppression d'une commande
    /// </summary>
    public interface ICommandeDeleteValidator : IValidator<CommandeEnt>
    {

    }
}
