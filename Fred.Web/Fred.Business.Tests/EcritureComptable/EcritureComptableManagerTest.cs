using AutoMapper;
using FluentAssertions;
using Fred.Business.EcritureComptable;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.EcritureComptable;
using Fred.Common.Tests.Data.OperationDiverse.Fake;
using Fred.DataAccess.Common;
using Fred.DataAccess.EcritureComptable;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Fred.Business.Tests.EcritureComptable
{
    [Ignore]
    [TestClass]
    public class EcritureComptableManagerTest : BaseTu<EcritureComptableManager>
    {

        private Mock<IUnitOfWork> uowMock;
        private Mock<FredDbContext> context;

        private readonly EcritureComptableBuilder ecritureComptableBuilder = new EcritureComptableBuilder();
        private Mock<IEcritureComptableRepository> ecritureComptableRepositoryMock;
        private EcritureComptableRepository EcritureComptableRepositoryReal;

        [TestInitialize]
        public void Initialize()
        {
            context = GetMocked<FredDbContext>();
            uowMock = GetMocked<IUnitOfWork>();

            ecritureComptableRepositoryMock = GetMocked<IEcritureComptableRepository>();
            ecritureComptableRepositoryMock.Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<MonthLimits>())).Returns<int, MonthLimits>((i, m) => EcritureComptableRepositoryReal.GetAsync(i, m));


            IMapper fakeMapper = new OperationDiverseFake().FakeMapper;
            SubstituteConstructorArgument<IMapper>(fakeMapper);
        }

        private void GenerateFakeDatabase(List<EcritureComptableEnt> ecritureComptableEnts)
        {
            Mock<ISecurityManager> securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(context.Object, securityManager.Object);
            context.Object.EcritureComptables = ecritureComptableBuilder.BuildFakeDbSet(ecritureComptableEnts);
            EcritureComptableRepositoryReal = new EcritureComptableRepository(null, uow, context.Object);
        }

        private void GenerateFakeDatabase()
        {
            Mock<ISecurityManager> securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(context.Object, securityManager.Object);
            EcritureComptableRepositoryReal = new EcritureComptableRepository(null, uow, context.Object);
        }


        [TestMethod]
        [TestCategory("EcritureComptableManager")]
        public async System.Threading.Tasks.Task GetAllByCiIdAndDateComptable_WithUniqueCiIdAndUniqueDateComptable_ReturnAllEcritureComptableAsync()
        {
            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt>{
                ecritureComptableBuilder.EcritureComptableId(1).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                ecritureComptableBuilder.EcritureComptableId(2).DateComptable(new DateTime(2020, 1, 1)).CiId(2).Build()
            };

            GenerateFakeDatabase(ecritureComptableEnts);
            (await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2020, 1, 1)).ConfigureAwait(false)).Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("EcritureComptableManager")]
        public async System.Threading.Tasks.Task GetAllByCiIdAndDateComptable_WithUniqueCiIdAndUniqueDateComptable_ReturnNoneEcritureComptableAsync()
        {
            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt>{
                ecritureComptableBuilder.EcritureComptableId(1).DateComptable(new DateTime(2020, 1, 1)).CiId(2).Build(),
                ecritureComptableBuilder.EcritureComptableId(2).DateComptable(new DateTime(2020, 1, 1)).CiId(2).Build()
            };

            GenerateFakeDatabase(ecritureComptableEnts);
            (await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2020, 1, 1)).ConfigureAwait(false)).Should().HaveCount(0);
        }

    }
}
