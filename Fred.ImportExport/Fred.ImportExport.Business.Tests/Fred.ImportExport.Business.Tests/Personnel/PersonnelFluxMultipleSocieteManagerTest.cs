using System.Collections.Generic;
using Fred.Business.Personnel;
using Fred.Common.Tests;
using Fred.Entities.Personnel.Import;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Entities.ImportExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.ImportExport.Business.Tests.Personnel
{
    /// <summary>
    /// Classe de test du manager <see cref="PersonnelFluxMultipleSocieteManager"/>
    /// </summary>
    [TestClass]
    public class PersonnelFluxMultipleSocieteManagerTest : BaseTu<PersonnelFluxMultipleSocieteManager>
    {
        [TestMethod]
        [TestCategory("PersonnelFluxMultipleSocieteManager")]
        public void ImportationMultipleSocietes_ForFondation_NotThrowException()
        {
            var personnelManager = GetMocked<IPersonnelManager>();
            personnelManager.Setup(m => m.ImportPersonnelsAffectations(It.IsAny<List<PersonnelAffectationResult>>())).Verifiable();
            PersonnelEtlFactory personnelEtlFactory = new PersonnelEtlFactory(GetMocked<IImportPersonnelFesManager>().Object, personnelManager.Object);
            SubstituteConstructorArgument<PersonnelEtlFactory>(personnelEtlFactory);
            var fluxManager = GetMocked<FluxManager>();
            fluxManager.Setup(c => c.GetByCode(It.IsAny<string>())).Returns(new FluxEnt { Code = PersonnelFluxCode.CodeFluxFon, SocieteCode = "500,700", IsActif = true });
            fluxManager.Setup(c => c.Update(It.IsAny<FluxEnt>())).Returns(true);
            Actual.SetFLuxManager(fluxManager.Object);
            Actual.ImportationMultipleSocietes(true, PersonnelFluxCode.CodeFluxFon);
        }
    }
}
