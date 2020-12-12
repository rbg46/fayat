using FluentValidation;
using Fred.Entities.EcritureComptable;

namespace Fred.Business.EcritureComptable.Validator
{
    /// <summary>
    /// Interface du valideur IEcritureComptableCumulValidator
    /// </summary>
    public interface IEcritureComptableCumulValidator : IValidator<EcritureComptableCumulEnt>
    { }
}
