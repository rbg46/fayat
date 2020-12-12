using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Societe.Classification.Builder;
using Fred.Common.Tests.Data.Societe.Classification.Mock;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Societe.Classification;
using Fred.Entities.Societe.Classification;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Societe.Classification
{
    /// <summary>
    /// Test de la classe <see cref="SocieteClassificationRepository" />
    /// </summary>
    [TestClass]
    public class SocieteClassificationRepositoryTest : BaseTu<SocieteClassificationRepository>
    {
        private readonly SocieteClassificationMocks Mocks = new SocieteClassificationMocks();

        [TestInitialize]
        public void TestInitialize()
        {
            //Mocks
            var fakeContext = GetMocked<FredDbContext>();
            fakeContext.Setup(c => c.Set<SocieteClassificationEnt>()).Returns(Mocks.GetFakeDbSet());
            fakeContext.Object.Societes = new SocieteMocks().GetFakeDbSet();
            //uow instance
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(fakeContext.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(uow);
        }

        [TestMethod]
        [TestCategory("SocieteClassificationRepository")]
        public void GetOnlyActive_ReturnsList()
        {
            Actual.GetOnlyActive().Should().OnlyContain(c => c.Statut);
        }


        [TestMethod]
        [TestCategory("SocieteClassificationRepository")]
        public void GetByGroupeId_ReturnsList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .WhereContains(s => s.GroupeId == SocieteMocks.GroupeIdGRZB)
                .Build();
            Actual.GetByGroupeId(SocieteMocks.GroupeIdGRZB, true)
            .Should().HaveCount(classifications.Count());
        }

        [TestMethod]
        [TestCategory("SocieteClassificationRepository")]
        public void Search_ReturnsList()
        {
            //Arrange
            var textSearch = "Fayat";
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .WhereContains(s => s.Libelle.Contains(textSearch))
                .Build();
            Actual.Search(textSearch, 1, 20)
            .Should().HaveCount(classifications.Count());
        }

        [TestMethod]
        [TestCategory("SocieteClassificationRepository")]
        public void Search_WrongText_ReturnsEmptyList()
        {
            //Arrange
            var textSearch = "SQLI";
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .WhereContains(s => s.Libelle.Contains(textSearch))
                .Build();
            Actual.Search(textSearch, 1, 20)
            .Should().HaveCount(classifications.Count());
        }
    }
}
