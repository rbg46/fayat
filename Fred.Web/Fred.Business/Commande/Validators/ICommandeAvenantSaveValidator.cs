using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
  /// <summary>
  /// Interface du valideur pour l'enregistrement d'un avenant de commande.
  /// </summary>
  public interface ICommandeAvenantSaveValidator : IValidator<CommandeEnt>
  { }
}
