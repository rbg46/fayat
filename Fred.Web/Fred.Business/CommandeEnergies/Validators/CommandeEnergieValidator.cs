using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Validator Commande Energie
    /// </summary>
    public class CommandeEnergieValidator : AbstractValidator<CommandeEnt>, ICommandeEnergieValidator
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CommandeEnergieValidator" />.
        /// </summary>
        public CommandeEnergieValidator()
        {
            // Règles de gestion métiers
            AddBusinessRules();
        }
        /// <summary>
        /// Règles de gestion métiers
        /// </summary>
        private void AddBusinessRules()
        {
            RuleFor(x => x.TypeEnergieId).GreaterThan(0).WithMessage(FeatureCommandeEnergie.Notification_Type_Energie_Obligatoire);
            RuleFor(x => x.Date).NotNull().WithMessage(FeatureCommandeEnergie.Notification_Periode_Obligatoire);
            RuleFor(x => x.CiId).GreaterThan(0).WithMessage(FeatureCommandeEnergie.Notification_Ci_Obligatoire);
            RuleFor(x => x.FournisseurId).GreaterThan(0).WithMessage(FeatureCommandeEnergie.Notification_Fournisseur_Obligatoire);

            RuleFor(x => x.Numero).NotEmpty().WithMessage(FeatureCommandeEnergie.Notification_Numero_Obligatoire);
            RuleFor(c => c.Lignes).SetCollectionValidator(new CommandeEnergieLigneValidator());
        }
    }
}
