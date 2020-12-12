using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
  /// <summary>
  /// Interface du valideur pour la validation d'un avenant de commande.
  /// </summary>
  public interface ICommandeAvenantValidateValidator : IValidator<CommandeEnt>
  { }
}
