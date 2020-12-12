using FluentValidation;
using Fred.Entities;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Validator Associés SEP
    /// </summary>
    public class AssocieSepValidator : AbstractValidator<AssocieSepEnt>, IAssocieSepValidator
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="AssocieSepValidator" />.
        /// </summary>
        public AssocieSepValidator()
        {
            RuleFor(x => x.QuotePart).NotEmpty().WithMessage(FeatureSociete.AssocieSep_QuotePart_Required);
            RuleFor(x => x.TypeParticipationSepId).NotEmpty().WithMessage(FeatureSociete.AssocieSep_TypeParticipation_Required);
            RuleFor(x => x.SocieteId).NotEmpty().WithMessage(FeatureSociete.AssocieSep_SocieteSep_Required);
            RuleFor(x => x.SocieteAssocieeId).NotEmpty().WithMessage(FeatureSociete.AssocieSep_SocieteAssociee_Required);
            RuleFor(x => x.FournisseurId).NotEmpty().WithMessage(FeatureSociete.AssocieSep_Fournisseur_Required);
        }
    }
}
