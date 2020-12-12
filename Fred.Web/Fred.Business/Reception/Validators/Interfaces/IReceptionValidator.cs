using FluentValidation;
using Fred.Entities.Depense;

namespace Fred.Business.Reception
{
  /// <summary>
  ///   Interface du valideur des Réceptions
  /// </summary>
  public interface IReceptionValidator : IValidator<DepenseAchatEnt>
  {
  }
}