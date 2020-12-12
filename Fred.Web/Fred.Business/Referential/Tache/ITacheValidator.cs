using FluentValidation;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.Tache
{
  /// <summary>
  ///   Interface de validation d'une Tache d'un budget
  /// </summary>
  public interface ITacheValidator : IValidator<TacheEnt>
  {
  }
}