using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.ImportExport.Business.Tests.Personnel
{
    [TestClass]
    public class PersonnelFluxMultipleSocieteManagerTest
    {
        [TestMethod]
        [TestCategory("PersonnelFluxMultipleSocieteManager")]
        public async Task ImportationMultipleSocietes_ForFondation_NotThrowException()
        {
            var personnelManager = new Mock<IPersonnelManager>();
            personnelManager.Setup(m => m.ImportPersonnelsAffectations(It.IsAny<List<PersonnelAffectationResult>>())).Verifiable();
            var personnelEtlFactory = new PersonnelEtlFactory(new Mock<IImportPersonnelManager>().Object, personnelManager.Object);

            var fluxManagerMock = new Mock<IFluxManager>();
            fluxManagerMock.Setup(c => c.GetByCode(It.IsAny<string>())).Returns(new FluxEnt { Code = PersonnelFluxCode.CodeFluxFon, SocieteCode = "500,700", IsActif = true });
            fluxManagerMock.Setup(c => c.Update(It.IsAny<FluxEnt>())).Returns(true);

            var fluxRepositoryMock = new Mock<IFluxRepository>();

            var actual = new PersonnelFluxMultipleSocieteManager(fluxManagerMock.Object, personnelEtlFactory, fluxRepositoryMock.Object);

            await actual.ImportationMultipleSocietesJobAsync(new ImportationByCodeFluxParameter { BypassDate = true, CodeFlux = PersonnelFluxCode.CodeFluxFon });
        }
    }
}
