using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.FeatureFlipping;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Referential;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Validators
{
    /// <summary>
    ///   Valideur des commandes
    /// </summary>
    public class CommandeValidator : AbstractValidator<CommandeEnt>, ICommandeValidator
    {
        /// <summary>
        /// Le montant de l'accord cadre.
        /// </summary>
        public const int MontantAccordCadre = 15000;

        private const string CodeEur = "EUR";
        private readonly IUtilisateurManager usrManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly ICommandeRepository cmdRepo;
        private readonly IFournisseurRepository fournisseurRepository;
        private readonly IDeviseRepository deviseRepository;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeValidator" />.
        /// </summary>
        /// <param name="usrManager">Gestionnaire des utilisateurs.</param> 
        /// <param name="uow">Repository des Unit of Work.</param>
        /// <param name="featureFlippingManager">feature flipping manager</param>
        public CommandeValidator(
            IUtilisateurManager usrManager,
            IFeatureFlippingManager featureFlippingManager,
            IFournisseurRepository fournisseurRepository,
            ICommandeRepository commandeRepository,
            IDeviseRepository deviseRepository)
        {
            // NOTE : ce validator est appelé 6~7 fois au chargement d'une seule commande...
            this.cmdRepo = commandeRepository;
            this.fournisseurRepository = fournisseurRepository;
            this.deviseRepository = deviseRepository;
            this.usrManager = usrManager;
            this.featureFlippingManager = featureFlippingManager;

            //----------------------------- Mode [BROUILLON] [A VALIDER] -----------------------------

            When(c => !(c.IsStatutManuelleValidee || c.IsStatutValidee), () =>
            {
                //lancer pour le mode brouillon et à valider
                RulesForAddandUpdate(usrManager);

                //----------------------------- seulement Mode [A VALIDER]  ----------------------------------------------

                When(c => !c.IsStatutBrouillon, () => ActionRulesForSave());
            });

            //----------------------------- seulement Mode [VALIDEE] -----------------------------
            When(c => c.IsStatutManuelleValidee || c.IsStatutValidee, () =>
            {
                FournisseurValideRule();
                SeuilRules();
            });
        }

        private void RulesForAddandUpdate(IUtilisateurManager usrManager)
        {
            RuleFor(c => c.Libelle).NotEmpty().WithMessage(FeatureCommande.CmdManager_LibelleObligatoire);

            RuleFor(c => c.TypeId).NotNull().WithMessage(FeatureCommande.CmdManager_TypeObligatoire);

            RuleFor(c => c.CiId).NotNull().WithMessage(BusinessResources.CIObligatoire);

            RuleFor(c => c.DeviseId).NotNull().WithMessage(FeatureCommande.CmdManager_DeviseObligatoire);

            // Vérifications spécifiques aux commandes manuelles (numero externe)

            RuleFor(c => c.NumeroCommandeExterne)
              .Must((c, numero) => !string.IsNullOrEmpty(numero))
              .When(x => x.CommandeManuelle && x.CommandeId == 0)
              .WithMessage(FeatureCommande.CmdManager_NumCommandeExterneObligatoire);

            RuleFor(c => c.NumeroCommandeExterne)
              .Must((c, numero) => !this.cmdRepo.DoesCommandeExist(cmd => cmd.NumeroCommandeExterne == numero))
              .When(x => x.CommandeManuelle && x.CommandeId == 0 && !x.IsStatutBrouillon)
              .WithMessage(FeatureCommande.CmdManager_NumCommandeExterne_ExisteDeja);

            RuleFor(c => c.NumeroCommandeExterne)
              .Length(1, 20)
              .When(x => x.CommandeManuelle)
              .WithMessage(string.Format(FeatureCommande.CmdManager_NumCommandeExterne_ErreurNbCaractere, 20));

            RuleFor(c => c.Numero).Length(1, 10)
                .WithMessage(string.Format(FeatureCommande.CmdManager_Numero_ErreurNbCaractere, 10));

            RuleFor(c => c.Lignes.Count).GreaterThan(0).WithMessage(FeatureCommande.CmdManager_LigneObligatoire);

            RuleFor(c => c.Lignes).SetCollectionValidator(c => new CommandeLigneValidator(c));

            ConcurrentSuppressionRules();
            CheckMontantTotalCommande(usrManager);

            RuleFor(c => c.Date).LessThan(c => DateTime.Today.AddDays(1).ToUniversalTime()).WithMessage(FeatureCommande.CmdManager_DateAnterieure);
        }

        private void CheckMontantTotalCommande(IUtilisateurManager usrManager)
        {
            RuleFor(c => c.MontantHT)
                .GreaterThan(c => 0)
                .When(x =>
                {
                    // Vérifier droits
                    bool canCreateBrouillonWithFournisseurTemporaire = false;
                    // S'il s'agit d'une commande externe (par exemple importée depuis fred IE), on ne vérifie pas les droits
                    // Corrige le bug 9446
                    if (string.IsNullOrEmpty(x.NumeroCommandeExterne) && x.IsStatutBrouillon)
                    {
                        canCreateBrouillonWithFournisseurTemporaire = usrManager.HasPermissionToCreateBrouillonWithFournisseurTemporaire();
                    }

                    return !canCreateBrouillonWithFournisseurTemporaire;
                })
                .WithMessage(FeatureCommande.CmdManager_MontantNotNull);
        }

        private void ActionRulesForSave()
        {
            FournisseurRules();
            SuiviRules();
            ContactRules();
            LivraisonRules();
            FacturationRules();
            MontantRules();
            DateMiseADispoRules();

            // Validation commande Abonnement
            CommandeValidatorHelper.AbonnementRules(this);
        }

        private void SuiviRules()
        {
            RuleFor(c => c).Custom((c, context) =>
             {
                 if (!c.IsExterne() && (c.SuiviId == null || c.SuiviId == 0))
                     context.AddFailure("SuiviId", FeatureCommande.CmdManager_SuiviObligatoire);
             });
        }

        private void ContactRules()
        {
            RuleFor(c => c).Custom((c, context) =>
             {
                 if (!c.IsExterne() && (c.ContactId == null || c.ContactId == 0))
                     context.AddFailure("ContactId", FeatureCommande.CmdManager_ContactObligatoire);
             });
        }

        private void LivraisonRules()
        {
            RuleFor(c => c.LivraisonAdresse).NotEmpty().NotNull().When(x => !x.IsExterne()).WithMessage(FeatureCommande.CmdManager_AdresseLivraisonObligatoire);
            RuleFor(c => c.LivraisonVille).NotEmpty().NotNull().WithMessage(FeatureCommande.CmdManager_VilleLivraisonObligatoire);
            RuleFor(c => c.LivraisonCPostale).NotEmpty().NotNull().When(x => !x.IsExterne()).WithMessage(FeatureCommande.CmdManager_CPLivraisonObligatoire);
        }

        private void FacturationRules()
        {
            RuleFor(c => c.FacturationAdresse).NotEmpty().NotNull().WithMessage(FeatureCommande.CmdManager_AdresseFacturationObligatoire);
            RuleFor(c => c.FacturationVille).NotEmpty().NotNull().WithMessage(FeatureCommande.CmdManager_VilleFacturationObligatoire);
            RuleFor(c => c.FacturationCPostale).NotEmpty().NotNull().WithMessage(FeatureCommande.CmdManager_CPFacturationObligatoire);
        }

        private void DateMiseADispoRules()
        {
            // Commande de type Location non externe doit avoir une date de mise à disposition
            When(c => !c.IsExterne(), () =>
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

        private void MontantRules()
        {
            AccordCadreRules();
            JustificatifRules();
        }

        private void SeuilRules()
        {
            // Vérification du seuil de commande
            When(c => !c.IsExterne(), () =>
              RuleFor(c => c).Custom((c, context) =>
              {
                  if (c.StatutCommande.Code == StatutCommandeEnt.CommandeStatutVA)
                  {
                      return;
                  }

                  decimal seuil = usrManager.GetSeuilValidation(usrManager.GetContextUtilisateurId(), c.CiId.Value, c.DeviseId.Value);
                  string msg;

                  if (seuil == 0)
                  {
                      msg = string.Format(FeatureCommande.CmdManager_SeuilValidationIndefini, c.Devise.Libelle);
                  }
                  else if (c.MontantHT >= seuil)
                  {
                      msg = string.Format(FeatureCommande.CmdManager_SeuilValidationInvalide, seuil);
                  }
                  else
                  {
                      return;
                  }

                  context.AddFailure("MontantHT", msg);
              }));
        }

        private void AccordCadreRules()
        {
            RuleFor(c => c.AccordCadre)
              .Must((c, accordCadre) =>
              {
                  if (c.IsStatutBrouillon)
                  {
                      return true;
                  }

                  if (!c.DeviseId.HasValue
                   || c.Devise.IsoCode != CodeEur
                   || c.MontantHT <= MontantAccordCadre)
                  {
                      return true;
                  }

                  return c.AccordCadre;
              })
                .WithMessage(FeatureCommande.CmdManager_AccordCadreRequis);
        }

        private void JustificatifRules()
        {
            When(c => c.AccordCadre, () =>
             RuleFor(c => c.Justificatif)
               .Must(j => !string.IsNullOrEmpty(j))
               .WithMessage(FeatureCommande.CmdManager_JustificatifRequis));
        }

        private void FournisseurRules()
        {
            RuleFor(c => c.FournisseurId).NotEmpty().WithMessage(BusinessResources.FournisseurObligatoire);
        }

        private void FournisseurValideRule()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                // Si FF activé & statut validé && commande non de type intérimaire
                if ((this.featureFlippingManager.IsActivated(EnumFeatureFlipping.BlocageFournisseursSansSIRET))
                && (c.IsStatutValidee)
                && (c.Type.Code != CommandeTypeEnt.CommandeTypeI))
                {
                    FournisseurEnt f = this.fournisseurRepository.FindById(c.FournisseurId);

                    // US:7245 
                    // Si le fournisseur est de profession libérale ne pas faire
                    // le check sur le SIREN
                    if (!f.IsProfessionLiberale)
                    {
                        int siren;
                        bool isNumber = int.TryParse(f?.SIREN, out siren);
                        bool isSirenEmptyOrZero = string.IsNullOrEmpty(f?.SIREN) || (isNumber && siren == 0);
                        if (isSirenEmptyOrZero)
                            context.AddFailure("Fournisseur", FeatureCommande.CmdManager_FournisseurSirenObligatoire);
                    }
                }
            });
        }

        private void ConcurrentSuppressionRules()
        {
            // Vérfication sur la date de suppression
            RuleFor(c => c).Custom((c, context) =>
            {
                // Si nouvelle commande => pas de check
                if (c.CommandeId != 0)
                {
                    // Commande from DB
                    CommandeEnt commande = cmdRepo.GetById(c.CommandeId);

                    // Si validation ou sauvegarde d'une commande supprimée
                    if (commande.DateSuppression.HasValue)
                        context.AddFailure("DateSuppression", FeatureCommande.Commande_Detail_Notification_Erreur_Enregistrement_Suppression);
                }
            });
        }

        /// <summary>
        ///   Validater la commande.
        /// </summary>
        /// <param name="instance">La commande.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(CommandeEnt instance)
        {
            if (instance.DeviseId.HasValue)
            {
                instance.Devise = deviseRepository.GetById(instance.DeviseId.Value);
            }

            if (instance.StatutCommandeId.HasValue)
            {
                instance.StatutCommande = cmdRepo.GetStatutCommandeByStatutCommandeId(instance.StatutCommandeId.Value);
            }

            if (instance.TypeId.HasValue)
            {
                instance.Type = cmdRepo.GetCommandeTypeByCommandeTypeId(instance.TypeId.Value);
            }

            return base.Validate(instance);
        }
    }
}
