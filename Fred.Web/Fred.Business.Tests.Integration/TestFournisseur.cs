using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestFournisseur : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des fournisseurs n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFournisseurListReturnNotNullList()
        {
            int groupeId = 1;
            var fournisseurs = FournisseurMgr.GetFournisseurList(groupeId);
            Assert.IsTrue(fournisseurs != null);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewFournisseur()
        {

            int groupeId = 1;

            FournisseurEnt fournisseur = new FournisseurEnt
            {
                Code = "TEST DE CREATION",
                Libelle = "Libellé du fournisseur",
                Adresse = "Adresse du fournisseur",
                CodePostal = "34000",
                Ville = "Saint Georges d'Orcques",
                SIRET = "123456789",
                SIREN = "987654321",
                Telephone = "0102030405",
                Fax = "0504030201",
                PaysId = 1,
                Email = "fournisseur@fred.fr",
                ModeReglement = "Mode réglement du fournisseur",
                DateOuverture = DateTime.Now,
                DateCloture = DateTime.Now.AddDays(1),
                TypeSequence = "Type de séquence",
                TypeTiers = "F", // contient le code « F, C, I » de la règle de gestion du compte tiers (TKLEPS).
                GroupeId = groupeId
            };

            int countBefore = FournisseurMgr.GetFournisseurList(groupeId).Count();

            FournisseurMgr.AddFournisseur(fournisseur);

            int countAfter = FournisseurMgr.GetFournisseurList(groupeId).Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateExistingFournisseur()
        {

            int groupeId = 1;

            FournisseurEnt fournisseurBefore = new FournisseurEnt
            {
                Code = "TEST0002",
                Libelle = "Fournisseur de test de mise à jour",
                GroupeId = groupeId
            };

            int fournisseurId = FournisseurMgr.AddFournisseur(fournisseurBefore).FournisseurId;

            string libBefore = fournisseurBefore.Libelle;
            fournisseurBefore.Libelle = "Fournisseur de test après modification";

            FournisseurMgr.UpdateFournisseur(fournisseurBefore);

            FournisseurEnt fournisseurAfter = FournisseurMgr.GetFournisseur(fournisseurId, groupeId);

            Assert.AreNotEqual(libBefore, fournisseurAfter.Libelle);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteExistingFournisseur()
        {

            int groupeId = 1;

            FournisseurEnt fournisseur = new FournisseurEnt
            {
                Code = "TEST0003",
                Libelle = "Fournisseur de test de suppression",
                GroupeId = groupeId
            };

            int fournisseurId = FournisseurMgr.AddFournisseur(fournisseur).FournisseurId;
            int countBefore = FournisseurMgr.GetFournisseurList(groupeId).Count();
            FournisseurMgr.DeleteFournisseurById(fournisseurId);
            int countAfter = FournisseurMgr.GetFournisseurList(groupeId).Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste recherche des tâches
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFournisseurListReturnAtLeastOneRecord()
        {
            int groupeId = 1;

            var fournisseurs = FournisseurMgr.GetFournisseurList(groupeId).ToList();
            Assert.IsTrue(fournisseurs.Count > 0);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingFournisseurReturnNull()
        {

            FournisseurEnt fournisseur = FournisseurMgr.GetFournisseur(-1, 1);
            Assert.IsNull(fournisseur);
        }

        /// <summary>
        ///   Teste la validité d'un Fournisseur importé depuis ANAËL
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void CheckValidityFournisseurBeforeImportation()
        {

            int groupeId = 1;

            FournisseurEnt fournisseurEnt = new FournisseurEnt
            {
                Code = "Test le " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Libelle = "CheckValidityFournisseurBeforeImportation",
                TypeSequence = "403xxx",
                TypeTiers = "F",
                GroupeId = groupeId
            };

            var fournisseurs = new List<FournisseurEnt>();
            fournisseurs.Add(fournisseurEnt);

            FournisseurMgr.ManageImportedFournisseurs(fournisseurs, "1000", groupeId);

            fournisseurEnt.TypeTiers = "X";
            Assert.IsFalse(FournisseurMgr.CheckValidityBeforeImportation(fournisseurEnt));

            fournisseurEnt.TypeSequence = "X";
            Assert.IsFalse(FournisseurMgr.CheckValidityBeforeImportation(fournisseurEnt));
        }

        [TestMethod]
        [TestCategory("DBDepend")]

        public void TestSearchWithFilters()
        {

            SearchFournisseurEnt searchEnt = new SearchFournisseurEnt
            {
                Departement = "37"
            };
            List<FournisseurEnt> list = FournisseurMgr.SearchFournisseurWithFilters(searchEnt, 1, 20).ToList();
        }
    }
}
