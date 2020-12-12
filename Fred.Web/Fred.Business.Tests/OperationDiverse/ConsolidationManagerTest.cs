using FluentAssertions;
using Fred.Business.OperationDiverse;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.OperationDiverse.Builder;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.Tests.OperationDiverse
{
    /// <summary>
    /// Classe de test de <see cref="ConsolidationManager"/>
    /// </summary>
    [TestClass]
    public class ConsolidationManagerTest : BaseTu<ConsolidationManager>
    {
        private FamilleOperationDiverseBuilder FamilleOperationDiverseBuilder = new FamilleOperationDiverseBuilder();

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<IFamilleOperationDiverseManager> manager = GetMocked<IFamilleOperationDiverseManager>();
            manager.Setup(m => m.GetFamiliesBySociety(It.IsAny<int>())).Returns(FamilleOperationDiverseBuilder.BuildFakeDbSet(
                FamilleOperationDiverseBuilder.Code("00").Libelle("Lib01").Build(),
                FamilleOperationDiverseBuilder.Code("01").Libelle("Lib02").Build(),
                FamilleOperationDiverseBuilder.Code("02").Libelle("Lib03").Build())
            );
        }

        [Ignore]
        [TestMethod]
        [TestCategory("ConsolidationManager")]
        public void GetConsolidationDatas_WithIncorrectsInputs_ThrowException()
        {
            /* Invoking(() => Actual.GetConsolidationDatas(0, new DateTime(), new DateTime()))
                 .Should().Throw<Exception>();*/
            //Action
            Task<IDictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel>> result = Actual.GetConsolidationDatasAsync(0, new DateTime(), new DateTime());
            result.Exception.Message.Should().BeNull();
            Invoking(() => Actual.GetConsolidationDatasAsync(0, new DateTime(), new DateTime()))
                .Should().Throw<Exception>();
        }
    }
}
