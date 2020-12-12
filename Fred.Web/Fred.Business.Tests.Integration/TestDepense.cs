using System;
using System.Linq;
using Fred.Entities.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestDepense : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des dépenses n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetDepenseListReturnNotNullList()
        {

            var depenses = DepenseMgr.GetDepenseList();
            Assert.IsTrue(depenses != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void ValidateFiltresDepenses()
        {

            SearchDepenseEnt filter = DepenseMgr.GetNewFilter();

            filter = new SearchDepenseEnt
            {
                ValueText = "ValidateFiltresDepenses",
                LibelleAsc = true,
                Libelle = true,

                // Critères :
                DateFrom = new DateTime(2016, 12, 7),
                DateTo = new DateTime(2016, 12, 9)
            };

            var depenses = DepenseMgr.SearchDepenseListWithFilter(filter, 1, 20).ToList();
            Assert.IsTrue(depenses != null && depenses.Count == 3);

            double montant = DepenseMgr.GetMontantTotal(filter);
            Assert.IsTrue(montant == 140); // 10 * 1 + 20 * 2 + 30 * 3

        }
    }
}
