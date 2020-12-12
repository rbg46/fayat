using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.Journal;
using Fred.Business.OperationDiverse;
using Fred.Business.Referential.Nature;
using Fred.Common.Tests;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Test.OperationDiverse
{
    /// <summary>
    /// Test de classe de <see cref="FamilleOperationDiverseExportExcelManager"/>
    /// </summary>
    [TestClass]
    public class FamilleOperationDiverseExportExcelManagerTest : BaseTu<FamilleOperationDiverseExportExcelManager>
    {
        private readonly Mock<IUnitOfWork> UnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IFamilleOperationDiverseRepository> FamilleOperationDiverseRepository = new Mock<IFamilleOperationDiverseRepository>();
        private readonly List<FamilleOperationDiverseEnt> FamilleOperationDiverseMockList = new List<FamilleOperationDiverseEnt>();
        private Mock<IJournalManager> fakeJournalManager;
        private Mock<INatureManager> fakeNatureManager;
        FamilleOperationDiverseExportExcelManager FamilleOperationDiverseExportExcelManager;

        /// <summary>
        /// Initialisation des tests avec les mocks
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            fakeJournalManager = GetMocked<IJournalManager>();
            fakeNatureManager = GetMocked<INatureManager>();

            //setup repositories
            FamilleOperationDiverseRepository.Setup(la => la.GetFamilyBySociety(1))
                .Returns(FamilleOperationDiverseMockList);

            FamilleOperationDiverseExportExcelManager = new FamilleOperationDiverseExportExcelManager(
                UnitOfWork.Object,
                FamilleOperationDiverseRepository.Object,
                fakeNatureManager.Object,
                fakeJournalManager.Object);

        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseExportExcelManager")]
        public void GenerateFamilleJournalExcel_ForFamille_SocieteNull()
        {
            //Act
            Invoking(() => FamilleOperationDiverseExportExcelManager.GetExportExcelForJournal(0, "Famille"))
            //Assert
            .Should().Throw<FredBusinessException>().Which.Message.Should().Be(
                FeatureExportExcel.Invalid_FileFormat
            );
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseExportExcelManager")]
        public void GenerateFamilleJournalExcel_ForNature_SocieteNull()
        {
            //Act
            Invoking(() => FamilleOperationDiverseExportExcelManager.GetExportExcelForJournal(0, "Nature"))
            //Assert
            .Should().Throw<FredBusinessException>().Which.Message.Should().Be(
                FeatureExportExcel.Invalid_FileFormat
            );
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseExportExcelManager")]
        public void GenerateFamilleJournalExcel_ForJournal_SocieteNull()
        {
            //Act
            Invoking(() => FamilleOperationDiverseExportExcelManager.GetExportExcelForJournal(0, "Journal"))
            //Assert
            .Should().Throw<FredBusinessException>().Which.Message.Should().Be(
                FeatureExportExcel.Invalid_FileFormat
            );
        }
    }
}
