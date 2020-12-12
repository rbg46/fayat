using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
  /// <summary>
  ///   Interface du valideur des commandes
  /// </summary>
  public interface ICommandeValidator : IValidator<CommandeEnt>
  {
  }
}