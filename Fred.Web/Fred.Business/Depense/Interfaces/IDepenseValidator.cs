using FluentValidation;
using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
  /// <summary>
  ///   Interface du valideur des Dépenses
  /// </summary>
  public interface IDepenseValidator : IValidator<DepenseAchatEnt>
  {
  }
}