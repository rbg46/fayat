using FluentValidation;
using Fred.Business.Depense;
using Fred.Entities.Depense;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.Business.Reception
{
    /// <summary>
    ///   Valideur des Réceptions
    /// </summary>
    public class ReceptionValidator : AbstractValidator<DepenseAchatEnt>, IReceptionValidator, IGroupAwareService
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ReceptionValidator" />.
        /// </summary> 
        public ReceptionValidator()
        {
            //  Le type de la dépense achat est obligatoire et doit correspondre au type Réception
            RuleFor(r => r.DepenseTypeId).NotEmpty().NotNull().WithMessage("Le type de la dépense achat est obligatoire.");
            RuleFor(c => c.CiId).NotNull().WithMessage(BusinessResources.CIObligatoire);
            RuleFor(c => c.Date).NotNull().WithMessage(DepenseResources.DateDepenseObligatoire);
            RuleFor(c => c.Libelle).NotEmpty().WithMessage(BusinessResources.LibelleObligatoire);
            RuleFor(c => c.NumeroBL).NotEmpty().NotNull().WithMessage("Le Numéro de bon de livraison est obligatoire.");
            RuleFor(c => c.FournisseurId).NotEmpty().WithMessage(BusinessResources.FournisseurObligatoire);
            RuleFor(c => c.RessourceId).NotEmpty().WithMessage(BusinessResources.RessourceObligatoire);
            RuleFor(c => c.TacheId).NotEmpty().WithMessage(BusinessResources.TacheObligatoire);
            RuleFor(c => c.DeviseId).NotEmpty().WithMessage(BusinessResources.DeviseObligatoire);
            RuleFor(c => c.UniteId).NotEmpty().WithMessage(BusinessResources.UniteObligatoire);
            RuleFor(c => c.PUHT).GreaterThanOrEqualTo(0).WithMessage(DepenseResources.PrixSupEgalZero);
        }

    }
}
