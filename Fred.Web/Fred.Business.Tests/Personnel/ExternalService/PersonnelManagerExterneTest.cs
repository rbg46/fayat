using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.CI.Services;
using Fred.Business.ExternalService.Personnel;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.Entities.Organisation.Tree;
using Fred.Web.Models.Societe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Personnel.ExternalService
{
    /// <summary>
    /// Classe de test du service <see cref="PersonnelManagerExterne"/>
    /// </summary>
    [TestClass]
    public class PersonnelManagerExterneTest : BaseTu<PersonnelManagerExterne>
    {
        private UtilisateurBuilder UserBuilder = new UtilisateurBuilder();
        private SocieteModelBuilder SocieteBuilder = new SocieteModelBuilder();
        private Mock<IUtilisateurManager> UtilisateurManager;

        [TestInitialize]
        public void TestInitialize()
        {
            UtilisateurManager = GetMocked<IUtilisateurManager>();
        }

        [TestMethod]
        [TestCategory("PersonnelManagerExterne")]
        public void ExportReceptionInterimaires_NotThrowException()
        {
            UtilisateurManager.Setup(u => u.GetContextUtilisateurAsync()).Returns(Task.FromResult(UserBuilder.Prototype()));
            Invoking(async () => await Actual.ExportReceptionInterimairesAsync(new List<SocieteModel>())).Should().NotThrow<Exception>();
        }

        [TestMethod]
        [TestCategory("PersonnelManagerExterne")]
        public void ExportReceptionInterimaires_WhenSuperAdmin_NotThrowFredBusinessException()
        {
            UtilisateurManager.Setup(u => u.GetContextUtilisateurAsync()).Returns(Task.FromResult(UserBuilder.SuperAdmin()));
            Invoking(async () => await Actual.ExportReceptionInterimairesAsync(new List<SocieteModel>())).Should().NotThrow<Exception>();
        }

        [TestMethod]
        [TestCategory("PersonnelManagerExterne")]
        public void ExportReceptionInterimaires_WhenInputNotEmpty_NotThrowException()
        {
            UtilisateurManager.Setup(u => u.GetContextUtilisateurAsync()).Returns(Task.FromResult(UserBuilder.Prototype()));
            var ciAccessiblesService = GetMocked<ICisAccessiblesService>();
            ciAccessiblesService.Setup(s => s.GetCisAccessiblesForUserAndPermission(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<OrganisationBase>());
            ciAccessiblesService.Setup(s => s.GetCiIdsAvailablesForReceptionInterimaire(It.IsAny<IEnumerable<int>>()))
                .Returns(new List<int>());
            var ciManager = GetMocked<ICIManager>();
            ciManager.Setup(m => m.GetCiIdListBySocieteId(It.IsAny<int>()))
                .Returns(new List<int>());
            Invoking(async () => await Actual.ExportReceptionInterimairesAsync(SocieteBuilder.BuildFakeDbSet(
                SocieteBuilder.Prototype()
            ).ToList())).Should().NotThrow<Exception>();
        }
    }
}
