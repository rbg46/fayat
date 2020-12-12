using FluentValidation;
using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    ///   Valideur des Lots de far
    /// </summary>
    public class LotFarValidator : AbstractValidator<LotFarEnt>, ILotFarValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="LotFarValidator" />.
        /// </summary>    
        public LotFarValidator()
        {
            RuleFor(x => x.DateComptable).NotEmpty().NotNull().WithMessage("La date comptablee est obligatoire.");

            RuleFor(x => x.AuteurCreationId).NotEmpty().NotNull().WithMessage("L'auteur de la création est obligatoire.");
        }
    }
}
