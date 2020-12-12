using Fred.Business.RepriseDonnees.Commande.Selector;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.RepriseDonnees.Commande.Selector
{
    [TestClass]
    public class QuantiteSelectorTests
    {
        private MockRepository mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private QuantiteSelector CreateQuantiteSelector()
        {
            return new QuantiteSelector();
        }

        [TestMethod]
        public void GetQuantiteCommandeLigne_Si_qt_commandee_moins_qt_facturee_rapprochee_si_qt_commandee_qt_facturee_rapprochee_supperieur_a_0()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "100,99",
                QuantiteFactureeRapprochee = "50,99",
                QuantiteReceptionnee = "60,8"
            };

            // Act
            var result = unitUnderTest.GetQuantiteCommandeLigne(repriseExcelCommande);

            // Assert
            Assert.AreEqual((decimal)50, result.Value, "Qté commandée - Qté facturée rapprochée si Qté commandée – Qté facturée rapprochée > 0");

        }

        [TestMethod]
        public void GetQuantiteCommandeLigne_devrait_rejeter_la_ligne()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "100",
                QuantiteFactureeRapprochee = "200",
                QuantiteReceptionnee = "120"
            };
            // Act
            var result = unitUnderTest.GetQuantiteCommandeLigne(repriseExcelCommande);


            // Assert
            Assert.AreEqual(false, result.HasValue, "Qté commandée - Qté facturée rapprochée si Qté commandée – Qté facturée rapprochée > 0" +
                "Qté réceptionnée - Qté facturée rapprochée si Qté réceptionnée - Qté facturée rapprochée > 0" +
                "Sinon rejet de la ligne.");
        }

        [TestMethod]
        public void GetQuantiteCommandeLigne_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "100,99",
                QuantiteFactureeRapprochee = "150,7",
                QuantiteReceptionnee = "250,8"
            };
            // Act
            var result = unitUnderTest.GetQuantiteCommandeLigne(repriseExcelCommande);


            // Assert
            Assert.AreEqual((decimal)100.10, result.Value, "Qté réceptionnée - Qté facturée rapprochée si Qté réceptionnée - Qté facturée rapprochée > 0");
        }


        [TestMethod]
        public void CanCreateReception_devrait_pouvoir_crer_une_reception_si_qt_receptionnee_qt_facturee_rapprochee_supp_a_0()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                QuantiteFactureeRapprochee = "50",
                QuantiteReceptionnee = "100"
            };
            // Act
            var result = unitUnderTest.CanCreateReception(repriseExcelCommande);


            // Assert
            Assert.IsTrue(result, "Devrait pouvoir crer une reception si Qté réceptionnée - Qté facturée rapprochée  > 0");
        }

        [TestMethod]
        public void CanCreateReception_ne_devrait_pouvoir_crer_une_reception_si_qt_receptionnee_qt_facturee_rapprochee_inf_ou_egal_a_0()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                QuantiteFactureeRapprochee = "150",
                QuantiteReceptionnee = "100"
            };
            // Act
            var result = unitUnderTest.CanCreateReception(repriseExcelCommande);


            // Assert
            Assert.IsFalse(result, "Ne Devrait pouvoir crer une reception si Qté réceptionnée - Qté facturée rapprochée  <= 0");
        }
        [TestMethod]
        public void CanCreateReception_ne_devrait_pouvoir_crer_une_reception_si_qt_receptionnee_qt_facturee_rapprochee_inf_ou_egal_a_0__2()
        {
            // Arrange
            var unitUnderTest = this.CreateQuantiteSelector();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                QuantiteFactureeRapprochee = "100,15",
                QuantiteReceptionnee = "100,15"
            };
            // Act
            var result = unitUnderTest.CanCreateReception(repriseExcelCommande);


            // Assert
            Assert.IsFalse(result, "Ne Devrait pouvoir crer une reception si Qté réceptionnée - Qté facturée rapprochée  <= 0");
        }

        //[TestMethod]
        //public void GetQuantiteReception_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateQuantiteSelector();
        //    RepriseExcelCommande repriseExcelCommande = TODO;

        //    // Act
        //    var result = unitUnderTest.GetQuantiteReception(                repriseExcelCommande);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
