using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Validators
{
  /// <summary>
  ///   Interface du valideur des lignes de commande
  /// </summary>
  public interface ICommandeLigneValidator : IValidator<CommandeLigneEnt>
  {
  }
}