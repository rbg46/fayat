using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Personnel.Interimaire.Mock;
using Fred.Common.Tests.Data.Rapport.Pointage.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Rapport.Pointage
{
    /// <summary>
    /// Classe de test de <see cref="PointageManager"/>
    /// </summary>
    [TestClass]
    public class PointageManagerTest : BaseTu<PointageManager>
    {
        private PointageFakes Mocks;
        private ContratInterimaireMocks CttInterimMocks;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IPointageRepository> PointageRepository;

        [TestInitialize]
        public void Initialize()
        {
            //Mocks
            Mocks = new PointageFakes();
            CttInterimMocks = new ContratInterimaireMocks();
            PointageRepository = GetMocked<IPointageRepository>();
            unitOfWork = GetMocked<IUnitOfWork>();

            //Setup Fake Items
            var fakeCttManager = GetMocked<IContratInterimaireManager>();
            fakeCttManager.Setup(c => c.GetContratInterimaireByDatePointage(It.IsAny<int?>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((i, d) => CttInterimMocks.ContratsInterimairesExpected.Where(c => c.DateDebut <= d.Date && c.DateFin >= d.Date).FirstOrDefault());
            fakeCttManager.Setup(c => c.GetContratInterimaireByDatePointageAndSouplesse(It.IsAny<int?>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((i, d) => CttInterimMocks.ContratsInterimairesExpected.Where(c => c.DateDebut <= d.Date && c.DateFin >= d.Date).FirstOrDefault());
        }

        [TestMethod]
        [TestCategory("Moq")]

        public void TraitementPointageInterimaireExport_ReturnsAllActivesContracts()
        {
            //Arrange
            IEnumerable<IGrouping<string, RapportLigneEnt>> contrats = new List<IGrouping<string, RapportLigneEnt>>();
            //Act
            var contratsInterimaires = Actual.TraitementPointageInterimaireExport(Mocks.PointagesExpected, contrats);
            //Assert
            Assert.AreEqual(CttInterimMocks.ContratsInterimairesExpected.Count(), contratsInterimaires.Count());
        }

        [TestMethod]
        [TestCategory("Moq")]

        public void TraitementPointageInterimaireExport_WhenPointageOutside_ReturnsEmptyList()
        {
            //Arrange
            var pointagesHorsContrats = new List<RapportLigneEnt> { Mocks.FakePointageHorsContrats() };
            IEnumerable<IGrouping<string, RapportLigneEnt>> contrats = new List<IGrouping<string, RapportLigneEnt>>();
            //Act
            var contratsInterimaires = Actual.TraitementPointageInterimaireExport(pointagesHorsContrats, contrats);
            //Assert
            Assert.AreEqual(0, contratsInterimaires.Count());
        }

        [TestMethod]
        [TestCategory("Moq")]
        public void GetPointageByPersonnelIDAndInterval_WhenRapportLignesVerrouillee_ReturnsStatutVerrouille()
        {
            var dateMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            var personnelModel = new RapportHebdoPersonnelWithAllCiEnt
            {
                IsForMonth = false,
                PersonnelIds = new List<int> { 1 },
                Mondaydate = dateMonday
            };
            var fakePointages = Mocks.GetFakePointagesSemaine(dateMonday, new List<int> { 1 });
            fakePointages.ForEach(x => x.RapportLigneStatutId = RapportStatutEnt.RapportStatutVerrouille.Key);
            fakePointages.ForEach(x => x.HeureNormale = 7);

            this.PointageRepository
                .Setup(x => x.GetRapportLigneByPersonnelIdAndWeek(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(fakePointages);

            var resultList = Actual.GetPointageByPersonnelIDAndInterval(personnelModel);
            Assert.IsTrue(resultList.Count == 1);
            Assert.IsTrue(resultList.First().PointageStatutCode == RapportStatutEnt.RapportStatutVerrouille.Value);
        }

        [TestMethod]
        [TestCategory("Moq")]
        public void GetPointageByPersonnelIDAndInterval_WhenRapportLignesValide2AndMultipleCIAndMajoration_ReturnsStatutValide2()
        {

            var dateMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            var personnelModel = new RapportHebdoPersonnelWithAllCiEnt
            {
                IsForMonth = false,
                PersonnelIds = new List<int> { 1 },
                Mondaydate = dateMonday
            };
            var fakePointages = Mocks.GetFakePointagesSemaine(dateMonday, new List<int> { 1, 2 });
            fakePointages.ForEach(x => x.RapportLigneStatutId = RapportStatutEnt.RapportStatutValide2.Key);
            fakePointages.Where(x => x.CiId == 1).ToList().ForEach(x => x.HeureNormale = 7);
            // Ajout de majorations sur le 2eme CI
            fakePointages.Where(x => x.CiId == 2).ToList().ForEach(x => x.ListRapportLigneMajorations.First().HeureMajoration = 3);

            this.PointageRepository
                .Setup(x => x.GetRapportLigneByPersonnelIdAndWeek(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(fakePointages);
            this.PointageRepository.Setup(f => f.GetRapportStatutAllSync())
                .Returns(new List<RapportStatutEnt> { new RapportStatutEnt { Code = "EC", Libelle = "En cours" }, new RapportStatutEnt { Code = "V2", Libelle = "Validé 2" } });

            var resultList = Actual.GetPointageByPersonnelIDAndInterval(personnelModel);
            Assert.IsTrue(resultList.Count == 1);
            Assert.IsTrue(resultList.First().PointageStatutCode == RapportStatutEnt.RapportStatutValide2.Value);
        }

        [TestMethod]
        [TestCategory("Moq")]
        public void GetPointageByPersonnelIDAndInterval_WhenRapportLignesEnCours_ReturnsStatutEnCours()
        {
            var dateMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            var personnelModel = new RapportHebdoPersonnelWithAllCiEnt
            {
                IsForMonth = false,
                PersonnelIds = new List<int> { 1 },
                Mondaydate = dateMonday
            };
            var fakePointages = Mocks.GetFakePointagesSemaine(dateMonday, new List<int> { 1 });
            fakePointages.ForEach(x => x.RapportLigneStatutId = RapportStatutEnt.RapportStatutVerrouille.Key);
            // 1 seul statut en cours le lundi
            fakePointages.First(x => x.DatePointage == dateMonday).RapportLigneStatutId = RapportStatutEnt.RapportStatutEnCours.Key;

            fakePointages.ForEach(x => x.HeureNormale = 7);

            this.PointageRepository
                .Setup(x => x.GetRapportLigneByPersonnelIdAndWeek(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(fakePointages);

            var resultList = Actual.GetPointageByPersonnelIDAndInterval(personnelModel);
            Assert.IsTrue(resultList.Count == 1);
            Assert.IsTrue(resultList.First().PointageStatutCode == RapportStatutEnt.RapportStatutEnCours.Value);
        }
    }
}
