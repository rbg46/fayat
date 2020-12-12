using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.Organisation.Tree;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Mock;
using Fred.Common.Tests.Data.Organisation.Mock;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.CI;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Fred.GroupSpecific.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.CI
{
    /// <summary>
    /// Classe de tes de <see cref="DefaultCIManager"/>
    /// </summary>
    [TestClass]
    public class CiManagerTest : BaseTu<DefaultCIManager>
    {
        private readonly CIValidator Validator = new CIValidator();
        private readonly CiMocks CiMocks = new CiMocks();
        private readonly SocieteMocks SocieteMocks = new SocieteMocks();
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();
        private readonly UtilisateurBuilder UserBuilder = new UtilisateurBuilder();
        private List<CIEnt> Cis;

        [TestInitialize]
        public void TestInitialize()
        {
            Cis = CiMocks.GetFakeDbSet().ToList();
            var context = GetMocked<FredDbContext>();
            context.Setup(c => c.Set<CIEnt>()).Returns(CiMocks.GetFakeDbSet());
            context.Setup(c => c.Set<SocieteEnt>()).Returns(SocieteMocks.GetFakeDbSet());
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<ICIValidator>(Validator);
            SubstituteConstructorArgument<IUnitOfWork>(uow);
            SubstituteConstructorArgument<ICIRepository>(new CIRepository(context.Object, null));
            var utilisateurManager = GetMocked<IUtilisateurManager>();
            utilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(UserBuilder.Prototype());
            utilisateurManager.Setup(u => u.GetContextUtilisateurAsync()).ReturnsAsync(UserBuilder.Prototype());
            utilisateurManager.Setup(u => u.GetContextUtilisateurId()).Returns(UserBuilder.Prototype().UtilisateurId);
            utilisateurManager.Setup(u => u.IsSuperAdmin(It.IsAny<int>())).Returns(true);
            var CiIds = Cis.Select(c => c.CiId).ToList();
            utilisateurManager.Setup(u => u.GetAllCIbyUser(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int?>())).Returns(CiIds);
            utilisateurManager.Setup(u => u.GetAllCIbyUserAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int?>())).ReturnsAsync(CiIds);
            var organisationTreeService = GetMocked<IOrganisationTreeService>();
            organisationTreeService.Setup(o => o.GetOrganisationTree(It.IsAny<bool>())).Returns(new Fred.Entities.Organisation.Tree.OrganisationTree(OrganisationBaseMocks.GetOrganisationBases()));
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public async Task SearchLight_WithDot_ReturnsExpectedResult()
        {
            (await Actual.SearchLightAsync(".", 1, 25).ConfigureAwait(false)).Should().HaveCount(Cis.Where(c => c.Code.Contains(".")).Count());
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public async Task SearchLight_WithSocieteId_WithDot_ReturnsExpectedResult()
        {
            (await Actual.SearchLightAsync(".", 1, 25, 1).ConfigureAwait(false)).Should().HaveCount(Cis.Where(c => c.Code.Contains(".")).Count());
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public void SearchLightCompteInterneSep_WithDot_ReturnsExpectedResult()
        {
            var societeManager = GetMocked<ISocieteManager>();
            societeManager.Setup(m => m.GetSocieteByOrganisationIdEx(It.IsAny<int>(), It.IsAny<bool>())).Returns(SocietyBuilder.Prototype());
            var sepService = GetMocked<ISepService>();
            sepService.Setup(s => s.GetSocieteGerante(It.IsAny<int>())).Returns(SocietyBuilder.Prototype());

            Actual.SearchLightCompteInterneSep(".", 1, 25, Cis.Select(c => c.CiId).FirstOrDefault()).Should().HaveCount(Cis.Where(c => c.Code.Contains(".")).Count());
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public void CiSearchLightForAffectationMoyen_WithDot_ReturnsExpectedResult()
        {
            Actual.CiSearchLightForAffectationMoyen(".", 1, 25).Should().HaveCount(Cis.Where(c => c.Code.Contains(".")).Count());
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public async Task SearchLightByPersonnelId_WithDot_ReturnsExpectedResult()
        {
            (await Actual.SearchLightByPersonnelId(".", 1, 25, 1).ConfigureAwait(false)).Should().HaveCount(Cis.Where(c => c.Code.Contains(".")).Count());
        }

        [TestMethod]
        [TestCategory("CIManager")]
        public void GetCisOfUserFilteredBySocieteId_Returns()
        {
            Actual.GetCisOfUserFilteredBySocieteId("test", 1, 20, 1).Should().NotBeNull();
        }
    }
}
