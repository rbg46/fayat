using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Avenant;
using Fred.Business.Commande.Validators;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Web.Shared.Models.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Avenant
{
    [TestClass]
    public class AvenantManagerTest : BaseTu<AvenantManager>
    {
        private const int utilisateurId = 1;
        private readonly DateTime dateTraitement = new DateTime(2019, 11, 1);

        private CommandeAvenantEnt commandeAvenantWithValidationNull;
        private CommandeAvenantEnt commandeAvenantWithValidationNotNull;
        private CommandeEnt defaultCommande;
        private CommandeEnt defaultCommandeWithLignes;
        private CommandeEnt commandeWithLignes;

        private CommandeAvenantBuilder commandeAvenantBuilder;

        private CommandeAvenantSaveModelBuilder commandeAvenantSaveModelBuilder;
        private CommandeAvenantSaveModelBuilder defaultcommandeAvenantSaveModelBuilder;

        private CommandeAvenantSave.Model commandeAvenantSaveModel;

        private Mock<ICommandeAvenantRepository> commandeAvenantRepositoryFake;
        private Mock<ICommandeLigneAvenantRepository> commandeLigneAvenantRepository;
        private Mock<ICommandeRepository> commandeRepositoryFake;
        private Mock<ICommandeLignesRepository> commandeLignesRepositoryFake;
        private Mock<IUnitOfWork> unitOfWorkFake;
        private Mock<IUtilisateurManager> userManagerFake;
        private Mock<ICommandeAvenantSaveValidator> commandeAvenantSaveValidatorFake;

        [TestInitialize]
        public void TestInitialize()
        {
            commandeAvenantSaveModelBuilder = new CommandeAvenantSaveModelBuilder();
            commandeAvenantBuilder = new CommandeAvenantBuilder();

            var utilisateurBuilder = new UtilisateurBuilder();
            var utilisateur = utilisateurBuilder.UtilisateurId(utilisateurId).Build();
            userManagerFake = GetMocked<IUtilisateurManager>();
            userManagerFake.Setup(u => u.GetContextUtilisateur()).Returns(utilisateur);

            commandeAvenantRepositoryFake = GetMocked<ICommandeAvenantRepository>();
            commandeLigneAvenantRepository = GetMocked<ICommandeLigneAvenantRepository>();
            commandeRepositoryFake = GetMocked<ICommandeRepository>();
            commandeLignesRepositoryFake = GetMocked<ICommandeLignesRepository>();
            commandeAvenantSaveValidatorFake = GetMocked<ICommandeAvenantSaveValidator>();
            unitOfWorkFake = GetMocked<IUnitOfWork>();

            commandeAvenantWithValidationNull = new CommandeAvenantBuilder().WithDateValidationNull().Build();
            commandeAvenantWithValidationNotNull = new CommandeAvenantBuilder().WithDateValidationNotNull().Build();

            commandeAvenantSaveValidatorFake.Setup(c => c.Validate(It.IsAny<CommandeEnt>())).Returns(new ValidationResult());
            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeId(It.IsAny<int>())).Returns(new List<CommandeAvenantEnt>() { commandeAvenantWithValidationNull });

            defaultcommandeAvenantSaveModelBuilder = new CommandeAvenantSaveModelBuilder().ParDefaut();
            defaultCommande = new CommandeBuilder().ParDefaut().Build();
            defaultCommandeWithLignes = new CommandeBuilder().ParDefautWithLignes().Build();
            commandeWithLignes = new CommandeBuilder().MinimumInfoWithLignes().Build();
        }

        [TestCategory("UpdateAvenantHangfireJobId")]
        [TestMethod]
        public void UpdateAvenantHangfireJobId_WithHangfireJobId_ShouldCallUpdateAvenant()
        {
            var commandeAvenantWithHangfireJobId = commandeAvenantBuilder.CommandeId(1).NumeroAvenant(1).HangfireJobId("12").Build();
            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeIdAndAvenantNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(commandeAvenantWithHangfireJobId);
            string hangfireJobId = "777";

            Invoking(() => Actual.UpdateAvenantHangfireJobId(1, 1, hangfireJobId)).Should().NotThrow();

            commandeAvenantWithHangfireJobId.HangfireJobId.Should().Be(hangfireJobId);
            commandeAvenantRepositoryFake.Verify(c => c.UpdateAvenant(It.IsAny<CommandeAvenantEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestCategory("ValideAvenant")]
        [TestMethod]
        public void ValideAvenant_WithAvenantCommandeWithValidationNull_Returns_AvenantCommandeValidated()
        {
            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeId(It.IsAny<int>())).Returns(new List<CommandeAvenantEnt>() { commandeAvenantWithValidationNull });

            Invoking(() => Actual.ValideAvenant(1)).Should().NotThrow();

            commandeAvenantWithValidationNull.DateValidation.Should().NotBeNull();
            commandeAvenantWithValidationNull.AuteurValidationId.Should().Be(utilisateurId);
            commandeAvenantRepositoryFake.Verify(c => c.UpdateAvenant(It.IsAny<CommandeAvenantEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestCategory("ValideAvenant")]
        [TestMethod]
        public void ValideAvenant_WithAvenantCommandeValidated_ThrowValidationException()
        {
            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeId(It.IsAny<int>())).Returns(new List<CommandeAvenantEnt>() { commandeAvenantWithValidationNotNull });

            Invoking(() => Actual.ValideAvenant(0)).Should().Throw<ValidationException>();
        }

        [TestCategory("GetCurrentAvenant")]
        [TestMethod]
        public void GetCurrentAvenant_WithAvenantCommandeWithDateValidationNull_Returns_SameAvenantCommande()
        {
            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeId(It.IsAny<int>())).Returns(new List<CommandeAvenantEnt>() { commandeAvenantWithValidationNull });

            Invoking(() => Actual.GetCurrentAvenant(1)).Should().NotThrow();

            commandeAvenantRepositoryFake.Verify(c => c.AddAvenant(It.IsAny<CommandeAvenantEnt>()), Times.Never);
            unitOfWorkFake.Verify(u => u.Save(), Times.Never);
        }

        [TestCategory("GetCurrentAvenant")]
        [TestMethod]
        public void GetCurrentAvenant_WithAvenantCommandeWithDateValidationNotNull_Returns_NewAvenantCommande()
        {
            var newCommandeAvenant = commandeAvenantBuilder.CommandeId(commandeAvenantWithValidationNotNull.CommandeId).NumeroAvenant(commandeAvenantWithValidationNotNull.NumeroAvenant + 1)
                .DateCreation(new DateTime(2019, 5, 2)).AuteurCreationId(2).Build();

            commandeAvenantRepositoryFake.Setup(c => c.GetAvenantByCommandeId(It.IsAny<int>())).Returns(new List<CommandeAvenantEnt>() { commandeAvenantWithValidationNotNull });

            Invoking(() => Actual.GetCurrentAvenant(1)).Should().NotThrow();

            newCommandeAvenant.CommandeId.Should().Be(commandeAvenantWithValidationNotNull.CommandeId);
            newCommandeAvenant.NumeroAvenant.Should().NotBe(commandeAvenantWithValidationNotNull.NumeroAvenant);
            newCommandeAvenant.AuteurCreationId.Should().NotBe(utilisateurId);
            newCommandeAvenant.DateValidation.Should().BeNull();
            newCommandeAvenant.AuteurValidationId.Should().BeNull();
            commandeAvenantRepositoryFake.Verify(c => c.AddAvenant(It.IsAny<CommandeAvenantEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }


        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("WithNullCommandeArgument")]
        public void SaveAvenant_WithNullCommandeArgument_ThrowArgumentException()
        {
            var commande = default(CommandeEnt);
            var avenant = default(CommandeAvenantEnt);
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commande);
            commandeAvenantSaveModel = commandeAvenantSaveModelBuilder.New();

            Invoking(() => Actual.SaveAvenant(commandeAvenantSaveModel, dateTraitement, out commande, out avenant)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("BusinessValidation")]
        public void SaveAvenant_WithAvenantModel_ShouldCallBusinessValidation()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(defaultCommande);
            commandeAvenantSaveModel = defaultcommandeAvenantSaveModelBuilder.Build();

            Actual.SaveAvenant(commandeAvenantSaveModel, dateTraitement, out defaultCommande, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("MapCommandeFieldsFromModel")]
        public void SaveAvenant_WithCommande_ShouldMapCommandeFieldsFromModel()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(defaultCommande);
            var commandeAvenantSaveModel = defaultcommandeAvenantSaveModelBuilder.Build();

            Actual.SaveAvenant(commandeAvenantSaveModel, dateTraitement, out defaultCommande, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            defaultCommande.CommentaireFournisseur.Should().Be(commandeAvenantSaveModel.CommentaireFournisseur);
            defaultCommande.CommentaireInterne.Should().Be(commandeAvenantSaveModel.CommentaireInterne);
            defaultCommande.DelaiLivraison.Should().Be(commandeAvenantSaveModel.DelaiLivraison);
            defaultCommande.IsAbonnement.Should().Be(commandeAvenantSaveModel.Abonnement.IsAbonnement);
            defaultCommande.FrequenceAbonnement.Should().Be(commandeAvenantSaveModel.Abonnement.Frequence);
            defaultCommande.DureeAbonnement.Should().Be(commandeAvenantSaveModel.Abonnement.Duree);
            defaultCommande.DateProchaineReception.Should().Be(commandeAvenantSaveModel.Abonnement.DateProchaineReception);
            defaultCommande.DatePremiereReception.Should().Be(commandeAvenantSaveModel.Abonnement.DatePremiereReception);
            defaultCommande.FournisseurAdresse.Should().Be(commandeAvenantSaveModel.Fournisseur.Adresse);
            defaultCommande.FournisseurCPostal.Should().Be(commandeAvenantSaveModel.Fournisseur.CodePostal);
            defaultCommande.FournisseurVille.Should().Be(commandeAvenantSaveModel.Fournisseur.Ville);
            defaultCommande.FournisseurPaysId.Should().Be(commandeAvenantSaveModel.Fournisseur.PaysId);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }


        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("AddLignesAvenantByModel")]
        public void SaveAvenant_WithNewAvenantLignesModel_ShouldCallAddCommandeLigne()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(defaultCommande);
            var commandeAvenantSaveModelCreatedLignes = commandeAvenantSaveModelBuilder.ParDefautWithCreatedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelCreatedLignes, dateTraitement, out defaultCommande, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.AddCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }


        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("UpdateLignesAvenantByModel")]
        public void SaveAvenant_WithAvenantLignesModel_ShouldCallUpdateCommandeLigneAndMapCommandeLigneFieldsFromModel()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(defaultCommandeWithLignes);
            var commandeAvenantSaveModelUpdatedLignes = commandeAvenantSaveModelBuilder.ParDefautWithUpdatedLignes().Build();
            var defaultCommandeLigne = defaultCommandeWithLignes.Lignes.FirstOrDefault();
            var commandeAvenantSaveLigneModelUpdatedLignes = commandeAvenantSaveModelUpdatedLignes.UpdatedLignes.FirstOrDefault();
            Actual.SaveAvenant(commandeAvenantSaveModelUpdatedLignes, dateTraitement, out defaultCommandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            defaultCommandeLigne.CommandeLigneId.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.CommandeLigneId);
            defaultCommandeLigne.NumeroLigne.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.NumeroLigne);
            defaultCommandeLigne.Libelle.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.Libelle);
            defaultCommandeLigne.TacheId.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.TacheId);
            defaultCommandeLigne.RessourceId.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.RessourceId);
            defaultCommandeLigne.UniteId.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.UniteId);
            defaultCommandeLigne.Quantite.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.Quantite);
            defaultCommandeLigne.PUHT.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.PUHT);
            defaultCommandeLigne.AuteurModificationId.Should().Be(utilisateurId);
            defaultCommandeLigne.AvenantLigne.IsDiminution.Should().Be(commandeAvenantSaveLigneModelUpdatedLignes.IsDiminution);
            commandeLignesRepositoryFake.Verify(c => c.UpdateCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }


        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("DeleteLignesAvenantByModel")]
        public void SaveAvenant_WithAvenantLignesModel_ShouldCallDeleteCommandeLigneAndDeleteCommandeLigneAvenant()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commandeWithLignes);
            var commandeAvenantSaveModelWithDeletedLignes = commandeAvenantSaveModelBuilder.ParDefautWithDeletedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelWithDeletedLignes, dateTraitement, out commandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.DeleteCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLigneAvenantRepository.Verify(c => c.DeleteCommandeLigneAvenant(It.IsAny<CommandeLigneAvenantEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }


        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("AddLignesAvenantModel and UpdateLignesAvenantModel")]
        public void SaveAvenant_WithLignesAvenantModel_ShouldCallAddCommandeLigneAndUpdateCommandeLigne()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commandeWithLignes);
            var commandeAvenantSaveModelWithCreatedLignesAndUpdatedLignes = commandeAvenantSaveModelBuilder.ParDefautWithCreatedAndUpdatedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelWithCreatedLignesAndUpdatedLignes, dateTraitement, out commandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.AddCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.UpdateCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.DeleteCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Never);
            commandeLigneAvenantRepository.Verify(c => c.DeleteCommandeLigneAvenant(It.IsAny<CommandeLigneAvenantEnt>()), Times.Never);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("AddLignesAvenantByModel and DeleteLignesAvenantByModel")]
        public void SaveAvenant_WithNewLignesAvenant_ShouldCallAddCommandeLigneAndDeleteCommandeLigne()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commandeWithLignes);
            var commandeAvenantSaveModelWithCreatedLignesAndDeletedLignes = commandeAvenantSaveModelBuilder.ParDefautWithCreatedAndDeletedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelWithCreatedLignesAndDeletedLignes, dateTraitement, out commandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.AddCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.UpdateCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Never);
            commandeLignesRepositoryFake.Verify(c => c.DeleteCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLigneAvenantRepository.Verify(c => c.DeleteCommandeLigneAvenant(It.IsAny<CommandeLigneAvenantEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("UpdateLignesAvenantByModel and DeleteLignesAvenantByModel")]
        public void SaveAvenant_WithLignesAvenantModel_ShouldCallUpdateCommandeLigneAndDeleteCommandeLigneAvenant()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commandeWithLignes);
            var commandeAvenantSaveModelWithUpdatedLignesAndDeletedLignes = commandeAvenantSaveModelBuilder.ParDefautWithdUpdatedAndDeletedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelWithUpdatedLignesAndDeletedLignes, dateTraitement, out commandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.AddCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Never);
            commandeLignesRepositoryFake.Verify(c => c.UpdateCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.DeleteCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLigneAvenantRepository.Verify(c => c.DeleteCommandeLigneAvenant(It.IsAny<CommandeLigneAvenantEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        [TestCategory("SaveAvenant")]
        [Description("AddLignesAvenantByModel and UpdateLignesAvenantByModel and DeleteLignesAvenantByModel")]
        public void SaveAvenant_WithLignesAvenantModel_ShouldCallAddCommandeLigneAndUpdateCommandeLigneAndDeleteCommandeLigneAvenant()
        {
            commandeRepositoryFake.Setup(c => c.GetCommandeWithCommandeLignes(It.IsAny<int>())).Returns(commandeWithLignes);
            var commandeAvenantSaveModelWithCreatedLignesAndUpdatedLignesAndDeletedLignes = commandeAvenantSaveModelBuilder.ParDefautWithCreatedUpdatedAndDeletedLignes().Build();

            Actual.SaveAvenant(commandeAvenantSaveModelWithCreatedLignesAndUpdatedLignesAndDeletedLignes, dateTraitement, out commandeWithLignes, out commandeAvenantWithValidationNull).Should().BeOfType<CommandeAvenantSave.ResultModel>();

            commandeLignesRepositoryFake.Verify(c => c.AddCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.UpdateCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLignesRepositoryFake.Verify(c => c.DeleteCommandeLigne(It.IsAny<CommandeLigneEnt>()), Times.Once);
            commandeLigneAvenantRepository.Verify(c => c.DeleteCommandeLigneAvenant(It.IsAny<CommandeLigneAvenantEnt>()), Times.Once);
            commandeAvenantSaveValidatorFake.Verify(c => c.Validate(It.IsAny<CommandeEnt>()), Times.Once);
            unitOfWorkFake.Verify(u => u.Save(), Times.Once);
        }

    }
}

