using System.Linq;
using AutoMapper;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.Commande.Services;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Mock;
using Fred.Common.Tests.Data.Commande.Fake;
using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Mock;
using Fred.Common.Tests.Data.Unite.Builder;
using Fred.Entities.CI;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Commande
{
    [TestClass]
    public class GenerateExempleExcelTest : BaseTu<CommandeImportExportExcelService>
    {
        private readonly CiMocks ciMock = new CiMocks();
        private readonly RessourceMocks ressourceMocks = new RessourceMocks();

        private string checkinValueAvenant = string.Empty;
        private string checkinValueCommande = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            var uniteManager = GetMocked<IUniteManager>();
            var cIManager = GetMocked<ICIManager>();
            var tacheManager = GetMocked<ITacheManager>();
            var referentielFixeManager = GetMocked<IReferentielFixeManager>();

            checkinValueAvenant = "F000403945"; //le numéro de la commande est celui du numéro commande du fichier Excel Avanant.xslx
            checkinValueCommande = "03/10/2019";//la date est celui de la date du fichier Excel Commande.xslx

            //Arrange
            cIManager.Setup(m => m.GetCIById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(ciMock.GetFakeDbSet().FirstOrDefault());

            uniteManager.Setup(m => m.SearchLight(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new UniteBuilder().UniteId(1).Code("FRT").Libelle("Forfait").BuildNObjects(1, true));

            tacheManager.Setup(m => m.GetTacheListByCiId(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(new TacheBuilder().TacheId(1).Code("00").Libelle("Tache par defaut").BuildNObjects(1, true));

            referentielFixeManager.Setup(m => m.SearchRessourcesRecommandees(It.IsAny<string>(), It.IsAny<CIEnt>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), null))
                .Returns(ressourceMocks.GetFakeDbSet().ToList());
            var fakeMapper = new CommandeFake().FakeMapper;
            SubstituteConstructorArgument(fakeMapper);
        }

        /// <summary>
        ///   Teste si fichier Format correct
        /// </summary>
        [TestMethod]
        [TestCategory("GenerateExempleExcel")]
        public void GenerateExempleExcel_CiNull()
        {
            //Act
            Invoking(() => Actual.GenerateExempleExcel(0, false))
            //Assert
            .Should().Throw<FredBusinessException>().Which.Message.Should().Be(
                FeatureExportExcel.Invalid_FileFormat
            );
        }
    }
}
