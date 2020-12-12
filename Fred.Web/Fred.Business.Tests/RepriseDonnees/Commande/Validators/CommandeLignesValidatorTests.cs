using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Validators;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.RepriseDonnees.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.RepriseDonnees.Commande.Validators
{
    [TestClass]
    public class CommandeLignesValidatorTests
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

        private CommandeLignesValidator CreateCommandeLignesValidator()
        {
            return new CommandeLignesValidator();
        }

        [TestMethod]
        public void VerifyCodeTacheForCommandeLigneRule_si_une_tache_est_trouvee_pour_le_ci_mais_que_le_code_ne_correspond_pas_il_devrait_y_avoir_une_erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeTache = "CODETACHE1"
            };

            ContextForImportCommande context = new ContextForImportCommande();
            context.GroupeId = OrganisationTreeMocks.GROUPE_ID_RZB;
            context.OrganisationTree = OrganisationTreeMocks.GetOrganisationTree();
            context.TachesUsedInExcel = new List<GetT3ByCodesOrDefaultResponse>
            {
                new GetT3ByCodesOrDefaultResponse
                {
                    CiId = 1587,
                    Code = "000000",
                    Tache =  new Entities.Referential.TacheEnt
                    {
                        TacheId = 15,
                        CI = null,
                        CiId = 1587,
                        Code = "000000",
                    }
                }
            };
            // Act
            var result = unitUnderTest.VerifyCodeTacheForCommandeLigneRule(repriseExcelCommande, context);

            // Assert
            Assert.AreEqual(false, result.IsValid, "Si une tache est trouvee pour le ci mais que le code ne correspond pas il devrait y avoir une erreur.");
            Assert.AreEqual("Rejet ligne n°1 : Code Tâche invalide.", result.ErrorMessage, "Si une tache est trouvee pour le ci mais que le code ne correspond pas il devrait y avoir une erreur.");

        }

        [TestMethod]
        public void VerifyCodeTacheForCommandeLigneRule_si_une_tache_est_trouvee_pour_le_ci_et_que_le_code_est_vide_il_ne_devrait_pas_y_avoir_une_erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeTache = "",
                CodeSociete = OrganisationTreeMocks.CODE_SOCIETE_RZB,
                CodeCi = OrganisationTreeMocks.CODE_CI_411100_SOCIETE_RZB
            };

            ContextForImportCommande context = new ContextForImportCommande();
            context.GroupeId = OrganisationTreeMocks.GROUPE_ID_RZB;
            context.OrganisationTree = OrganisationTreeMocks.GetOrganisationTree();
            context.TachesUsedInExcel = new List<GetT3ByCodesOrDefaultResponse>
            {
                new GetT3ByCodesOrDefaultResponse
                {
                    CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                    Code = "",
                    Tache =  new Entities.Referential.TacheEnt
                    {
                        TacheId = 15,
                        CI = null,
                        CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                        Code = "000000",
                        TacheParDefaut = true
                    }
                }
            };
            // Act
            var result = unitUnderTest.VerifyCodeTacheForCommandeLigneRule(repriseExcelCommande, context);

            // Assert
            Assert.AreEqual(true, result.IsValid, "Si une tache est trouvee pour le ci et que le code est vide, il ne devrait pas y avoir une erreur.");

        }

        [TestMethod]
        public void VerifyCodeTacheForCommandeLigneRule_si_une_tache_est_trouvee_pour_le_ci_et_que_le_code_est_vide_il_ne_devrait_pas_y_avoir_une_erreur_2()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                CodeTache = "002",
                CodeSociete = OrganisationTreeMocks.CODE_SOCIETE_RZB,
                CodeCi = OrganisationTreeMocks.CODE_CI_411100_SOCIETE_RZB
            };

            ContextForImportCommande context = new ContextForImportCommande();
            context.GroupeId = OrganisationTreeMocks.GROUPE_ID_RZB;
            context.OrganisationTree = OrganisationTreeMocks.GetOrganisationTree();
            context.TachesUsedInExcel = new List<GetT3ByCodesOrDefaultResponse>
            {
                new GetT3ByCodesOrDefaultResponse
                {
                    CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                    Code = "",
                    Tache =  new Entities.Referential.TacheEnt
                    {
                        TacheId = 15,
                        CI = null,
                        CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                        Code = "000000",
                        TacheParDefaut = true
                    }
                },
                 new GetT3ByCodesOrDefaultResponse
                {
                    CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                    Code = "002",
                    Tache =  new Entities.Referential.TacheEnt
                    {
                        TacheId = 16,
                        CI = null,
                        CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                        Code = "T3",
                        TacheParDefaut = false
                    }
                }
            };
            // Act
            var result = unitUnderTest.VerifyCodeTacheForCommandeLigneRule(repriseExcelCommande, context);

            // Assert
            Assert.AreEqual(true, result.IsValid, "Si une tache est trouvee pour le ci et que le code est NON vide, il ne devrait pas y avoir une erreur.");
        }

        [TestMethod]
        public void VerifyQuantiteReceptionneeFormatRule_Si_se_n_est_pas_un_chiffre_il_devrait_y_avoir_une_erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteReceptionnee = "FQSDQSD8"
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteReceptionneeFormatRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "il faut que ce soit un chiffre");

        }

        [TestMethod]
        public void VerifyQuantiteFactureeRapprocheeFormatRule_Si_se_n_est_pas_un_chiffre_il_devrait_y_avoir_une_erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteFactureeRapprochee = "FQSDQSD8"
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteFactureeRapprocheeFormatRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "il faut que ce soit un chiffre");
        }

        [TestMethod]
        public void VerifyQuantiteCommandeeFormatRule__Si_se_n_est_pas_un_chiffre_il_devrait_y_avoir_une_erreur()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "FQSDQSD8"
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteCommandeeFormatRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "il faut que ce soit un chiffre");
        }

        [TestMethod]
        public void VerifyQuantiteRule_la_quantite_ne_peux_etre_calculer_qui_si_les_quantites_sont_des_chiffres()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "FQSDQSD8",
                QuantiteFactureeRapprochee = "FQSDQSD8",
                QuantiteReceptionnee = "FQSDQSD8",
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "La quantite ne peux etre calculer qui si les quantites sont des chiffres");
        }

        [TestMethod]
        public void VerifyQuantiteRule_la_quantite_est_valide_si_qt_commandee_moins_qt_facturee_rapproche_est_supperieur_a_0()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "15,5",
                QuantiteFactureeRapprochee = "10,5",
                QuantiteReceptionnee = "",
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(true, result.IsValid, "La quantite peux etre calculer si Qté commandée - Qté facturée rapprochée > 0");
        }

        [TestMethod]
        public void VerifyQuantiteRule_la_quantite_est_valide_si_qt_receptionnee_moins_qt_facturee_rapprochee_est_supperieur_a_0()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "1",
                QuantiteFactureeRapprochee = "10,5",
                QuantiteReceptionnee = "100",
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(true, result.IsValid, "La quantite est valide si Qté réceptionnée - Qté facturée rapprochée > 0");
        }

        [TestMethod]
        public void VerifyQuantiteRule_la_quantite_n_est_pas_valide_si_QuantiteFactureeRapprochee_est_supperieur_au_autres_quantites()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "10",
                QuantiteFactureeRapprochee = "100",
                QuantiteReceptionnee = "10",
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "La quantite n 'est pas valide si Qté réceptionnée - Qté facturée rapprochée <= 0  et Qté commandée - Qté facturée rapprochée <= 0");
        }

        [TestMethod]
        public void VerifyQuantiteRule_la_quantite_n_est_pas_valide_si_QuantiteFactureeRapprochee_est_supperieur_au_autres_quantites_2()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                QuantiteCommandee = "10",
                QuantiteFactureeRapprochee = "10",
                QuantiteReceptionnee = "10",
            };

            // Act
            var result = unitUnderTest.VerifyQuantiteRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "La quantite n 'est pas valide si Qté réceptionnée - Qté facturée rapprochée <= 0  et Qté commandée - Qté facturée rapprochée <= 0");
            Assert.AreEqual("Rejet ligne n°1 : Qté commandée invalide", result.ErrorMessage, "La quantite n 'est pas valide si Qté réceptionnée - Qté facturée rapprochée <= 0  et Qté commandée - Qté facturée rapprochée <= 0");
        }

        [TestMethod]
        public void VerifyPuhtFormatRule_Si_format_non_valide_rejet_de_la_ligne()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                PuHt = "FQSDQSD8",

            };
            // Act
            var result = unitUnderTest.VerifyPuhtFormatRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(false, result.IsValid, "Si format non valide => rejet de la ligne");
            Assert.AreEqual("Rejet ligne n°1 : PU HT invalide.", result.ErrorMessage, "Si format non valide => rejet de la ligne");

        }

        [TestMethod]
        public void VerifyPuhtFormatRule_Si_format_valide_non_rejet_de_la_ligne()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                PuHt = "99,159",

            };
            // Act
            var result = unitUnderTest.VerifyPuhtFormatRule(repriseExcelCommande);

            // Assert
            Assert.AreEqual(true, result.IsValid, "Si format valide => non rejet de la ligne");
            Assert.AreEqual(string.Empty, result.ErrorMessage, "Si format valide => non rejet de la ligne");

        }

        [TestMethod]
        public void VerifyUniteRule_Si_non_reconnu_rejet_de_la_ligne()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                Unite = "BAD_UNITE",

            };
            ContextForImportCommande context = new ContextForImportCommande();
            context.UnitesUsedInExcel = new System.Collections.Generic.List<Entities.Referential.UniteEnt>
            {
                new Entities.Referential.UniteEnt
                {
                    Carburants = null,
                    Code = "FRT",
                    Libelle = "Forfait",
                    ParametrageReferentielEtendus = null,
                    Taches = null,
                    UniteId = 1
                }
            };

            // Act
            var result = unitUnderTest.VerifyUniteRule(repriseExcelCommande, context);

            // Assert
            Assert.AreEqual(false, result.IsValid, "Si non reconnu => rejet de la ligne.");
            Assert.AreEqual("Rejet ligne n°1 : Code Unité invalide.", result.ErrorMessage, "Si non reconnu => rejet de la ligne.");
        }

        [TestMethod]
        public void VerifyUniteRule_Si_reconnu_non_rejet_de_la_ligne()
        {
            // Arrange
            var unitUnderTest = this.CreateCommandeLignesValidator();
            RepriseExcelCommande repriseExcelCommande = new RepriseExcelCommande()
            {
                NumeroDeLigne = "1",
                Unite = "FRT",

            };
            ContextForImportCommande context = new ContextForImportCommande();
            context.UnitesUsedInExcel = new System.Collections.Generic.List<Entities.Referential.UniteEnt>
            {
                new Entities.Referential.UniteEnt
                {
                    Carburants = null,
                    Code = "FRT",
                    Libelle = "Forfait",
                    ParametrageReferentielEtendus = null,
                    Taches = null,
                    UniteId = 1
                }
            };

            // Act
            var result = unitUnderTest.VerifyUniteRule(repriseExcelCommande, context);

            // Assert
            Assert.AreEqual(true, result.IsValid, "Si reconnu => non rejet de la ligne.");
            Assert.AreEqual(string.Empty, result.ErrorMessage, "Si reconnu => non rejet de la ligne.");
        }

    }
}
