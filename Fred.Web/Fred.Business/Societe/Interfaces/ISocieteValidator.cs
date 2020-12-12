using FluentValidation;
using Fred.Entities.Societe;

namespace Fred.Business
{
  /// <summary>
  ///   Interface Valideur des Sociétés
  /// </summary>
  public interface ISocieteValidator : IValidator<SocieteEnt>
  {
  }
}