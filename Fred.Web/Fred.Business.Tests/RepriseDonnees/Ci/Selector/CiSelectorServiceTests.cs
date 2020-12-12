using System.Linq;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Selector;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.RepriseDonnees.Selector
{
    [TestClass]
    public class CiSelectorServiceTests
    {

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        private CiSelectorService CreateService()
        {
            return new CiSelectorService();
        }

        [TestMethod]
        public void GetCiOfDatabase_Si_un_seul_ci_pour_une_societe_retourne_le_ci_de_la_societe()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            RepriseExcelCi repriseExcelCi = RepriseCiFakeDataProvider.CreateExcelRows().FirstOrDefault();

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.GetCiOfDatabase(repriseExcelCi, context);

            // Assert
            Assert.AreEqual(result.CiId, OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB, "Le selecteur doit choisir le ci de la bonne societe.");
        }


        [TestMethod]
        public void GetCiOfDatabase_Si_un_seul_ci_pour_une_societe_retourne_le_ci_de_la_societe2()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            RepriseExcelCi repriseExcelCiRzb = RepriseCiFakeDataProvider.CreateExcelRowsWith2CiWithSameCode().First();

            RepriseExcelCi repriseExcelCiBianco = RepriseCiFakeDataProvider.CreateExcelRowsWith2CiWithSameCode().Last();

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateComplexeContext();

            // Act
            var resultRzb = unitUnderTest.GetCiOfDatabase(repriseExcelCiRzb, context);

            var resultBianco = unitUnderTest.GetCiOfDatabase(repriseExcelCiBianco, context);

            // Assert
            Assert.AreEqual(resultRzb.CiId, OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB, "Le selecteur doit choisir le ci de la bonne societe.");

            Assert.AreEqual(resultBianco.CiId, OrganisationTreeMocks.CI_ID_411100_SOCIETE_BIANCO, "Le selecteur doit choisir le ci de la bonne societe.");
        }
    }
}
