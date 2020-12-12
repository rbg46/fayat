using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    /// Valideur pour l'enregistrement d'un avenant de commande.
    /// </summary>
    public class CommandeAvenantSaveValidator : AbstractValidator<CommandeEnt>, ICommandeAvenantSaveValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeAvenantSaveValidator" />.
        /// </summary>
        public CommandeAvenantSaveValidator()
        {
            CommandeAvenantValidatorHelper.StatutRules(this);
            MontantHtRules();
            CommandeAvenantValidatorHelper.CommentaireFournisseurRules(this);
            CommandeAvenantValidatorHelper.CommentaireInterneRules(this);
            CommandeValidatorHelper.AbonnementRules(this);

            // NPI : accord cadre ? justificatif ?

            // Valide les lignes de l'avenant
            RuleFor(c => c.Lignes).SetCollectionValidator(c => new CommandeLigneValidator(c));
        }

        private void MontantHtRules()
        {
            // Le montant HT de la commande ne doit pas être <= 0 ni exéder le montant accord cadre
            RuleFor(c => c).Custom((c, context) =>
            {
                c.ComputeMontantHT();
                if (c.MontantHT <= 0)
                    context.AddFailure("MontantHT", string.Format(FeatureCommande.CmdManager_MontantNotNull));
            });
        }
    }
}
