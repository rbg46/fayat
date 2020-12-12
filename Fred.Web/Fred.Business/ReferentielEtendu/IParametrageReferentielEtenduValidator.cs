using FluentValidation;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business.ReferentielEtendu
{
  /// <summary>
  ///   Interface de validation d'un ParametrageReferentielEtendu
  /// </summary>
  public interface IParametrageReferentielEtenduValidator : IValidator<ParametrageReferentielEtenduEnt>
  {
  }
}