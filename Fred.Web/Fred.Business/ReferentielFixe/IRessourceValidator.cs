using FluentValidation;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.ReferentielFixe
{
  /// <summary>
  ///   Interface de validation d'une Ressource
  /// </summary>
  public interface IRessourceValidator : IValidator<RessourceEnt>
  {
  }
}