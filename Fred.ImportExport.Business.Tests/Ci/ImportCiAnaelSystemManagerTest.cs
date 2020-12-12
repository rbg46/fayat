using FluentAssertions;
using Fred.Business;
using Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor;
using Fred.Common.Tests;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.AnaelSystem;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.CI
{
    [TestClass]
    public class ImportCiAnaelSystemManagerTest : BaseTu<ImportCiAnaelSystemManager>
    {
        [TestInitialize]
        public void Initialize()
        {
            //J'utilise le vrai extractor service
            SubstituteProperty<ICiExtractorService>(new CiExtractorService());
        }

        [TestMethod]
        public void ImportCiFromAnaelAndSendToSap_WithInvalidExcelFile_ThrowException()
        {
            //Arrange
            ImportCisByExcelInputs input = new ImportCisByExcelInputs
            {
                GroupeId = 0,
                ExcelStream = null
            };
            //Act
            Invoking(() => Actual.ImportCiByExcelAsync(input))
            //Assert
            .Should().Throw<FredBusinessException>().WithMessage(BusinessResources.RepriseDonnees_Erreur_Extraction_Cis);
        }
    }
}
