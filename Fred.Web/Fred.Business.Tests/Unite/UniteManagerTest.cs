using FluentAssertions;
using Fred.Business.Unite;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Unite.Builder;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Unite
{
    /// <summary>
    /// Classe de test de <see cref=""/>
    /// </summary>
    [TestClass]
    public class UniteManagerTest : BaseTu<UniteManager>
    {
        [TestMethod]
        [TestCategory("UniteManager")]
        [TestCategory("OperationDiverseExcelService")]
        public void SearchLight_Returns_NotEmptyList()
        {
            var builder = new UniteBuilder();
            var context = GetMocked<FredDbContext>();
            context.Setup(c => c.Set<UniteEnt>()).Returns(builder.BuildFakeDbSet(builder.New()));
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(uow);

            var uniteRepositoryMock = new Mock<IUniteRepository>();
            uniteRepositoryMock.Setup(urm => urm.Query()).Returns(new RepositoryQuery<UniteEnt>(new DbRepository<UniteEnt>(context.Object)));
            SubstituteConstructorArgument(uniteRepositoryMock.Object);

            Actual.SearchLight(string.Empty, 1, int.MaxValue).Should().NotBeEmpty();
        }
    }
}
