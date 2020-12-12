using FluentValidation;
using Fred.Entities.Carburant;

namespace Fred.Business.Carburant
{
    /// <summary>
    ///   Validator des CarburantOrganisationDevise
    /// </summary>
    public class ParametrageCarburantValidator : AbstractValidator<CarburantOrganisationDeviseEnt>, IParametrageCarburantValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ParametrageCarburantValidator" />.
        /// </summary>        
        public ParametrageCarburantValidator()
        {
            // Vérifications générales     
            RuleFor(c => c.CarburantId).NotNull().WithMessage("Le carburant est obligatoire.");

            RuleFor(c => c.OrganisationId).NotNull().WithMessage("L'organisation est obligatoire.");

            RuleFor(c => c.DeviseId).NotNull().WithMessage("La devise est obligatoire.");

            RuleFor(c => c.Prix).NotNull().WithMessage("Le prix est obligatoire.")
                                .GreaterThanOrEqualTo(0).WithMessage("Le prix doit être positif.");

            RuleFor(c => c.Periode).NotNull().WithMessage("La période est obligatoire.");
        }
    }
}
