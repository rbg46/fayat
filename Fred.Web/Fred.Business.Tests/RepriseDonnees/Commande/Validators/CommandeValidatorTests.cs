using Fred.Business.RepriseDonnees.Commande.Validators;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.RepriseDonnees.Commande.Validators
{
    [TestClass]
    public class CommandeValidatorTests
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

        private CommandeValidator CreateCommandeValidator()
        {
            return new CommandeValidator();
        }


        private const string CommaonRequiredError = @"Les informations suivantes sont obligatoires :
                                                •   Code Société
                                                •   Code CI
                                                •   Code Fournisseur
                                                •   N° Commande Externe
                                                •   Type Commande
                                                •   Libellé En - tête Commande
                                                •   Code Devise
                                                •   Date Commande
                                                •   Désignation ligne Commande
                                                •   Code Ressource
                                                •   Unité
                                                •   PU
                                                •   Qté commandée
                                                •   Date Réception
                                                ";
        [TestMethod]
        public void VerifyRequiredFieldsRule_tout_est_ok()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411001",
                CodeFournisseur = "FOURNISSEUR 1",
                NumeroCommandeExterne = "F00011",
                TypeCommande = "FOURNITURE",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsTrue(result.IsValid, "Une commande doit nécessairement contenir les mêmes informations communes .");
            Assert.AreEqual(string.Empty, result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_CodeSociete_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = null,
                CodeCi = "411001",
                CodeFournisseur = "FOURNISSEUR 1",
                NumeroCommandeExterne = "F00011",
                TypeCommande = "FOURNITURE",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_CodeCi_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "",
                CodeFournisseur = "FOURNISSEUR 1",
                NumeroCommandeExterne = "F00011",
                TypeCommande = "FOURNITURE",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }



        [TestMethod]
        public void VerifyRequiredFieldsRule_CodeFournisseur_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "",
                NumeroCommandeExterne = "F00011",
                TypeCommande = "FOURNITURE",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_NumeroCommandeExterne_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "",
                TypeCommande = "FOURNITURE",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_TypeCommande_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "",
                LibelleEnteteCommande = "LIBELLE",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_LibelleEnteteCommande_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "",
                CodeDevise = "FR",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019"
            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_CodeDevise_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_DateCommande_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "",
                DesignationLigneCommande = "DESIGNATION",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_DesignationLigneCommande_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "",
                CodeRessource = "DESIGNATION",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_CodeRessource_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "designation",
                CodeRessource = "",
                Unite = "FRT",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_Unite_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "designation",
                CodeRessource = "158",
                Unite = "",
                PuHt = "10.5",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_PuHt_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "designation",
                CodeRessource = "158",
                Unite = "EUR",
                PuHt = "",
                QuantiteCommandee = "150",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);


        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_QuantiteCommandee_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "designation",
                CodeRessource = "158",
                Unite = "EUR",
                PuHt = "125.5",
                QuantiteCommandee = "",
                DateReception = "01/01/2019",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);


        }

        [TestMethod]
        public void VerifyRequiredFieldsRule_DateReception_manquant()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeSociete = "RB",
                CodeCi = "411100",
                CodeFournisseur = "Fournisseur",
                NumeroCommandeExterne = "NumeroCommandeExterne",
                TypeCommande = "LibelleEnteteCommande",
                LibelleEnteteCommande = "LibelleEnteteCommande",
                CodeDevise = "frf",
                DateCommande = "01/01/2019",
                DesignationLigneCommande = "designation",
                CodeRessource = "158",
                Unite = "EUR",
                PuHt = "125.5",
                QuantiteCommandee = "158",
                DateReception = "",

            };
            // Act
            var result = unitUnderTest.VerifyRequiredFieldsRule(repriseExcelCommande);

            // Assert
            Assert.IsFalse(result.IsValid, CommaonRequiredError);
            Assert.AreEqual("Rejet ligne n°1 : champ(s) obligatoire(s) non renseigné(s).", result.ErrorMessage, CommaonRequiredError);

        }

        //[TestMethod]
        //public void VerifyTypeIdRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    RepriseExcelCommande repriseExcelCommande = TODO;
        //    ContextForImportCommande context = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyTypeIdRule(
        //        repriseExcelCommande,
        //        context);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void VerifyCodeFournisseurRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    RepriseExcelCommande repriseExcelCommande = TODO;
        //    ContextForImportCommande context = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyCodeFournisseurRule(
        //        repriseExcelCommande,
        //        context);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void VerifyCodeCiRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    int groupeId = TODO;
        //    RepriseExcelCommande repriseExcelCommande = TODO;
        //    OrganisationTree organisationTree = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyCodeCiRule(
        //        groupeId,
        //        repriseExcelCommande,
        //        organisationTree);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void VerifyCodeSocieteRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    RepriseExcelCommande repriseExcelCommande = TODO;
        //    ContextForImportCommande context = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyCodeSocieteRule(
        //        repriseExcelCommande,
        //        context);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void VerifyFormatDateCommandeRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    RepriseExcelCommande repriseExcelCommande = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyFormatDateCommandeRule(
        //        repriseExcelCommande);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void VerifyCommandeDeviseRule_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommandeValidator();
        //    RepriseExcelCommande repriseExcelCommande = TODO;
        //    ContextForImportCommande context = TODO;

        //    // Act
        //    var result = unitUnderTest.VerifyCommandeDeviseRule(
        //        repriseExcelCommande,
        //        context);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
