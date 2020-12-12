using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Groupe;
using Fred.Business.Organisation;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Fred.GroupSpecific.Default.Societe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Societe
{
    /// <summary>
    /// Classe de test de <see cref="DefaultSocieteManager"/>
    /// </summary>
    [TestClass]
    public class SocieteManagerTest : BaseTu<DefaultSocieteManager>
    {
        private readonly FakeDbSet<SocieteEnt> SocietesStub = new SocieteMocks().GetFakeDbSet();
        private readonly List<string> RZBetMBTP = new List<string> { "RB", "MBTP" };
        private readonly int typeOrganisationSociete = 4;
        private readonly OrganisationBuilder OrganisationBuilder = new OrganisationBuilder();
        private readonly UtilisateurBuilder UserBuilder = new UtilisateurBuilder();
        private readonly GroupeBuilder GroupeBuilder = new GroupeBuilder();
        private Mock<IOrganisationManager> OrgaManager;
        private List<OrganisationEnt> Organisations = new List<OrganisationEnt>();

        [TestInitialize]
        public void TestInitialize()
        {
            var context = GetMocked<FredDbContext>();

            Organisations = SocietesStub.Select(s => s.Organisation).ToList();
            context.Object.Societes = SocietesStub;
            context.Object.Organisations = OrganisationBuilder.BuildFakeDbSet(Organisations);
            context.Setup(c => c.Set<SocieteEnt>()).Returns(SocietesStub);
            context.Setup(c => c.Set<OrganisationEnt>()).Returns(OrganisationBuilder.BuildFakeDbSet(Organisations));
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);

            SubstituteConstructorArgument<IUnitOfWork>(uow);

            var societeRepositoryMock = new Mock<ISocieteRepository>();
            societeRepositoryMock.Setup(srm => srm.GetSocieteParentByOrgaId(It.IsAny<int>())).Returns(SocietesStub.Find(5));
            SubstituteConstructorArgument(societeRepositoryMock.Object);

            var userManager = GetMocked<IUtilisateurManager>();
            userManager.Setup(u => u.GetContextUtilisateurId()).Returns(UserBuilder.Prototype().UtilisateurId);

            var groupeManager = GetMocked<IGroupeManager>();
            groupeManager.Setup(u => u.FindById(It.IsAny<int>())).Returns(GroupeBuilder.RazelBec().Build());
            SubstituteConstructorArgument(groupeManager);

            OrgaManager = GetMocked<IOrganisationManager>();
            OrgaManager.Setup(o => o.GetTypeOrganisationIdByCode(It.IsAny<string>())).Returns(typeOrganisationSociete);
        }

        [TestMethod]
        [TestCategory("SocieteManager")]
        public void GetSocietesGroupesByUserHabibilitation_WithAdminHabilitation_Returns_FullList()
        {
            //Arrange
            var listOrgaLight = new List<OrganisationLightEnt>();
            Organisations.ForEach(o => listOrgaLight.Add(o.ToOrganisationLightEnt()));
            OrgaManager.Setup(o => o.GetOrganisationsAvailable(It.IsAny<string>(), It.IsAny<List<int>>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(listOrgaLight);
            //Act And Assert
            Actual.GetSocietesGroupesByUserHabibilitation().Should().HaveCount(SocietesStub.GroupBy(s => s.Groupe).Select(g => g.Key).Count());
        }

        [TestMethod]
        [TestCategory("SocieteManager")]
        public void GetSocietesGroupesByUserHabibilitation_WithHabilitationRZBetMBTP_Returns_RZBetMBTPgroupe()
        {
            //Arrange
            OrgaManager.Setup(o => o.GetOrganisationsAvailable(It.IsAny<string>(), It.IsAny<List<int>>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(new List<OrganisationLightEnt> {
                    OrganisationBuilder.OrganisationId((int)SocieteMocks.OrgaType.RZB).TypeOrganisationSociete().Build().ToOrganisationLightEnt(),
                    OrganisationBuilder.OrganisationId((int)SocieteMocks.OrgaType.MBTP).TypeOrganisationSociete().Build().ToOrganisationLightEnt()
                });
            //Act And Assert
            Actual.GetSocietesGroupesByUserHabibilitation().Should().HaveCount(SocietesStub.Where(s => RZBetMBTP.Contains(s.Code)).GroupBy(s => s.Groupe).Select(g => g.Key).Count());
        }
    }
}
