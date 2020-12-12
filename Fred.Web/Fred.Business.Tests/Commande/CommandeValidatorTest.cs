using System;
using FluentAssertions;
using FluentValidation;
using Fred.Business.Commande;
using Fred.Business.Commande.Validators;
using Fred.Business.FeatureFlipping;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Commande.Mock;
using Fred.Common.Tests.Data.Devise.Builder;
using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Commande
{
    [TestClass]
    public class CommandeValidatorTest : BaseTu<CommandeManager>
    {
        private CommandeBuilder CommandeBuilder = null;
        private PersonnelBuilder PersonnelBuilder = null;
        private CommandeLigneBuilder CommandeLigneBuilder = null;
        private CommandeValidator CommandeValidator;
        private const int SeuilCommande = 10000;
        public const int MontantAccordCadre = 15000;
        private const int ContextUtilisateurId = 1;
        private const string EmptyError = "NotEmptyValidator";
        private Mock<ICommandeRepository> commandeRepoFake;

        [TestInitialize]
        public void TestInitialize()
        {
            var mocks = new CommandeMocks();
            var commandeStatutMock = new CommandeStatutMocks();
            var commandeTypeMock = new CommandeTypeMocks();
            var deviseMock = new DeviseMocks();

            //Initialisation des builders pour chaque tests
            CommandeBuilder = new CommandeBuilder();
            CommandeLigneBuilder = new CommandeLigneBuilder();
            PersonnelBuilder = new PersonnelBuilder();

            //Mock les dependances du CommandeValidator (oui il y en a beaucoup...)
            var utilisateurMgrFake = GetMocked<IUtilisateurManager>();
            utilisateurMgrFake.Setup(m => m.GetSeuilValidation(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(SeuilCommande);
            utilisateurMgrFake.Setup(m => m.GetContextUtilisateurId())
                .Returns(ContextUtilisateurId);

            var featureFlippingMgrFake = GetMocked<IFeatureFlippingManager>();
            featureFlippingMgrFake.Setup(m => m.IsActivated(It.IsAny<EnumFeatureFlipping>())).Returns(true);

            //Differe pour chaque test
            var fournisseurRepoFake = GetMocked<IFournisseurRepository>();
            fournisseurRepoFake.Setup(m => m.FindById(It.IsAny<int>()))
                .Returns(new FournisseurBuilder().Build());

            var deviseRepoFake = GetMocked<IDeviseRepository>();
            deviseRepoFake.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns((int deviseId) => deviseMock.GetFakeList().Find(x => x.DeviseId == deviseId));

            commandeRepoFake = GetMocked<ICommandeRepository>();
            commandeRepoFake.Setup(m => m.GetStatutCommandeByStatutCommandeId(It.IsAny<int>()))
                .Returns((int commandeStatutId) => commandeStatutMock.GetFakeList().Find(x => x.StatutCommandeId == commandeStatutId));
            commandeRepoFake.Setup(m => m.GetCommandeTypeByCommandeTypeId(It.IsAny<int>()))
                .Returns((int commandeTypeId) => commandeTypeMock.GetFakeList().Find(x => x.CommandeTypeId == commandeTypeId));

            //Real Validator     
            CommandeValidator = new CommandeValidator(
                utilisateurMgrFake.Object,
                featureFlippingMgrFake.Object,
                fournisseurRepoFake.Object,
                commandeRepoFake.Object,
                deviseRepoFake.Object
            );

            //Real constructor arguments
            SubstituteConstructorArgument<ICommandeValidator>(CommandeValidator);
        }

        /// <summary>
        ///   Teste la validation d'une commande valide avec le statut a brouillon.
        ///     => ne renvoie pas d'erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeValide_Brouillon_PasDeValidationErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Libelle("MaCommandeValide").Numero("2200001")
                   .AddLigne(CommandeLigneBuilder.Quantite(1).PU(1).Build())
                   .Statut.Brouillon()
                   .Do(c => c.ComputeMontantHT())
                   .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().NotThrow<ValidationException>();
        }

        /// <summary>
        ///   Teste la validation d'une commande valide avec le statut a valider.
        ///     => ne renvoie pas d'erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeValide_AValider_PasDeValidationErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Libelle("MaCommandeValide").Numero("2200001")
                    .AddLignes(2)
                    .Type.Prestation()
                    .Statut.AValider()
                    .Contact(PersonnelBuilder.Prototype())
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().NotThrow<ValidationException>();
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un libelle empty.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLibelleEmpty_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Libelle("").Numero("2200001")
                    .AddLignes(2)
                    .Statut.AValider()
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_LibelleObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un numero vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecNumeroVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Libelle("MaCommandeInvalide").Numero("")
                    .AddLignes(2).Statut.AValider()
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(string.Format(FeatureCommande.CmdManager_Numero_ErreurNbCaractere, 10)));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un numero trop long.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecNumeroTropLong_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Libelle("MaCommandeInvalide").Numero("ddededededeeeeeeeeeede")
                    .AddLignes(2)
                    .Statut.AValider()
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(string.Format(FeatureCommande.CmdManager_Numero_ErreurNbCaractere, 10)));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un code postal de facturation a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecFacturationCodePostalVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .FacturationCodePostal("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Facturation C Postale\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec une ville de facturation a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecFacturationVilleVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .FacturationVille("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Facturation Ville\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec une adresse de facturation a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecFacturationAdresseVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .FacturationAdresse("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Facturation Adresse\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un code postal de livraison a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLivraisonCodePostalVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .LivraisonCodePostal("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Livraison C Postale\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec une ville de livraison a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLivraisonVilleVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .LivraisonVille("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Livraison Ville\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec une adresse de livraison a vide.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLivraisonAdresseVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .LivraisonAdresse("")
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains($"\'Livraison Adresse\'") && x.ErrorCode.Equals(EmptyError));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un typeId a null.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecTypeIdNull_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .SetFieldValue("TypeId", null)
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_TypeObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un ciId a null.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecCiIdNull_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.Brouillon()
                    .SetFieldValue("CiId", null)
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(BusinessResources.CIObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un DeviseId a null.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecDeviseIdNull_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.AValider()
                    .SetFieldValue("DeviseId", null)
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DeviseObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec une date a demain.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecDateAujourdhui_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.AddLignes(2).Statut.Brouillon()
                    .Date(DateTime.Now.AddDays(1))
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateAnterieure));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans ligne de commande.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_SansLigneCommande_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.Brouillon().Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_LigneObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans information de suivi.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_SansContact_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Libelle("MaCommandeValide").Numero("2200001")
                    .AddLignes(2).Statut.AValider()
                    .SetFieldValue("ContactId", 0)
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_ContactObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans contact de suivi.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_SansContactSuivi_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Libelle("MaCommandeValide").Numero("2200001").AddLignes(2).Statut.AValider()
                    .SetFieldValue("SuiviId", 0)
                    .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_SuiviObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande manuelle sans numero externe.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeManuelle_SansNumeroExterne_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Manuelle().NumeroExterne("")
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_NumCommandeExterneObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande manuelle avec numero externe trop long.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeManuelle_NumeroExterneTropLong_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Manuelle().NumeroExterne("ddddddddddddddddddddddd")
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(string.Format(FeatureCommande.CmdManager_NumCommandeExterne_ErreurNbCaractere, 20)));
        }

        /// <summary>
        ///   Teste la validation d'une commande manuelle avec numero externe existant deja.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeManuelle_NumeroExterneExisteDeja_RenvoieErreur()
        {
            commandeRepoFake.Setup(m => m.DoesCommandeExist(It.IsAny<Func<CommandeEnt, bool>>()))
                .Returns(true);

            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Manuelle().Statut.AValider().NumeroExterne("Commande1")
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_NumCommandeExterne_ExisteDeja));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans fournisseur id.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_FournisseurIdVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Statut.AValider()
                .SetFieldValue("FournisseurId", null)
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(BusinessResources.FournisseurObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande validee avec un mauvais numero de SIREN.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeValidee_FournisseurSIRENInvalide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Statut.Valider().Type.Prestation()
                .Fournisseur(new FournisseurBuilder().Id(666).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_FournisseurSirenObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans justificatif mais qui a besoin d'un accord cadre.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AccordCadre_SansJustificatif_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.AddLignes(1).Statut.AValider()
                .AccordCadre().Justificatif("")
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_JustificatifRequis));
        }

        /// <summary>
        ///   Teste la validation d'une commande avec un accord cadre et un montant trop eleve.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AccordCadre_MontantTropEleve_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider()
                .Devise(new DeviseBuilder().Code("EUR").Build())
                .AddLigne(CommandeLigneBuilder.Quantite(3).PU(MontantAccordCadre).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_AccordCadreRequis));
        }

        /// <summary>
        ///   Teste la validation d'une commande de type location sans date de mise a disposition.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandePrestation_AvecDateDeDispoNull_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider().Type.Location().AddLignes(3)
                .DateMiseADispo(null)
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateSuiviObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande abonnement sans periodicite.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeAbonnement_SansPeriodicite_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider().AddLignes(2)
                .Abonnement().DureeAbonnement(2).FrequenceAbonnement(null)
                .DateProchaineReception(DateTime.Now.AddMonths(1))
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_AbonnementPeriodicite_Requis));
        }

        /// <summary>
        ///   Teste la validation d'une commande abonnement sans date de prochaine reception.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeAbonnement_SansDateProchaineReception_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider().AddLignes(2)
                .Abonnement().DureeAbonnement(2).FrequenceAbonnement(2)
                .DateProchaineReception(null)
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateProchaineGeneration_Obligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande abonnement sans duree abonnement.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeAbonnement_SansDureeAbonnement_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider().AddLignes(2)
                .Abonnement().DureeAbonnement(null).FrequenceAbonnement(2)
                .DateProchaineReception(DateTime.Now.AddMonths(1))
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_NbReception_Obligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande avec un montant a 0
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecMontantHTaZero_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Numero("2200001").AddLignes(3).Statut.AValider().Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_MontantNotNull));
        }

        /// <summary>
        ///   Teste la validation de la commande avec un montant a 0.001 (minimum est 0.01)
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecMontantEnDessousDuMinimum_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Statut.AValider()
                    .AddLigne(CommandeLigneBuilder.Quantite((decimal)0.1).PU((decimal)0.01).Build())
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_MontantMinumum));
        }

        /// <summary>
        ///   Teste la validation de la commande avec seuil trop eleve. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_SeuilDepasse_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Numero("2200001").Statut.Valider()
                    .AddLignes(CommandeLigneBuilder.Quantite(1).PU(SeuilCommande).BuildNObjects(2, true))
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManager_FournisseurSirenObligatoire));
        }

        //######################### LIGNE COMMANDE ############################



        /// <summary>
        ///   Teste la validation de la commande avec une quantite a zero 
        ///     => doit renvoyer une erreur
        /// </summary>
        /// <remarks>RG_7541_001</remarks>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLigneZeroQuantite_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Numero("2200001").Statut.AValider()
                    .AddLigne(CommandeLigneBuilder.Quantite(10).PU(2).Build())
                    .AddLigne(CommandeLigneBuilder.Quantite(0).PU(2).Build())
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_QuantiteObligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande avec un PuHT a 0. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        /// <remarks>RG_7541_001</remarks>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecZeroPuHT_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                CommandeBuilder.Numero("2200001")
                    .AddLigne(CommandeLigneBuilder.Quantite(10).PU(2).Build())
                    .AddLigne(CommandeLigneBuilder.Quantite(10).PU(0).Build())
                    .Type.Prestation().Statut.AValider()
                    .Do(c => c.ComputeMontantHT())
                    .Build();
            //2. Definition action
            Invoking(() => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_Pu_Energie_Obligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande avec une ligne de commande contenant un libelle vide. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLigneLibelleVide_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Type.Prestation().Statut.AValider()
                .AddLigne(CommandeLigneBuilder.Libelle("").Quantite(10).PU(2).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DesignationObligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande avec une ligne de commande sans unite. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLigneSansUnite_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Type.Prestation().Statut.AValider()
                .AddLigne(CommandeLigneBuilder.Quantite(10).PU(2).SetFieldValue("UniteId", null).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_UniteObligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande abonnement avec une ligne de commande sans tache. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_CommandeAbonnement_AvecLigneSansTache_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Type.Prestation().Statut.AValider()
                .Abonnement().DureeAbonnement(2).FrequenceAbonnement(2).DateProchaineReception(DateTime.Now.AddMonths(1))
                .AddLigne(CommandeLigneBuilder.Quantite(10).PU(2).SetFieldValue("TacheId", null).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_TacheObligatoire));
        }

        /// <summary>
        ///   Teste la validation de la commande avec une ligne de commande sans ressource. 
        ///     => Doit renvoyer une exception.
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeValidation")]
        public void ValidateCommande_AvecLigneSansRessource_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = CommandeBuilder.Type.Prestation().Statut.AValider()
                .AddLigne(CommandeLigneBuilder.Quantite(10).PU(2).SetFieldValue("RessourceId", null).Build())
                .Do(c => c.ComputeMontantHT()).Build();
            //2. Definition action
            Invoking(
                () => Actual.BusinessValidation(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_RessourceObligatoire));
        }
    }
}
