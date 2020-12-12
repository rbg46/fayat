using FluentValidation;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business.ReferentielEtendu
{
  /// <summary>
  ///   Interface de validation d'un ReferentielEtendu
  /// </summary>
  public interface IReferentielEtenduValidator : IValidator<ReferentielEtenduEnt>
  {
  }
}