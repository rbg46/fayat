using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    /// Helper pour les validateurs des avenants de commande.
    /// </summary>
    public static class CommandeAvenantValidatorHelper
    {
        private const int CommentaireMaxLength = 500;

        /// <summary>
        /// Règles sur le statut.
        /// </summary>
        /// <param name="validator">Le validateur concerné.</param>
        public static void StatutRules(AbstractValidator<CommandeEnt> validator)
        {
            // La commande doit être validée et non clôturée
            validator.RuleFor(c => c).Custom((c, context) =>
            {
                var commandeValidee = c.IsStatutManuelleValidee || c.IsStatutValidee;
                if (c.IsStatutCloturee || !commandeValidee)
                    context.AddFailure("StatutCommande", FeatureCommande.CmdManager_Avenant_Erreur_CommandeStatut);
            });
        }

        /// <summary>
        /// Règles sur le commentaire fournisseur.
        /// </summary>
        /// <param name="validator">Le validateur concerné.</param>
        public static void CommentaireFournisseurRules(AbstractValidator<CommandeEnt> validator)
        {
            // Commentaire fournisseur
            validator.RuleFor(c => c).Custom((c, context) =>
            {
                if (c.CommentaireFournisseur != null && c.CommentaireFournisseur.Length > CommentaireMaxLength)
                    context.AddFailure("CommentaireFournisseur", string.Format(FeatureCommande.CmdManager_Enregistrement_Erreur_CommentaireFournisseurTropLong, CommentaireMaxLength));
            });
        }

        /// <summary>
        /// Règles sur le commentaire interne.
        /// </summary>
        /// <param name="validator">Le validateur concerné.</param>
        public static void CommentaireInterneRules(AbstractValidator<CommandeEnt> validator)
        {
            // Commentaire interne
            validator.RuleFor(c => c).Custom((c, context) =>
            {
                if (c.CommentaireInterne != null && c.CommentaireInterne.Length > CommentaireMaxLength)
                    context.AddFailure("CommentaireInterne", string.Format(FeatureCommande.CmdManager_Enregistrement_Erreur_CommentaireInterneTropLong, CommentaireMaxLength));
            });
        }
    }
}
