using FluentValidation;
using Fred.Business.Utilisateur;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    /// Valideur pour l'enregistrement d'un avenant de commande.
    /// </summary>
    public class CommandeAvenantValidateValidator : AbstractValidator<CommandeEnt>, ICommandeAvenantValidateValidator
    {
        private readonly IUtilisateurManager utilisateurMgr;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeAvenantValidateValidator" />.
        /// </summary>
        /// <param name="utilisateurMgr">Gestionnaire des utilisateurs.</param>
        public CommandeAvenantValidateValidator(IUtilisateurManager utilisateurMgr)
        {
            this.utilisateurMgr = utilisateurMgr;

            CommandeAvenantValidatorHelper.StatutRules(this);
            MontantHtRules();
            CommandeAvenantValidatorHelper.CommentaireFournisseurRules(this);
            CommandeAvenantValidatorHelper.CommentaireInterneRules(this);
            CommandeValidatorHelper.AbonnementRules(this);

            // Valide les lignes de l'avenant
            RuleFor(c => c.Lignes).SetCollectionValidator(c => new CommandeLigneValidator(c));
        }

        private void MontantHtRules()
        {
            // Le montant HT de la commande ne doit pas être <= 0 ni exéder le seuil de l'utilisateur
            RuleFor(c => c).Custom((c, context) =>
            {
                if (c.StatutCommande.Code == StatutCommandeEnt.CommandeStatutVA)
                {
                    return;
                }

                c.ComputeMontantHT();
                 if (c.MontantHT <= 0)
                     context.AddFailure("MontantHT", string.Format(FeatureCommande.CmdManager_MontantNotNull));

                 var seuil = utilisateurMgr.GetSeuilValidation(utilisateurMgr.GetContextUtilisateurId(), c.CiId.Value, c.DeviseId.Value);
                 if (seuil == 0)
                     context.AddFailure("MontantHT", string.Format(FeatureCommande.CmdManager_SeuilValidationIndefini, c.Devise.Libelle));

                 if (c.MontantHT >= seuil)
                     context.AddFailure("MontantHT", string.Format(FeatureCommande.CmdManager_SeuilValidationInvalide, seuil));
             });
        }
    }
}
