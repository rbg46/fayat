using System;
using FluentAssertions;
using FluentValidation;
using Fred.Business.Commande;
using Fred.Business.Commande.Validators;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Commande.Mock;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Commande
{
    /// <summary>
    /// Test le cas metier de validation de header de la commande
    /// </summary>
    [TestClass]
    [Ignore]
    public class ValidateHeaderCommandeTest : BaseTu<CommandeManager>
    {
        //Builder
        private CommandeBuilder commandeBuilder = null;
        private PersonnelBuilder personnelBuilder = null;
        private CommandeLigneBuilder commandeLigneBuilder = null;

        //Validator
        private CommandeHeaderValidator commandeHeaderValidator;
        private CommandeValidator commandeValidator;

        //Mocks
        private Mock<IStatutCommandeManager> statutCommandeManagerFake;

        //Variables
        private const int SeuilCommande = 10000;
        public const int MontantAccordCadre = 15000;
        private const int ContextUtilisateurId = 1;
        private const string EmptyError = "notempty_error";

        [TestInitialize]
        public void TestInitialize()
        {
            ////////////////////
            //A MOCKER ICommandeRepository commandeRepository, IStatutCommandeManager statutCommandeManager
            ////////////////////

            var fakeContext = GetMocked<FredDbContext>();

            var mocks = new CommandeMocks();
            var commandeStatutMock = new CommandeStatutMocks();
            var commandeTypeMock = new CommandeTypeMocks();

            //Initialisation des builders pour chaque tests
            commandeBuilder = new CommandeBuilder();
            commandeLigneBuilder = new CommandeLigneBuilder();
            personnelBuilder = new PersonnelBuilder();

            //Mock les dependances du CommandeValidator (oui il y en a beaucoup...)
            //var utilisateurMgrFake = GetMocked<IUtilisateurManager>();
            //utilisateurMgrFake.Setup(m => m.GetSeuilValidation(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            //    .Returns(SeuilCommande);
            //utilisateurMgrFake.Setup(m => m.GetContextUtilisateurId())
            //    .Returns(ContextUtilisateurId);
            //var featureFlippingMgr = GetMocked<IFeatureFlippingManager>();
            //featureFlippingMgr.Setup(m => m.IsActivated(It.IsAny<EnumFeatureFlipping>())).Returns(true);

            //var uow = mocks.GetRealUowWithFakeDbSet(fakeContext);

            ////Real Validator     
            //CommandeValidator = new CommandeValidator(
            //    utilisateurMgrFake.Object,
            //    uow,
            //    featureFlippingMgr.Object
            //);

            //Real constructor arguments
            SubstituteConstructorArgument<IStatutCommandeManager>(statutCommandeManagerFake.Object);
            //SubstituteConstructorArgument<IUnitOfWork>(uow);
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_WithCommandeNull_ReturnArgumentNullException()
        {
            Invoking(() => Actual.ValidateHeaderCommande(null)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecCommandeLibelleEmpty_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("").Numero("2200001").Statut.AValider().Type.Prestation().Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_LibelleObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecCommandeTypeId_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Statut.AValider().Type.Prestation().TypeId(null).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_TypeObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecCommandeCiId_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Type.Prestation().CiId(null).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(BusinessResources.CIObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecCommandeDeviseId_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Type.Prestation().DeviseId(null).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DeviseObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecDateValeurParDefaut_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Type.Prestation().Date(new DateTime()).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecDateSuperieurAAujourdhui_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Type.Prestation().Date(DateTime.Today.AddDays(2)).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateAnterieure));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecCommandeFournisseurId_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("dede").Numero("2200001").Type.Prestation().Statut.AValider().FournisseurId(null).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(BusinessResources.FournisseurObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans information de suivi.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_SansContactSuivi_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Type.Prestation().Statut.AValider().SetFieldValue("SuiviId", 0).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_SuiviObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans contact de suivi.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_SansContact_RenvoieErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Type.Prestation().Statut.AValider().SetFieldValue("ContactId", 0).Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_ContactObligatoire));
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecLivraisonAdresseValide_NeRenvoiePasErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Statut.AValider()
                .LivraisonAdresse("1 rue des acacias").LivraisonCodePostal("33000").LivraisonVille("Paris")
                .Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().NotThrow<ValidationException>();
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecFournisseurAdresseValide_NeRenvoiePasErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Statut.AValider()
                .FournisseurAdresse("1 rue des acacias").FournisseurCPostal("33000").FournisseurVille("Paris")
                .Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().NotThrow<ValidationException>();
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_AvecFacturationAdresseValide_NeRenvoiePasErreur()
        {
            //1. Preparation des donnees
            CommandeEnt commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Statut.AValider()
                .FacturationAdresse("1 rue des acacias").FacturationCodePostal("33000").FacturationVille("Paris")
                .Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().NotThrow<ValidationException>();
        }

        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateHeaderCommande_DeTypeLocation_SansDateDeMiseADisposition_RenvoieErreur()
        {
            //1. Preparation des donnees
            var commande = commandeBuilder.Libelle("CMDValide").Numero("2200001").Type.Location().Statut.AValider().Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureCommande.CmdManager_DateSuiviObligatoire));
        }

        /// <summary>
        ///   Teste la validation d'une commande sans justificatif mais qui a besoin d'un accord cadre.
        ///     => renvoie une erreur de validation
        /// </summary>
        [TestMethod]
        [TestCategory("ValidateHeaderCommande")]
        public void ValidateCommande_AccordCadre_SansJustificatif_RenvoieErreur()
        {
            //1. Preparation des donnees
            var commande = commandeBuilder.Type.Prestation().Statut.AValider().AccordCadre().Justificatif("").Build();
            //2. Definition action
            Invoking(() => Actual.ValidateHeaderCommande(commande))
            //3. Assertion
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains(FeatureCommande.CmdManagerHeader_JustificatifRequis));
        }
    }
}
