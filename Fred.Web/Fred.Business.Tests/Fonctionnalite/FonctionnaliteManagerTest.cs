using System.Collections.Generic;
using System.Linq;
using Fred.Business.Fonctionnalite;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Module;
using Fred.Business.PermissionFonctionnalite;
using Fred.Common.Tests.Data.Fonctionnalite.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.FonctionnaliteDesactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Fonctionnalite
{
    [TestClass]
    public class FonctionnaliteManagerTest
    {

        private FonctionnaliteManager manager;

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IFonctionnaliteValidator> featureValidator;
        private Mock<IPermissionFonctionnaliteManager> permissionFonctionnaliteManager;
        private Mock<IFonctionnaliteDesactiveManager> fonctionnaliteDesactiveManager;
        private Mock<IFonctionnaliteRepository> fonctionnaliteRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.unitOfWork = new Mock<IUnitOfWork>();
            this.featureValidator = new Mock<IFonctionnaliteValidator>();
            this.permissionFonctionnaliteManager = new Mock<IPermissionFonctionnaliteManager>();
            this.fonctionnaliteDesactiveManager = new Mock<IFonctionnaliteDesactiveManager>();
            this.fonctionnaliteRepository = new Mock<IFonctionnaliteRepository>();


            manager = new FonctionnaliteManager(this.unitOfWork.Object,
                                             this.fonctionnaliteRepository.Object,
                                             this.featureValidator.Object,
                                             this.permissionFonctionnaliteManager.Object,
                                             this.fonctionnaliteDesactiveManager.Object);

        }



        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("FonctionnaliteManager")]
        public void Verifie_Que_les_fonctionnalites_inactives_ne_devraient_pas_remonter()
        {

            this.fonctionnaliteRepository.Setup(repo => repo.GetFeatureListByModuleId(It.IsAny<int>()))
                                         .Returns(FonctionnaliteDataMocks.GetFeatureListByModuleId());

            this.fonctionnaliteDesactiveManager.Setup(repo => repo.GetInactifFonctionnalitesForSocieteId(It.IsAny<int>()))
                                               .Returns(FonctionnaliteDesactiveDataMocks.GetInactifFonctionnalitesForSocieteId());

            var result = manager.GetFonctionnaliteAvailablesForSocieteIdAndModuleId(1, 1);

            Assert.IsTrue(result.Count() == 2, "Les fonctionnalités inactives ne devraient pas remonter.");

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("FonctionnaliteManager")]
        public void Verifie_Que_les_fonctionnalites_actives_devraient_remonter()
        {

            this.fonctionnaliteRepository.Setup(repo => repo.GetFeatureListByModuleId(It.IsAny<int>()))
                                         .Returns(FonctionnaliteDataMocks.GetFeatureListByModuleId());

            this.fonctionnaliteDesactiveManager.Setup(repo => repo.GetInactifFonctionnalitesForSocieteId(It.IsAny<int>()))
                                               .Returns(new List<FonctionnaliteDesactiveEnt>());

            var result = manager.GetFonctionnaliteAvailablesForSocieteIdAndModuleId(1, 1);

            Assert.IsTrue(result.Count() == 3, "S'il n'y pas de fonctionnalités inactives, alors toutes fonctionnalités devraient remonter.");

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("FonctionnaliteManager")]
        public void Verifie_Qu_aucunes_fonctionnalites_ne_devrait_remonter()
        {

            this.fonctionnaliteRepository.Setup(repo => repo.GetFeatureListByModuleId(It.IsAny<int>()))
                                         .Returns(FonctionnaliteDataMocks.GetFeatureListByModuleId());

            this.fonctionnaliteDesactiveManager.Setup(repo => repo.GetInactifFonctionnalitesForSocieteId(It.IsAny<int>()))
                                               .Returns(FonctionnaliteDesactiveDataMocks.GetAllInactivesFonctionnalites());

            var result = manager.GetFonctionnaliteAvailablesForSocieteIdAndModuleId(1, 1);

            Assert.IsTrue(!result.Any(), "Aucune fonctionnalité ne devrait remonter car elles sont toutes inactives.");

        }

    }
}
