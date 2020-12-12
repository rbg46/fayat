using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestRapprochementFacture : FredBaseTest
    {
        #region Rapprochement dépense/facture

        /// <summary>
        ///   Teste que la liste des dépenses retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetDepenseARapprocherList()
        {
            var resu = DepenseMgr.GetDepenseList();
            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste que la liste des factures a rapprocher retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFactureARapprocherList()
        {
            var resu = FactureMgr.GetFactureARList();
            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste que la liste des Depenses temporaire retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetDepenseTemporaireList()
        {
            var resu = DepenseMgr.GetDepenseList();
            Assert.IsTrue(resu != null);
        }

        #endregion
    }
}
