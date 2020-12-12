using FluentValidation;
using Fred.Entities.Fonctionnalite;

namespace Fred.Business.Module
{
  /// <summary>
  ///   Interface du valideur de fonctionnalités
  /// </summary>
  public interface IFonctionnaliteValidator : IValidator<FonctionnaliteEnt>
  {
  }
}