using FluentValidation.Results;
using Fred.Common.Tests.Data.EcritureComptable.Builder;
using Fred.ImportExport.Business.EcritureComptable.Validator;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.EcritureComptable
{
    [TestClass]
    public class AchatAvecCommandeFayatTpValidatorTest
    {
        private EcritureComptableFayatTpModelBuilder ecritureComptableFayatTpModelBuilder = null;
        private ValidationResult resultat = null;
        private EcritureComptableFluxManagerFayatTPValidator ecritureComptableFluxManagerFayatTPValidator = new EcritureComptableFluxManagerFayatTPValidator();

        [TestInitialize]
        public void Initialize()
        {
            ecritureComptableFayatTpModelBuilder = new EcritureComptableFayatTpModelBuilder().Default()
                .RapportLigneId(null);
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ImportOk()
        {
            string errorMessage = FeatureEcritureComptable.EcritureCompable_Erreur_DateCreation_Obligatoire;

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && resultat.IsValid);
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurDateCreation()
        {
            string errorMessage = FeatureEcritureComptable.EcritureCompable_Erreur_DateCreation_Obligatoire;
            ecritureComptableFayatTpModelBuilder.DateCreation(null);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurDateComptable()
        {
            string errorMessage = FeatureEcritureComptable.EcritureCompable_Erreur_DateComptable_Obligatoire;
            ecritureComptableFayatTpModelBuilder.DateComptable(null);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurLibelle()
        {
            string errorMessage = FeatureEcritureComptable.EcritureCompable_Erreur_Libelle_Obligatoire;
            ecritureComptableFayatTpModelBuilder.Libelle(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurNature()
        {
            string errorMessage = FeatureEcritureComptable.EcritureComptable_Erreur_CodeNatureAnalytique_Obligatoire;
            ecritureComptableFayatTpModelBuilder.NatureAnalytique(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurMontantDeviseInterne()
        {
            string errorMessage = FeatureEcritureComptable.EcritureComptable_Erreur_MontantDeviseInterne_Obligatoire;
            ecritureComptableFayatTpModelBuilder.MontantDeviseInterne(null);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurDeviseInterne()
        {
            string errorMessage = FeatureEcritureComptable.EcritureComptable_Erreur_DeviseInterne_Obligatoire;
            ecritureComptableFayatTpModelBuilder.DeviseInterne(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurMontantDeviseTransaction()
        {
            string errorMessage = FeatureEcritureComptable.EcritureComptable_Erreur_MontantTransactionDevise_Obligatoire;
            ecritureComptableFayatTpModelBuilder.MontantDeviseTransaction(null);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurDeviseTransaction()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_DeviseTransaction_Obligatoire;
            ecritureComptableFayatTpModelBuilder.DeviseTransaction(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurQuantite()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_Quantite_Obligatoire;
            ecritureComptableFayatTpModelBuilder.Quantite(null);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurUnite()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_Unite_Obligatoire;
            ecritureComptableFayatTpModelBuilder.Unite(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurCi()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_CI_Obligatoire;
            ecritureComptableFayatTpModelBuilder.Ci(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurNumPieceComptable()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroPiece_Obligatoire;
            ecritureComptableFayatTpModelBuilder.NumeroPiece(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurNumCommande()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroCommande_Obligatoire;
            ecritureComptableFayatTpModelBuilder.NumeroCommande(string.Empty);

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public void ValidateAchatRules_AchatAvecCommande_ErreurNumLignePointage()
        {
            string errorMessage = FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroLigneDePointage_Vide;
            ecritureComptableFayatTpModelBuilder.RapportLigneId("40120");

            resultat = ecritureComptableFluxManagerFayatTPValidator.ValidateAchatRules(ecritureComptableFayatTpModelBuilder.Model);

            Assert.IsTrue(resultat != null && !resultat.IsValid &&
                resultat.Errors[0].ErrorMessage.Contains(errorMessage));
        }
    }
}
