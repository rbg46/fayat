using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Fred.Business.OperationDiverse;
using Fred.Common.Tests;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.OperationDiverse
{
    [TestClass]
    public class OperationDiverseMapperTest : BaseTu<OperationDiverseMapper>
    {
        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithEmptyNatureList()
        {
            OperationDiverseCommandeFtpModel model1 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            OperationDiverseCommandeFtpModel model2 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel> { model1, model2 };

            List<NatureEnt> natures = new List<NatureEnt>();

            Invoking(() => Actual.MapOperationDiverseWithNature(operationDiverses, natures)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithNature(operationDiverses, natures).Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithNatureList()
        {
            OperationDiverseCommandeFtpModel model1 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            OperationDiverseCommandeFtpModel model2 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel> { model1, model2 };

            NatureEnt natureEnt = new NatureEnt
            {
                NatureId = 1,
                RessourceId = 100,
                Code = "Test"
            };

            List<NatureEnt> natures = new List<NatureEnt> { natureEnt };

            Invoking(() => Actual.MapOperationDiverseWithNature(operationDiverses, natures)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithNature(operationDiverses, natures).Should().NotBeEmpty();

            List<OperationDiverseCommandeFtpModel> result = Actual.MapOperationDiverseWithNature(operationDiverses, natures);
            
            foreach(OperationDiverseCommandeFtpModel item in  result)
            {
                item.RessourceId.Should().Be(100);
                item.RessourceCode.Should().Be("Test");
            }
        }

        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithEmptyODListWithNatureList()
        {
            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel>();

            NatureEnt natureEnt = new NatureEnt
            {
                NatureId = 1,
                RessourceId = 100,
                Code = "Test"
            };

            List<NatureEnt> natures = new List<NatureEnt> { natureEnt };

            Invoking(() => Actual.MapOperationDiverseWithNature(operationDiverses, natures)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithNature(operationDiverses, natures).Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithEmptyRessourceList()
        {
            OperationDiverseCommandeFtpModel model1 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            OperationDiverseCommandeFtpModel model2 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L"
            };

            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel> { model1, model2 };

            List<RessourceEnt> ressources = new List<RessourceEnt>();

            Invoking(() => Actual.MapOperationDiverseWithRessource(operationDiverses, ressources)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithRessource(operationDiverses, ressources).Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithRessourceList()
        {
            OperationDiverseCommandeFtpModel model1 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L",
                RessourceCode = "Test"
            };

            OperationDiverseCommandeFtpModel model2 = new OperationDiverseCommandeFtpModel
            {
                CiId = 1,
                CodeCi = "Test",
                CodeDevise = "EUR",
                CodeMateriel = "Materiel",
                CodeRef = "CodeRef",
                CommandeId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                FamilleOperationDiverseCode = "FOD",
                FamilleOperationDiverseId = 1,
                Libelle = "Libelle",
                Montant = 100,
                MontantCommande = 100,
                MontantDevise = 100,
                MontantODCommande = 100,
                NatureId = 1,
                NumeroCommande = "NumeroCommande",
                NumeroPiece = "NuméroPiece",
                PersonnelCode = "CodePersonnel",
                Quantite = 1,
                QuantiteCommande = 1,
                RapportLigneId = 1,
                SapCodeNature = "SAP",
                SocieteCode = "CodeSociete",
                SocieteMaterielCode = "CodeMaterielSociete",
                TacheId = 1,
                Unite = "L",
                RessourceCode = "Test"
            };

            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel> { model1, model2 };

            RessourceEnt ressourceEnt = new RessourceEnt
            {
                RessourceId = 100,
                Code = "Test"
            };

            List<RessourceEnt> ressources = new List<RessourceEnt> { ressourceEnt };

            Invoking(() => Actual.MapOperationDiverseWithRessource(operationDiverses, ressources)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithRessource(operationDiverses, ressources).Should().NotBeEmpty();

            List<OperationDiverseCommandeFtpModel> result = Actual.MapOperationDiverseWithRessource(operationDiverses, ressources);

            foreach (OperationDiverseCommandeFtpModel item in result)
            {
                item.RessourceId.Should().Be(100);
                item.RessourceCode.Should().Be("Test");
            }
        }

        [TestMethod]
        [TestCategory("OperationDiverseManagerMapper")]
        public void MapOperationDiverseWithEmptyODListWithRessourceList()
        {
            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel>();

            RessourceEnt ressourceEnt = new RessourceEnt
            {
                RessourceId = 100,
                Code = "Test"
            };

            List<RessourceEnt> ressources = new List<RessourceEnt> { ressourceEnt };

            Invoking(() => Actual.MapOperationDiverseWithRessource(operationDiverses, ressources)).Should().NotThrow<Exception>();
            Actual.MapOperationDiverseWithRessource(operationDiverses, ressources).Should().BeEmpty();
        }
    }
}
