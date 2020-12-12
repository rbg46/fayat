using FluentValidation;
using Fred.Entities.Budget.Recette;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Interface de validation d'une TacheRecette d'un budget
  /// </summary>
  public interface ITacheRecetteValidatorOld : IValidator<TacheRecetteEnt>
  {
  }
}
