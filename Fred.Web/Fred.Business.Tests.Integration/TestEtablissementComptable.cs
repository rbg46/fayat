using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestEtablissementComptable : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des établissements comptables n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetEtablissementListReturnNotNullList()
        {

            var etablissementsComptables = EtabComptableMgr.GetEtablissementComptableList();
            Assert.IsTrue(etablissementsComptables != null);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void GetNonExistingEtablissementComptableReturnNull()
        {

            EtablissementComptableEnt etablissementComptable = EtabComptableMgr.GetEtablissementComptableById(-1);
            Assert.IsNull(etablissementComptable);
        }

        /// <summary>
        ///   Teste que la liste des établissements comptables n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetEtablissementComptableBySocieteId()
        {

            var etablissementComptables = EtabComptableMgr.GetListBySocieteId(2);
            Assert.IsTrue(etablissementComptables != null);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddEtablissementComptable()
        {
            EtablissementComptableEnt etablissement = new EtablissementComptableEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Etablissement (test de création)",
                SocieteId = 1
            };

            int countBefore = EtabComptableMgr.GetEtablissementComptableList().Count();

            EtabComptableMgr.AddEtablissementComptable(etablissement);

            int countAfter = EtabComptableMgr.GetEtablissementComptableList().Count();

            Assert.IsTrue(countBefore < countAfter);

            //Test cleanup
            EtabComptableMgr.DeleteEtablissementComptableById(etablissement);
        }


        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void ValidateFiltresDepenses()
        {
            SearchDepenseEnt filter = new SearchDepenseEnt
            {
                ValueText = "ValidateFiltresDepenses",
                LibelleAsc = true,
                Libelle = true,
                DateFrom = new DateTime(2016, 12, 7),
                DateTo = new DateTime(2016, 12, 9)
            };

            var depenses = DepenseMgr.SearchDepenseListWithFilter(filter, 1, 20).ToList();
            Assert.IsTrue(depenses != null && depenses.Count == 3);

            double montant = DepenseMgr.GetMontantTotal(filter);
            Assert.IsTrue(montant == 140); // 10 * 1 + 20 * 2 + 30 * 3

        }
        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void DeleteExistingEtablissementComptable()
        {
            EtablissementComptableEnt etablissement = new EtablissementComptableEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Etablissement (test de création)",
                SocieteId = 1
            };

            int etablissementComptableId = EtabComptableMgr.AddEtablissementComptable(etablissement);

            int countBefore = EtabComptableMgr.GetEtablissementComptableList().Count();

            EtabComptableMgr.DeleteEtablissementComptableById(etablissement);

            int countAfter = EtabComptableMgr.GetEtablissementComptableList().Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste de recupération des établissement comptables via societeId
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetListBySocieteId()
        {
            int societeId = 1;

            var etablissementsComptables = EtabComptableMgr.GetListBySocieteId(societeId);
            Assert.IsTrue(etablissementsComptables != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void ImportExistingEtablissementComptable()
        {


            // Déclaration d'un établissement comptable existant
            EtablissementComptableEnt etablissement = new EtablissementComptableEnt
            {
                Code = "01",
                Libelle = "DRN- Idf Est",
                SocieteId = 1
            };
            List<EtablissementComptableEnt> ets = new List<EtablissementComptableEnt>();
            ets.Add(etablissement);
            bool test = EtabComptableMgr.ManageImportedEtablissementComptables(ets);

            Assert.IsFalse(test);
        }

        /// <summary>
        ///   Teste dla réussite d'import d'un nouvel établissement comptable
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void ImportNewEtablissementComptable()
        {
            EtablissementComptableEnt etablissement = new EtablissementComptableEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Etablissement fictif de test",
                SocieteId = 1
            };

            List<EtablissementComptableEnt> ets = new List<EtablissementComptableEnt>();
            ets.Add(etablissement);

            bool test = EtabComptableMgr.ManageImportedEtablissementComptables(ets);

            Assert.IsTrue(test);

            EtabComptableMgr.DeleteEtablissementComptableById(etablissement);
        }
    }
}
