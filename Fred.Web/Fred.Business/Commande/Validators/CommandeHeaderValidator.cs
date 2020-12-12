using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    ///   Valideur des commandes
    /// </summary>
    public class CommandeHeaderValidator : AbstractValidator<CommandeEnt>, ICommandeHeaderValidator
    {
        /// <summary>
        /// Le montant de l'accord cadre.
        /// </summary>
        public const int MontantAccordCadre = 15000;
        private readonly ICommandeRepository commandeRepository;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CommandeHeaderValidator" />.
        /// </summary>
        /// <param name="commandeRepository">Repository commande.</param>
        /// <param name="statutCommandeManager">Manager Statutcommande</param>
        public CommandeHeaderValidator(ICommandeRepository commandeRepository)
        {
            // NOTE : ce validator est appelé 6~7 fois au chargement d'une seule commande...
            this.commandeRepository = commandeRepository;

            //----------------------------- Mode [BROUILLON] [A VALIDER] [VALIDEE] -----------------------------
            RuleFor(c => c.Libelle).NotEmpty().WithMessage(FeatureCommande.CmdManager_LibelleObligatoire);

            RuleFor(c => c.TypeId).NotNull().WithMessage(FeatureCommande.CmdManager_TypeObligatoire);

            RuleFor(c => c.CiId).NotNull().WithMessage(BusinessResources.CIObligatoire);

            RuleFor(c => c.DeviseId).NotNull().WithMessage(FeatureCommande.CmdManager_DeviseObligatoire);

            RuleFor(c => c.Date).LessThan(_ => DateTime.Today.AddDays(1).ToUniversalTime()).WithMessage(FeatureCommande.CmdManager_DateAnterieure);

            RuleFor(c => c.Date).Must(ValideDate).WithMessage(FeatureCommande.CmdManager_DateObligatoire);
            //----------------------------- Mode [A VALIDER] [VALIDEE] ----------------------------------------------

            RuleFor(c => c).Custom((c, context) =>
            {
                if ((!c.FournisseurId.HasValue && !c.IsStatutBrouillon) && string.IsNullOrEmpty(c.FournisseurProvisoire))
                {
                    context.AddFailure("FournisseurId", BusinessResources.FournisseurObligatoire);
                }
            });

            SuiviRules();
            ContactRules();
            LivraisonRules();
            FacturationRules();
            DateMiseADispoRules();
            JustificatifRules();
            IsExistNumeroExterne();

            // Vérifications spécifiques aux commandes manuelles (numero externe)
            RuleFor(c => c.NumeroCommandeExterne)
              .Must((c, numero) => !string.IsNullOrEmpty(numero))
              .When(x => x.CommandeManuelle && x.CommandeId == 0)
              .WithMessage(FeatureCommande.CmdManager_NumCommandeExterneObligatoire);

            RuleFor(c => c.NumeroCommandeExterne)
              .Length(1, 20)
              .When(x => x.CommandeManuelle)
              .WithMessage(FeatureCommande.CmdManager_nbMaxCaractere);

            RuleFor(c => c.Numero).Length(0, 10).WithMessage(FeatureCommande.CmdManager_nbMinCaractere);

            // Validation commande Abonnement
            CommandeValidatorHelper.AbonnementRules(this);
        }

        private void JustificatifRules()
        {
            When(c => c.AccordCadre, () =>
            {
                RuleFor(c => c.Justificatif)
                .Must(j => !string.IsNullOrEmpty(j))
                .WithMessage(FeatureCommande.CmdManagerHeader_JustificatifRequis);
            });
        }

        private void SuiviRules()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                if (!c.IsStatutBrouillon && !c.IsExterne() && (c.SuiviId == null || c.SuiviId == 0))
                    context.AddFailure("SuiviId", FeatureCommande.CmdManager_SuiviObligatoire);
            });
        }

        private void ContactRules()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                if (!c.IsStatutBrouillon && !c.IsExterne() && (c.ContactId == null || c.ContactId == 0))
                    context.AddFailure("ContactId", FeatureCommande.CmdManager_ContactObligatoire);
            });
        }

        private void LivraisonRules()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                if (!c.IsStatutBrouillon && !c.IsExterne() && string.IsNullOrEmpty(c.LivraisonAdresse))
                    context.AddFailure("LivraisonAdresse", FeatureCommande.CmdManager_AdresseLivraisonObligatoire);
            });

            RuleFor(c => c).Custom((c, context) =>
            {
                if (!c.IsStatutBrouillon && string.IsNullOrEmpty(c.LivraisonVille))
                    context.AddFailure("LivraisonVille", FeatureCommande.CmdManager_VilleLivraisonObligatoire);
            });

            RuleFor(c => c).Custom((c, context) =>
            {
                if (!c.IsStatutBrouillon && !c.IsExterne() && string.IsNullOrEmpty(c.LivraisonCPostale))
                    context.AddFailure("LivraisonCPostale", FeatureCommande.CmdManager_CPLivraisonObligatoire);
            });
        }

        private void FacturationRules()
        {
            When(x => !x.IsStatutBrouillon, () =>
            {
                RuleFor(c => c.FacturationAdresse).NotNull().WithMessage(FeatureCommande.CmdManager_AdresseFacturationObligatoire);
                RuleFor(c => c.FacturationVille).NotNull().WithMessage(FeatureCommande.CmdManager_VilleFacturationObligatoire);
                RuleFor(c => c.FacturationCPostale).NotNull().WithMessage(FeatureCommande.CmdManager_CPFacturationObligatoire);
            });
        }

        private void DateMiseADispoRules()
        {
            // Commande de type Location non externe doit avoir une date de mise à disposition
            When(c => !c.IsExterne() || !c.IsStatutBrouillon, () =>
             RuleFor(c => c.DateMiseADispo)
              .Must(
                    (c, dateMiseADispo) =>
                    {
                        if (!c.TypeId.HasValue || c.Type.Code != CommandeTypeEnt.CommandeTypeL)
                        {
                            return true;
                        }

                        return dateMiseADispo.HasValue;
                    })
              .WithMessage(FeatureCommande.CmdManager_DateSuiviObligatoire));
        }

        private void IsExistNumeroExterne()
        {
            When(x => x.CommandeManuelle && x.CommandeId == 0, () =>
            {
                RuleFor(c => c.NumeroCommandeExterne)
                 .Must((c, numero) => !CheckExistsNumeroCommande(numero))
                 .WithMessage(FeatureCommande.CmdManager_NumCmdExistant);
            });
        }

        private bool CheckExistsNumeroCommande(string numero)
        {
            List<Expression<Func<CommandeEnt, bool>>> filter = new List<Expression<Func<CommandeEnt, bool>>> { x => x.NumeroCommandeExterne == numero };

            return commandeRepository.Search(filter).Any();
        }

        /// <summary>
        /// Vérifie si une date est valide
        /// </summary>
        /// <param name="date">Date à vérifié</param>
        /// <returns>false si la date est égale a la date par défaut</returns>
        private bool ValideDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        /// <summary>
        ///   Validater la commande.
        /// </summary>
        /// <param name="instance">La commande.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(CommandeEnt instance)
        {
            //besoin pour tester sur l'etatd de la commande IsStatutBrouillon/IsstatutValidee
            //instance.StatutCommande = statutCommandeManager.GetAll().Find(x => x.StatutCommandeId == instance.StatutCommandeId);
            if (instance.StatutCommandeId.HasValue)
            {
                instance.StatutCommande = commandeRepository.GetStatutCommandeByStatutCommandeId(instance.StatutCommandeId.Value);
            }

            return base.Validate(instance);
        }
    }
}
