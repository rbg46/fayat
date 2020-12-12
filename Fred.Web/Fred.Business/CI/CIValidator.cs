using FluentValidation;
using Fred.Entities.CI;

namespace Fred.Business.CI
{
    /// <summary>
    ///   Valideur des CI
    /// </summary>
    public class CIValidator : AbstractValidator<CIEnt>, ICIValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CIValidator" />.
        /// </summary>    
        public CIValidator()
        {
            // Vérifications générales     
            RuleFor(c => c).NotNull().WithMessage("Une affaire doit être passéee.");

            RuleFor(ci => ci.Code).NotNull().NotEmpty().WithMessage(BusinessResources.CodeObligatoire);

            RuleFor(ci => ci.Libelle).NotNull().NotEmpty().WithMessage(BusinessResources.LibelleObligatoire);
        }
    }
}
