using System;
using System.IO;
using FluentAssertions;
using Fred.Business.OperationDiverse.Service;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.OperationDiverse.Service
{
    /// <summary>
    /// Classe de test de <see cref="OperationDiverseExcelService"/>
    /// </summary>
    [TestClass]
    public class OperationDiverseServiceTest : BaseTu<OperationDiverseExcelService>
    {
        [TestMethod]
        [TestCategory("OperationDiverseExcelService")]
        public void GetFichierExempleChargementOD_WithIncorrectPathDirectory_Returns_FileNotFoundException()
        {
            Invoking(() => Actual.GetFichierExempleChargementODAsync(1, new DateTime(2019, 10, 03), "")).Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        [TestCategory("OperationDiverseExcelService")]
        public void GetFichierExempleChargementOD_Returns_ByteArray_NotEmpty()
        {
            Invoking(() => Actual.GetFichierExempleChargementODAsync(1, new DateTime(2019, 10, 03), Configuration.GetFredbaseDirectory())).Should().NotBeNull();
        }
    }
}
