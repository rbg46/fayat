using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.CI.Services;
using Fred.Common.Tests.Data.Fonctionnalite.Mock;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Common.Tests.Data.Permissions.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Fonctionnalite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Role.Services
{
    [TestClass]
    public class RoleValidsForPermissionServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IRoleRepository> mockRoleRepository;
        private Mock<IFonctionnaliteRepository> mockFonctionnaliteRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRoleRepository = this.mockRepository.Create<IRoleRepository>();
            this.mockFonctionnaliteRepository = this.mockRepository.Create<IFonctionnaliteRepository>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private RoleValidsForPermissionService CreateService()
        {
            return new RoleValidsForPermissionService(
                this.mockRoleRepository.Object,
                this.mockFonctionnaliteRepository.Object);
        }

        [TestMethod]
        public void GetRoleIdsValidsForPermission_Si_La_Fonctionnalite_Est_Desactive_Et_Que_C_est_Le_Seule_Fonctionnalite_Qui_As_Cette_Permission_Alors_Aucun_Role_N_est_Valide_Pour_Cette_Permission()
        {
            // Arrange
            var service = this.CreateService();

            int permissionIdRequested = PermissionMock.GetPermissionToShowCiList().PermissionId;

            int societeId = OrganisationTreeMocks.SOCIETE_ID_RZB;

            var fonctionnaliteCentreImputation = FonctionnaliteDataMocks.GetCentreImputationFonctionnalite();

            FonctionnaliteInactiveResponse fonctionnaliteInactiveResponse = new FonctionnaliteInactiveResponse()
            {
                FonctionnaliteId = fonctionnaliteCentreImputation.FonctionnaliteId,
                SocieteId = societeId
            };

            FonctionnaliteForPermissionResponse fonctionnalitesForPermissionResponse = new FonctionnaliteForPermissionResponse()
            {
                FonctionnaliteId = fonctionnaliteCentreImputation.FonctionnaliteId,
                SocieteId = societeId,
                RoleId = Fred.Common.Tests.Data.Role.Mock.RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER
            };

            FonctionnaliteTypeMode modesAutorized = FonctionnaliteTypeMode.Read;

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesInactives(It.IsAny<List<int>>()))
                                             .Returns(new List<FonctionnaliteInactiveResponse>() { fonctionnaliteInactiveResponse });

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesForPermission(It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<FonctionnaliteTypeMode>>()))
                                                    .Returns(new List<FonctionnaliteForPermissionResponse>() { fonctionnalitesForPermissionResponse });

            // Act
            var result = service.GetRoleIdsValidsForSocieteAndPermission(societeId, permissionIdRequested, modesAutorized);

            // Assert
            result.Should().BeEmpty(because: "Si la fonctionnalite est desactive et que c'est le seule fonctionnalite qui as cette permission, alors aucun role n'est valide pour cette permission");
        }

        [TestMethod]
        public void GetRoleIdsValidsForPermission_Si_La_Fonctionnaliten_N_Est_Pas_Desactivée_Et_Que_C_est_Le_Seule_Fonctionnalite_Qui_As_Cette_Permission_Alors_Un_Role_Est_Valide_Pour_Cette_Permission_()
        {
            // Arrange
            var service = this.CreateService();

            int permissionIdRequested = PermissionMock.GetPermissionToShowCiList().PermissionId;

            int societeId = OrganisationTreeMocks.SOCIETE_ID_RZB;

            var fonctionnaliteCentreImputation = FonctionnaliteDataMocks.GetCentreImputationFonctionnalite();

            FonctionnaliteForPermissionResponse fonctionnalitesForPermissionResponse = new FonctionnaliteForPermissionResponse()
            {
                FonctionnaliteId = fonctionnaliteCentreImputation.FonctionnaliteId,
                SocieteId = societeId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER
            };

            FonctionnaliteTypeMode modesAutorized = FonctionnaliteTypeMode.Read;

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesInactives(It.IsAny<List<int>>()))
                                             .Returns(new List<FonctionnaliteInactiveResponse>() { });

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesForPermission(It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<FonctionnaliteTypeMode>>()))
                                                    .Returns(new List<FonctionnaliteForPermissionResponse>() { fonctionnalitesForPermissionResponse });

            // Act
            var result = service.GetRoleIdsValidsForSocieteAndPermission(societeId, permissionIdRequested, modesAutorized);

            // Assert
            result.Should().HaveCount(1, because: "Si la fonctionnaliten n est pas desactivée et que c'est le seule fonctionnalite qui as cette permission, alors un role est valide pour cette permission");
        }

        [TestMethod]
        public void GetRoleIdsValidsForPermission_Si_La_Fonctionnaliten_N_Est_Pas_Desactivee_Et_Que_Deux_Roles_Ont_Cette_Fonctionnalite_Alors_Deux_Roles_Sont_Valides_Pour_Cette_Permission()
        {
            // Arrange
            var service = this.CreateService();

            int permissionIdRequested = PermissionMock.GetPermissionToShowCiList().PermissionId;

            int societeId = OrganisationTreeMocks.SOCIETE_ID_RZB;

            var fonctionnaliteCentreImputation = FonctionnaliteDataMocks.GetCentreImputationFonctionnalite();

            var fonctionnalitesForPermissionResponse1 = new FonctionnaliteForPermissionResponse()
            {
                FonctionnaliteId = fonctionnaliteCentreImputation.FonctionnaliteId,
                SocieteId = societeId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER
            };

            var fonctionnalitesForPermissionResponse2 = new FonctionnaliteForPermissionResponse()
            {
                FonctionnaliteId = fonctionnaliteCentreImputation.FonctionnaliteId,
                SocieteId = societeId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX
            };

            FonctionnaliteTypeMode modesAutorized = FonctionnaliteTypeMode.Read;

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesInactives(It.IsAny<List<int>>()))
                                             .Returns(new List<FonctionnaliteInactiveResponse>());

            this.mockFonctionnaliteRepository.Setup(x => x.GetFonctionnalitesForPermission(It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<FonctionnaliteTypeMode>>()))
                                                    .Returns(new List<FonctionnaliteForPermissionResponse>() {
                                                        fonctionnalitesForPermissionResponse1,
                                                        fonctionnalitesForPermissionResponse2
                                                    });

            // Act
            var result = service.GetRoleIdsValidsForSocieteAndPermission(societeId, permissionIdRequested, modesAutorized);

            // Assert
            result.Should().HaveCount(2, because: "Si la fonctionnaliten n est pas desactivee et que deux roles ont cette fonctionnalite, alors deux roles sont valides pour cette permission");
        }
    }
}
