using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.Commande.Services;
using Fred.Business.Societe;
using Fred.Common.Tests.Data.Agence.Builder;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.Entities.Adresse;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Models;
using Fred.ImportExport.Bootstrapper.Automappers;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.ImportExport.Business.Tests.Commande
{
    [TestClass]
    public class CommandeFluxManagerTest
    {
        private Mock<ISocieteManager> societeManagerMock = new Mock<ISocieteManager>();
        private Mock<IApplicationsSapManager> applicationsSapManagerMock = new Mock<IApplicationsSapManager>();
        private Mock<ICommandeFluxService> commandeFluxServiceMock = new Mock<ICommandeFluxService>();
        private Mock<ISepService> sepServiceMock = new Mock<ISepService>();
        private Mock<ICommandeStormImporter> commandeStormImporterMock = new Mock<ICommandeStormImporter>();
        private Mock<ICommandeFluxHelper> commandeFluxHelperMock = new Mock<ICommandeFluxHelper>();
        private CommandeFluxManager commandeFluxManager;

        private CommandeBuilder commandeBuilder = new CommandeBuilder();
        private FournisseurBuilder fournisseurBuilder = new FournisseurBuilder();
        private FournisseurEnt fournisseur;
        private AgenceBuilder agenceBuilder = new AgenceBuilder();
        private AgenceEnt agence;
        private int commandeId = 1;

        [TestInitialize]
        public void Initialize()
        {
            SetModelBuilder();
            SetMockService();

            commandeFluxManager = new CommandeFluxManager(AutoMapperConfig.CreateMapper(),
                societeManagerMock.Object,
                applicationsSapManagerMock.Object,
                commandeFluxServiceMock.Object,
                sepServiceMock.Object,
                commandeStormImporterMock.Object,
                commandeFluxHelperMock.Object);
        }

        [TestMethod]
        public async Task EnqueueExportCommandeJobToHangfireAsync_CommandeOrganisationDoesntExist_ShouldReturnResultFailure()
        {
            // Arrange
            string error = $"{FredImportExportBusinessResources.CommandeFluxMe21Error} {commandeId}";

            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(0);

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeJobAsync(commandeId);

            // Assert
            result.Should().BeEquivalentTo(Result<string>.CreateFailure(error));
        }

        [TestMethod]
        public async Task EnqueueExportCommandeJobToHangfireAsync_ParametersForSocieteDoesntExist_ShouldReturnResultFailure()
        {
            // Arrange
            string error = $"{FredImportExportBusinessResources.CommandeFluxMe21Error} {commandeId}";

            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(1);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "", Password = "", Url = "" });

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeJobAsync(commandeId);

            // Assert
            result.Should().BeEquivalentTo(Result<string>.CreateFailure(error));
        }

        [TestMethod]
        public async Task EnqueueExportCommandeJobToHangfireAsync_CommandeExistWithAllParam_ShouldReturnResultSuccessAndCallHelper()
        {
            // Arrange
            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(1);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeJobAsync(commandeId);

            // Assert
            commandeFluxHelperMock.Verify(h => h.EnqueueJob(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        }

        [TestMethod]
        public async Task EnqueueExportCommandeAvenantJobToHangfireAsync_CommandeOrganisationDoesntExist_ShouldReturnResultFailure()
        {
            // Arrange
            string error = $"{FredImportExportBusinessResources.CommandeFluxMe22Error} {commandeId}";

            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(0);

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeAvenantJobAsync(commandeId, 1);

            // Assert
            result.Should().BeEquivalentTo(Result<string>.CreateFailure(error));
        }

        [TestMethod]
        public async Task EnqueueExportCommandeAvenantJobToHangfireAsync_ParametersForSocieteDoesntExist_ShouldReturnResultFailure()
        {
            // Arrange
            string error = $"{FredImportExportBusinessResources.CommandeFluxMe22Error} {commandeId}";

            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(1);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "", Password = "", Url = "" });

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeAvenantJobAsync(commandeId, 1);

            // Assert
            result.Should().BeEquivalentTo(Result<string>.CreateFailure(error));
        }

        [TestMethod]
        public async Task EnqueueExportCommandeAvenantJobToHangfireAsync_CommandeExistWithAllParam_ShouldReturnResultSuccessAndCallHelper()
        {
            // Arrange
            commandeFluxServiceMock
               .Setup(s => s.GetOrganisationIdByCommandeIdAsync(It.IsAny<int>()))
               .ReturnsAsync(1);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            var result = await commandeFluxManager.EnqueueExportCommandeAvenantJobAsync(commandeId, 1);

            // Assert
            commandeFluxHelperMock.Verify(h => h.EnqueueJob(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        }

        [TestMethod]
        public void ExportCommandeToSap_ParametersForSocieteDoesntExist_ShouldThrowFredBusinessException()
        {
            // Arrange
            commandeFluxServiceMock
              .Setup(s => s.GetCommandeSAPAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(commandeBuilder.AddLignes(0)
                .CI(new CIEnt()
                {
                    CiId = 1,
                    Organisation = new OrganisationEnt() { OrganisationId = 1 }
                }).Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "", Password = "", Url = "" });

            // Act
            commandeFluxManager
                .Awaiting(m => m.ExportCommandeToSap(commandeId))
                .Should()
                .Throw<FredIeBusinessException>()
                .WithInnerException<FredBusinessException>()
                .WithMessage("La commande n'a pas pu être validée, veuillez contacter le support.");
        }

        [TestMethod]
        public async Task ExportCommandeToSap_DefaultCommande_ShouldCallHelper()
        {
            // Arrange
            commandeFluxServiceMock
              .Setup(s => s.GetCommandeSAPAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(commandeBuilder.AddLignes(0)
                .CI(new CIEnt()
                {
                    CiId = 1,
                    Organisation = new OrganisationEnt() { OrganisationId = 1 }
                }).Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            var result = await commandeFluxManager.ExportCommandeToSap(commandeId);

            // Assert
            commandeFluxHelperMock.Verify(h => h.SendJob(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ExportCommandeToSap_CommandeWithFournisseurAgence_ShouldReturnCommandeSapModel()
        {
            // Arrange
            commandeBuilder
                .Fournisseur(fournisseur)
                .Agence(agence)
                .FournisseurAdresse("9 Chemin de Halage")
                .FournisseurCPostal("76000")
                .FournisseurVille("ROUEN")
                .AddLignes(0)
                .CI(new CIEnt()
                {
                    CiId = 1,
                    Organisation = new OrganisationEnt() { OrganisationId = 1 }
                });

            commandeFluxServiceMock
              .Setup(s => s.GetCommandeSAPAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(commandeBuilder.Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            CommandeSapModel result = await commandeFluxManager.ExportCommandeToSap(commandeId);

            // Assert
            result
                .Should()
                .Match<CommandeSapModel>(c => c.AgenceCode == agence.Code
                    && c.AgenceAdresse == agence.Adresse.Ligne
                    && c.AgenceCPostal == agence.Adresse.CodePostal
                    && c.AgenceVille == agence.Adresse.Ville
                    && c.FournisseurCode == agence.Fournisseur.Code
                    && c.FournisseurAdresse == agence.Fournisseur.Adresse
                    && c.FournisseurCPostal == agence.Fournisseur.CodePostal
                    && c.FournisseurVille == agence.Fournisseur.Ville);
        }

        [TestMethod]
        public async Task ExportCommandeToSap_CommandeWithFournisseurAgenceAndEditedAdresse_ShouldReturnCommandeSapModel()
        {
            // Arrange         
            var commande = commandeBuilder
                .Fournisseur(fournisseur)
                .Agence(agence)
                .FournisseurAdresse("9 Chemin de Halage")
                .FournisseurCPostal("76120")
                .FournisseurVille("SOTTEVILLE LES ROUEN")
                .AddLignes(0)
                .CI(new CIEnt()
                {
                    CiId = 1,
                    Organisation = new OrganisationEnt() { OrganisationId = 1 }
                }).Build();

            commandeFluxServiceMock
              .Setup(s => s.GetCommandeSAPAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(commande);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            CommandeSapModel result = await commandeFluxManager.ExportCommandeToSap(commandeId);

            // Assert
            result
                .Should()
                .Match<CommandeSapModel>(c => c.AgenceCode == agence.Code
                    && c.AgenceAdresse == commande.FournisseurAdresse
                    && c.AgenceCPostal == commande.FournisseurCPostal
                    && c.AgenceVille == commande.FournisseurVille
                    && c.FournisseurCode == agence.Fournisseur.Code
                    && c.FournisseurAdresse == agence.Fournisseur.Adresse
                    && c.FournisseurCPostal == agence.Fournisseur.CodePostal
                    && c.FournisseurVille == agence.Fournisseur.Ville);
        }

        [TestMethod]
        public void ExportCommandeAvenantToSap_ParametersForSocieteDoesntExist_ShouldThrowFredBusinessException()
        {
            // Arrange
            commandeFluxServiceMock
                .Setup(s => s.GetCommandeAvenantSAPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(commandeBuilder.AddLignes(0)
                    .CI(new CIEnt()
                    {
                        CiId = 1,
                        Organisation = new OrganisationEnt() { OrganisationId = 1 }
                    }).Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "", Password = "", Url = "" });

            // Act
            commandeFluxManager
                .Awaiting(m => m.ExportCommandeAvenantToSap(commandeId, 1))
                .Should()
                .Throw<FredIeBusinessException>()
                .WithInnerException<FredBusinessException>()
                .WithMessage("La commande n'a pas pu être validée, veuillez contacter le support.");
        }

        [TestMethod]
        public async Task ExportCommandeAvenantToSap_DefaultCommande_ShouldCallHelper()
        {
            // Arrange
            commandeFluxServiceMock
                .Setup(s => s.GetCommandeAvenantSAPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(commandeBuilder.AddLignes(0)
                    .CI(new CIEnt()
                    {
                        CiId = 1,
                        Organisation = new OrganisationEnt() { OrganisationId = 1 }
                    }).Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            var result = await commandeFluxManager.ExportCommandeAvenantToSap(commandeId, 1);

            // Assert
            commandeFluxHelperMock.Verify(h => h.SendJob(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ExportCommandeAvenantToSap_CommandeWithFournisseurAgence_ShouldReturnCommandeAvenantSapModel()
        {
            // Arrange
            commandeBuilder
                .Fournisseur(fournisseur)
                .Agence(agence)
                .FournisseurAdresse("9 Chemin de Halage")
                .FournisseurCPostal("76000")
                .FournisseurVille("ROUEN")
                .AddLignes(0)
                .CI(new CIEnt()
                {
                    CiId = 1,
                    Organisation = new OrganisationEnt() { OrganisationId = 1 }
                });

            commandeFluxServiceMock
                .Setup(s => s.GetCommandeAvenantSAPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(commandeBuilder.Build());
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            var result = await commandeFluxManager.ExportCommandeAvenantToSap(commandeId, 1);

            // Assert
            result
                .Should()
                .Match<CommandeAvenantSapModel>(c => c.AgenceCode == agence.Code
                    && c.AgenceAdresse == agence.Adresse.Ligne
                    && c.AgenceCPostal == agence.Adresse.CodePostal
                    && c.AgenceVille == agence.Adresse.Ville
                    && c.FournisseurCode == agence.Fournisseur.Code
                    && c.FournisseurAdresse == agence.Fournisseur.Adresse
                    && c.FournisseurCPostal == agence.Fournisseur.CodePostal
                    && c.FournisseurVille == agence.Fournisseur.Ville);
        }

        [TestMethod]
        public async Task ExportCommandeAvenantToSap_CommandeWithFournisseurAgenceAndEditedAdresse_ShouldReturnCommandeAvenantSapModel()
        {
            // Arrange
            var commande = commandeBuilder
               .Fournisseur(fournisseur)
               .Agence(agence)
               .FournisseurAdresse("9 Chemin de Halage")
               .FournisseurCPostal("76120")
               .FournisseurVille("SOTTEVILLE LES ROUEN")
               .AddLignes(0)
               .CI(new CIEnt()
               {
                   CiId = 1,
                   Organisation = new OrganisationEnt() { OrganisationId = 1 }
               }).Build();

            commandeFluxServiceMock
              .Setup(s => s.GetCommandeAvenantSAPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(commande);
            applicationsSapManagerMock
                .Setup(m => m.GetParametersForSociete(It.IsAny<int>()))
                .Returns(new ApplicationSapParameter() { Login = "login", Password = "password", Url = "url" });

            // Act
            CommandeAvenantSapModel result = await commandeFluxManager.ExportCommandeAvenantToSap(commandeId, 1);

            // Assert
            result
                .Should()
                .Match<CommandeAvenantSapModel>(c => c.AgenceCode == agence.Code
                    && c.AgenceAdresse == commande.FournisseurAdresse
                    && c.AgenceCPostal == commande.FournisseurCPostal
                    && c.AgenceVille == commande.FournisseurVille
                    && c.FournisseurCode == agence.Fournisseur.Code
                    && c.FournisseurAdresse == agence.Fournisseur.Adresse
                    && c.FournisseurCPostal == agence.Fournisseur.CodePostal
                    && c.FournisseurVille == agence.Fournisseur.Ville);
        }

        private void SetMockService()
        {
            societeManagerMock
              .Setup(m => m.GetSocieteParentByOrgaId(It.IsAny<int>()))
              .Returns(new SocieteEnt() { SocieteId = 1 });

            commandeFluxHelperMock
                .Setup(h => h.EnqueueJob(It.IsAny<Expression<Func<Task>>>()))
                .Returns(Guid.NewGuid().ToString());

            commandeFluxHelperMock
              .Setup(h => h.SendJob(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new HttpResponseMessage()
              {
                  Content = new StringContent(string.Empty)
              });

            sepServiceMock
               .Setup(s => s.IsSepAsync(It.IsAny<SocieteEnt>()))
               .ReturnsAsync(false);
        }

        private void SetModelBuilder()
        {
            fournisseur = fournisseurBuilder
                            .Id(1)
                            .Code("LOXAM")
                            .Adresse("Rue Nicolas Coatanlem")
                            .CodePostal("56850")
                            .Ville("CAUDAN")
                            .Build();

            agence = agenceBuilder
                .Id(1)
                .Code("LOXAM ROUEN")
                .Fournisseur(fournisseur)
                .Adresse(new AdresseEnt()
                {
                    AdresseId = 1,
                    Ligne = "9 Chemin de Halage",
                    CodePostal = "76000",
                    Ville = "ROUEN"
                })
                .Build();
        }
    }
}
