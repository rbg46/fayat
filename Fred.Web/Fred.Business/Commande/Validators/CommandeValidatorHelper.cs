using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    /// Helper pour valider une commande.
    /// </summary>
    public static class CommandeValidatorHelper
    {
        /// <summary>
        /// Rules for commande abonnement.
        /// </summary>
        /// <param name="validator">Le validateur concerné.</param>
        public static void AbonnementRules(AbstractValidator<CommandeEnt> validator)
        {
            validator.RuleFor(c => c.FrequenceAbonnement).NotNull().WithMessage(FeatureCommande.CmdManager_AbonnementPeriodicite_Requis)
                                               .NotEmpty().WithMessage(FeatureCommande.CmdManager_AbonnementPeriodicite_Requis)
                                               .When(c => c.IsAbonnement && !c.IsStatutCloturee);
            validator.RuleFor(c => c.DureeAbonnement).NotNull().When(c => c.IsAbonnement && !c.IsStatutCloturee).WithMessage(FeatureCommande.CmdManager_NbReception_Obligatoire);
            validator.RuleFor(c => c.DateProchaineReception).NotNull().WithMessage(FeatureCommande.CmdManager_DateProchaineGeneration_Obligatoire)
                                                  .NotEmpty().WithMessage(FeatureCommande.CmdManager_DateProchaineGeneration_Obligatoire)
                                                  .When(c => c.IsAbonnement && !c.IsStatutCloturee);
        }
    }
}
