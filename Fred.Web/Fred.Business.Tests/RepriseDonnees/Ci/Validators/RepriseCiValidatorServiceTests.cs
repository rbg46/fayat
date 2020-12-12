using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Selector;
using Fred.Business.RepriseDonnees.Ci.Validators;
using Fred.Business.RepriseDonnees.Ci.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.RepriseDonnees.Validators
{
    [TestClass]
    public class RepriseCiValidatorServiceTests
    {

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        private RepriseCiValidatorService CreateService()
        {
            return new RepriseCiValidatorService(
                new PersonnelSelectorService(),
                new PaysSelectorService(),
                new CiSelectorService());
        }

        [TestMethod]
        public void VerifyImportRules_ToutesLesConditionsValides_Devrait_Retourner_Une_Validation_Ok()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            // Assert
            Assert.AreEqual(true, isValid, "Toutes les rg devrais etre valides");
        }

        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Une_Societe_Non_Connue_Devrait_Retourner_Une_Validation_En_Erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().CodeSociete = "AXA";

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowIsInvalid = result.ImportRuleResults.Any(x => !x.IsValid);
            var hasCiSocieteIsInGroupeError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiSocieteIsInGroupe && !x.IsValid);

            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir une erreur de type CiSocieteIsInGroupe");
            Assert.AreEqual(true, rowIsInvalid, "Il doit y avoir une erreur de type CiSocieteIsInGroupe");
            Assert.AreEqual(true, hasCiSocieteIsInGroupeError, "Il doit y avoir une erreur de type CiSocieteIsInGroupe");
        }

        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Un_CI_Non_Connu_Devrait_Retourner_Une_Validation_En_Erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().CodeCi = "41220";

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowIsInvalid = result.ImportRuleResults.Any(x => !x.IsValid);
            var hasCiCiIsInSocieteError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiCiIsInSociete && !x.IsValid);

            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir une erreur de type CiCiIsInSociete");
            Assert.AreEqual(true, rowIsInvalid, "Il doit y avoir une erreur de type CiCiIsInSociete");
            Assert.AreEqual(true, hasCiCiIsInSocieteError, "Il doit y avoir une erreur de type CiCiIsInSociete");
        }


        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Un_Pays_Non_Connu_Devrait_Retourner_Une_Validation_En_Erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().CodePays = "GB";

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowIsInvalid = result.ImportRuleResults.Any(x => !x.IsValid);
            var hasCiCiIsInSocieteError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiCodePaysUnknow && !x.IsValid);

            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir une erreur de type CiCodePaysUnknow");
            Assert.AreEqual(true, rowIsInvalid, "Il doit y avoir une erreur de type CiCodePaysUnknow");
            Assert.AreEqual(true, hasCiCiIsInSocieteError, "Il doit y avoir une erreur de type CiCodePaysUnknow");
        }

        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Un_Pays_Non_Connu_Devrait_Retourner_Une_Validation_En_Erreur_2()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().CodePays = "GB";
            repriseExcelCis.FirstOrDefault().CodePaysFacturation = "GB";
            repriseExcelCis.FirstOrDefault().CodePaysLivraison = "GB";

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowInvalids = result.ImportRuleResults.Count(x => !x.IsValid);
            var hasCiCiIsInSocieteError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiCodePaysUnknow && !x.IsValid);
            var hasCiCodePaysLivraisonUnknowError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiCodePaysLivraisonUnknow && !x.IsValid);
            var hasCiCodePaysFacturationUnknowError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiCodePaysFacturationUnknow && !x.IsValid);

            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir 3 erreurs sur les champs pays");
            Assert.AreEqual(3, rowInvalids, "Il doit y avoir 3 erreurs sur les champs pays");
            Assert.AreEqual(true, hasCiCiIsInSocieteError, "Il doit y avoir une erreur de type CiCodePaysUnknow");
            Assert.AreEqual(true, hasCiCodePaysLivraisonUnknowError, "Il doit y avoir une erreur de type CiCodePaysLivraisonUnknow");
            Assert.AreEqual(true, hasCiCodePaysFacturationUnknowError, "Il doit y avoir une erreur de type CiCodePaysFacturationUnknow");
        }


        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Un_Personnel_Non_Connu_Devrait_Retourner_Une_Validation_En_Erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().MatriculeResponsableChantier = "999";
            repriseExcelCis.FirstOrDefault().MatriculeResponsableAdministratif = "888";

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowInvalids = result.ImportRuleResults.Count(x => !x.IsValid);
            var hasCiResponsableAdministratifUnknowError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiResponsableAdministratifUnknow && !x.IsValid);
            var hasCiResponsableChantierUnknowError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiResponsableChantierUnknow && !x.IsValid);


            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir 2 erreurs sur les champs personnels");
            Assert.AreEqual(2, rowInvalids, "Il doit y avoir 2 erreurs sur les champs personnels");
            Assert.AreEqual(true, hasCiResponsableAdministratifUnknowError, "Il doit y avoir une erreur de type CiResponsableAdministratifUnknow");
            Assert.AreEqual(true, hasCiResponsableChantierUnknowError, "Il doit y avoir une erreur de type CiResponsableChantierUnknow");

        }

        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Une_Zone_Non_Connue_Devrait_Retourner_Une_Validation_En_Erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateExcelRows();
            repriseExcelCis.FirstOrDefault().ZoneModifiable = "R";//valeurs possible o, n et rien

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowInvalids = result.ImportRuleResults.Count(x => !x.IsValid);
            var hasCiZoneModifiableUnknowError = result.ImportRuleResults.Any(x => x.ImportRuleType == ImportRuleType.CiZoneModifiableUnknow && !x.IsValid);

            // Assert
            Assert.AreEqual(false, isValid, "Il doit y avoir 1 erreurs sur le champs ZoneModifiable");
            Assert.AreEqual(1, rowInvalids, "Il doit y avoir 1 erreurs sur le champs ZoneModifiable");
            Assert.AreEqual(true, hasCiZoneModifiableUnknowError, "Il doit y avoir une erreur de type CiZoneModifiableUnknow");

        }

        [TestMethod]
        public void VerifyImportRules_Si_La_Ligne_Excel_Contient_Aucune_donnee_sauf_les_champs_obligatoires_il_ne_devrait_pas_y_avoir_derreur()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            List<RepriseExcelCi> repriseExcelCis = RepriseCiFakeDataProvider.CreateEmptyExcelRows();

            ContextForImportCi context = RepriseCiFakeDataProvider.CreateContext();

            // Act
            var result = unitUnderTest.VerifyImportRules(repriseExcelCis, context);

            var isValid = result.AllLignesAreValid();
            var rowInvalids = result.ImportRuleResults.Count(x => !x.IsValid);

            // Assert
            Assert.AreEqual(true, isValid, "Il ne doit pas y avoir d'erreurs quand tout est vide");
            Assert.AreEqual(0, rowInvalids, "Il ne doit pas y avoir d'erreurs quand tout est vide");

        }


    }
}
