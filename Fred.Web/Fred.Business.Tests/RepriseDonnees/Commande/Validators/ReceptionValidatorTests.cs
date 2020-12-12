using Fred.Business.RepriseDonnees.Commande.Validators;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.RepriseDonnees.Commande.Validators
{
    [TestClass]
    public class ReceptionValidatorTests
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

        private ReceptionValidator CreateReceptionValidator()
        {
            return new ReceptionValidator();
        }

        [TestMethod]
        public void VerifyDateReceptionRule_a_positionner_avec_la_date_du_champ_date_reception_Si_format_non_valide_rejet_de_la_ligne_success()
        {
            // Arrange
            var unitUnderTest = this.CreateReceptionValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                DateReception = "31/12/2018"
            };

            // Act
            var result = unitUnderTest.VerifyDateReceptionRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(string.Empty, result.ErrorMessage, "A positionner avec la date du champ Date Réception Format non valide => rejet de la ligne");
            Assert.IsTrue(result.IsValid, "A positionner avec la date du champ Date Réception Format non valide => rejet de la ligne");

        }

        [TestMethod]
        public void VerifyDateReceptionRule_a_positionner_avec_la_date_du_champ_date_reception_Si_format_non_valide_rejet_de_la_ligne_error()
        {
            // Arrange
            var unitUnderTest = this.CreateReceptionValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                DateReception = "32/12/2018"
            };

            // Act
            var result = unitUnderTest.VerifyDateReceptionRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual("Rejet ligne n°1 : Date Réception invalide.", result.ErrorMessage, "A positionner avec la date du champ Date Réception, Format non valide => rejet de la ligne");
            Assert.IsFalse(result.IsValid, "A positionner avec la date du champ Date Réception, Format non valide => rejet de la ligne");

        }
    }
}
