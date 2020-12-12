using System;
using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    ///   Valideur des lignes de commande
    /// </summary>
    public class CommandeLigneValidator : AbstractValidator<CommandeLigneEnt>, ICommandeLigneValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeLigneValidator" />.
        /// </summary>
        /// <param name="commande">Commande courante à valider</param>
        public CommandeLigneValidator(CommandeEnt commande)
        {
            //RG_8125_006
            RuleFor(l => l.Libelle).NotEmpty().WithMessage(FeatureCommande.CmdManager_DesignationObligatoire);

            if (commande.IsStatutAValider) //evenement Enregistrer commnade Etat normal
            {
                CheckCommmandeAvalider(commande);
            }

            //evenement Enregistrer commnade Brouillon Etat normal
            if (commande.IsStatutBrouillon && commande.FournisseurProvisoire==null && !commande.CommandeAvaliderProvisoire)
            {
                CheckCommmandeBrouillon();
            }

            //evenement Enregistrer commnade avec fournisseur Provisoire
            if (commande.IsStatutBrouillon && commande.CommandeAvaliderProvisoire)
            {
                CheckCommmandeAvalider(commande);
            }
            //Avenants
            When(x => x.AvenantLigneId.HasValue, () => { CheckCommmandeAvalider(commande);});
        }

        /// <summary>
        /// Règle  pour une commande à Valider
        /// </summary>
        /// <param name="commande">Commande courante à valider</param>
        private void CheckCommmandeAvalider(CommandeEnt commande)
        {
            RuleFor(l => l.TacheId).NotEmpty().When(_ => commande.IsAbonnement).WithMessage(FeatureCommande.CmdManager_TacheObligatoire);
            RuleFor(l => l.UniteId).NotEmpty().WithMessage(FeatureCommande.CmdManager_UniteObligatoire);
            RuleFor(l => l.RessourceId).NotEmpty().WithMessage(FeatureCommande.CmdManager_RessourceObligatoire);
            RuleFor(l => l.Quantite).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_QuantiteObligatoire);
            RuleFor(l => l.PUHT).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_Pu_Energie_Obligatoire);
            MontantMinimumRule();
        }

        /// <summary>
        /// Règle  pour une commande etat Broouillon
        /// </summary>
        private void CheckCommmandeBrouillon()
        {
            RuleFor(l => l.Quantite).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_QuantiteObligatoire);
            RuleFor(l => l.PUHT).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_Pu_Energie_Obligatoire);
            MontantMinimumRule();
        }

        /// <summary>
        /// Règle sur le montant minimum
        /// </summary>
        private void MontantMinimumRule()
        {
            // Le montant HT de la ilgne de commande ne doit pas être <= 0.01 sinon erreur SAP
            RuleFor(l => l).Custom((l, context) =>
            {
                l.ComputeMontantHT();
                if (Math.Abs(l.MontantHT) < (decimal)0.01)
                    context.AddFailure("MontantHT", FeatureCommande.CmdManager_MontantMinumum);
            });
        }
    }
}
