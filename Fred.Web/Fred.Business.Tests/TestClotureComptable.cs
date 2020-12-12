using Fred.Business.DatesClotureComptable;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;
using Fred.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Fred.Business.Tests
{
    [TestClass]
    public class TestClotureComptable : BaseTu<DatesClotureComptableManager>
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUtilisateurManager> utilisateurManager;
        private Mock<IDatesClotureComptableRepository> datesClotureComptableRepository;
        private DateTime now;
        private Mock<FredDbContext> context;

        [TestInitialize]
        public void TestInitialize()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            context = GetMocked<FredDbContext>();
            utilisateurManager = new Mock<IUtilisateurManager>();
            datesClotureComptableRepository = GetMocked<IDatesClotureComptableRepository>();
            InitNow();
        }


        private void InitNow()
        {
            now = DateTime.Now;
        }

        private void FakeGetCIDatesClotureComptableByIdAndYearAndMonth(DatesClotureComptableEnt currentDatesClotureComptableEnt)
        {
            datesClotureComptableRepository
              .Setup(dccr => dccr.Get(3, It.IsAny<int>(), It.IsAny<int>()))
              .Returns(currentDatesClotureComptableEnt);
        }

        /// <summary>
        /// CheckIsMonthClosedReturnTrueTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthClosedReturnTrueWhenDateClotureIsInPastTest()
        {

            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = DateTime.Now.Year,
                Mois = DateTime.Now.Month,
                CiId = 3,
                DateCloture = DateTime.Now.AddDays(-1)
            };

            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, now.Year, now.Month);

            Assert.IsTrue(result);

        }

        /// <summary>
        /// CheckIsMonthClosedReturnTrueTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthClosedReturnTrueWhenDateClotureIsNowTest()
        {

            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = DateTime.Now.Year,
                Mois = DateTime.Now.Month,
                CiId = 3,
                DateCloture = DateTime.Now
            };

            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, now.Year, now.Month);

            Assert.IsTrue(result);

        }

        /// <summary>
        /// CheckIsMonthClosedReturnFalseWhenDateClotureIsInFuturTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthClosedReturnFalseWhenDateClotureIsInFuturTest()
        {

            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = DateTime.Now.Year,
                Mois = DateTime.Now.Month,
                CiId = 3,
                DateCloture = DateTime.Now.AddDays(1)
            };
            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, now.Year, now.Month);

            Assert.IsFalse(result);

        }

        /// <summary>
        /// CheckIsMonthClosedReturnFalseWhenDateClotureIsNullTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthClosedReturnFalseWhenDateClotureIsNullTest()
        {
            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = DateTime.Now.Year,
                Mois = DateTime.Now.Month,
                CiId = 3,
                DateCloture = null
            };
            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, now.Year, now.Month);

            Assert.IsFalse(result);

        }

        /// <summary>
        /// CheckIsMonthClosedReturnFalseWhenNoDatesClotureComptableEntTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthClosedReturnFalseWhenNoDatesClotureComptableEntTest()
        {

            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(default(DatesClotureComptableEnt));

            bool result = Actual.IsPeriodClosed(3, now.Year, now.Month);

            Assert.IsFalse(result);

        }


        /// <summary>
        /// CheckIsMonthNotClosedWhenDatesClotureComptableIsNextMonthTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthNotClosedWhenDatesClotureComptableIsNextMonthTest()
        {
            DateTime today = new DateTime(2017, 9, 25);

            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = 2017,
                Mois = 9,
                CiId = 3,
                DateCloture = new DateTime(2017, 10, 6)
            };

            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, 2017, 9, today);

            Assert.IsFalse(result);

        }

        /// <summary>
        /// CheckIsMonthNotClosedWhenDatesClotureComptableIsNextMonthTest
        /// </summary>
        [TestMethod]
        [TestCategory("ClotureComptable")]
        public void CheckIsMonthNotClosedWhenDatesClotureComptableIsNexYearTest()
        {
            DateTime today = new DateTime(2017, 9, 25);

            DatesClotureComptableEnt currentDatesClotureComptableEnt = new DatesClotureComptableEnt()
            {
                Annee = 2017,
                Mois = 12,
                CiId = 3,
                DateCloture = new DateTime(2018, 1, 6)
            };

            FakeGetCIDatesClotureComptableByIdAndYearAndMonth(currentDatesClotureComptableEnt);

            bool result = Actual.IsPeriodClosed(3, 2017, 12, today);

            Assert.IsFalse(result);

        }
    }
}