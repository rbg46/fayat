using System;
using System.Linq;
using Fred.Entities.CloturesPeriodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    /// <summary>
    /// Classe de test des clotures en masses
    /// </summary>
    [TestClass]
    public class TestCloturesPeriodes : FredBaseTest
    {
        /// <summary>
        /// RG_5211_008/009 Appliquer systématiquement [ChantierFRED]= 1
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void SearchFilterReturnCisAreAllChantierFred()
        {
            var currentDate = DateTime.UtcNow;
            var searchCloturesPeriodesForCiEnt = new SearchCloturesPeriodesForCiEnt
            {
                Month = currentDate.Month,
                Year = currentDate.Year
            };
            var cisFREDIds = CIMgr.GetCIList(true).Select(ci => ci.CiId).ToList();

            var cisACloturer = CloturesPeriodesManager.SearchFilter(searchCloturesPeriodesForCiEnt);

            Assert.IsTrue(cisACloturer.Items.All(c => cisFREDIds.Contains(c.CiId)));
        }

        /// <summary>
        /// RG_5211_008/009 Appliquer systématiquement [ChantierFRED]= 1
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void SearchFilterReturnCisNotChantierFredAreExcluded()
        {
            var currentDate = DateTime.UtcNow;
            var searchCloturesPeriodesForCiEnt = new SearchCloturesPeriodesForCiEnt
            {
                Month = currentDate.Month,
                Year = currentDate.Year
            };
            var cisNotFREDIds = CIMgr.GetCIList().Where(ci => !ci.ChantierFRED).Select(ci => ci.CiId).ToList();

            var cisACloturer = CloturesPeriodesManager.SearchFilter(searchCloturesPeriodesForCiEnt);

            Assert.IsFalse(cisACloturer.Items.Any(c => cisNotFREDIds.Contains(c.CiId)));
        }
    }

}
