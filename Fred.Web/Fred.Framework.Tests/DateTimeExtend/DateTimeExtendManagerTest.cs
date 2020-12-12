using System;
using Fred.Framework.DateTimeExtend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests.DateTimeExtend
{
    [TestClass]
    public class DateTimeExtendManagerTest
    {
        private IDateTimeExtendManager datetimeExtendMgr;

        /// <summary>
        ///   Initialise un test, cette méthode s'exécute avant chaque test
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this.datetimeExtendMgr = new DateTimeExtendManager();
        }

        /// <summary>
        ///   Teste des jours ouvrés en FRANCE
        /// </summary>
        [TestMethod]
        public void TestBusinessDay()
        {
            DateTime nouvelAn = new DateTime(2018, 1, 1);
            DateTime paque = new DateTime(2018, 4, 2);
            DateTime feteTravail = new DateTime(2018, 5, 1);
            DateTime victoire = new DateTime(2018, 5, 8);
            DateTime ascension = new DateTime(2018, 5, 10);
            DateTime lundiPentecote = new DateTime(2018, 5, 21);
            DateTime feteNational = new DateTime(2018, 7, 14);
            DateTime assomption = new DateTime(2018, 8, 15);
            DateTime toussaint = new DateTime(2018, 11, 1);
            DateTime armistice = new DateTime(2018, 11, 11);
            DateTime noel = new DateTime(2018, 12, 25);
            DateTime test = new DateTime(2018, 12, 26);

            DateTime monday = new DateTime(2018, 7, 9);
            DateTime saturday = new DateTime(2018, 7, 7);

            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(nouvelAn));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(paque));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(feteTravail));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(victoire));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(ascension));

            // Le lundi de pentecote n'est pas férié pour l'extension ...
            // Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(lundiPentecote));

            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(feteNational));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(assomption));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(toussaint));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(armistice));
            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(noel));

            Assert.IsFalse(datetimeExtendMgr.IsBusinessDay(saturday));
            Assert.IsTrue(datetimeExtendMgr.IsBusinessDay(monday));
        }


        /// <summary>
        ///   Teste Compte le nombre de jours non ouvrés dans une période
        /// </summary>
        [TestMethod]
        public void TestCountHolidaysAndWeekends()
        {
            // Début : 1er mai 2018
            DateTime start = new DateTime(2018, 5, 1);
            // Fin : 31 mai 2018
            DateTime end = new DateTime(2018, 5, 31);

            int cpt = datetimeExtendMgr.CountHolidaysAndWeekends(start, end);

            // Attention le 21 mai (lundi de pentecote) n'est pas compté comme un jour férié
            Assert.IsTrue(cpt == 11);
        }
    }
}
