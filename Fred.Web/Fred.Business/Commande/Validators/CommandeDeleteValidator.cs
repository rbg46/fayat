using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    /// Validateur utilisé lors de la suppression d'une commande
    /// </summary>
    public class CommandeDeleteValidator : AbstractValidator<CommandeEnt>, ICommandeDeleteValidator
    {
        /// <summary>
        /// Constructeur du validator
        /// </summary>
        public CommandeDeleteValidator()
        {
            RuleFor(commande => commande.DateSuppression)
              .Must((dateSuppression) => !dateSuppression.HasValue)
              .When(commande =>
              (commande.StatutCommande?.Code == StatutCommandeEnt.CommandeStatutVA)
              || commande.StatutCommande?.Code == StatutCommandeEnt.CommandeStatutCL
              || commande.StatutCommande?.Code == StatutCommandeEnt.CommandeStatutMVA)
                .WithMessage(FeatureCommande.CmdManager_SuppressionImpossiblePourCeStatut);
        }
    }
}
