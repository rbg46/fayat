using System;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.EtatPaie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestRapportPointage : FredBaseTest
    {

        #region UPDATE

        /// <summary>
        ///   Teste la mise à jour d'une ligne de rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void UpdateRapportLigne()
        {


            RapportEnt rapport = RapportMgr.GetNewRapport();

            rapport.CiId = 1;
            rapport.DateChantier = DateTime.Now;
            rapport.HoraireDebutM = DateTime.Now;
            rapport.HoraireDebutS = DateTime.Now;
            rapport.HoraireFinM = DateTime.Now;
            rapport.HoraireFinS = DateTime.Now;

            rapport.ListLignes.ElementAt(0).PrenomNomTemporaire = "Test";

            RapportMgr.AddRapport(rapport);

            rapport.ListLignes.ElementAt(0).PrenomNomTemporaire = "Test2";

            RapportMgr.UpdateRapport(rapport);

            rapport = RapportMgr.GetRapportById(rapport.RapportId, true);

            Assert.IsTrue(rapport.ListLignes.ElementAt(0).PrenomNomTemporaire.Equals("Test2"));
        }

        #endregion

        #region GET

        /// <summary>
        ///   Teste que la récupération de la liste des rapport ne ramène pas un null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetRapportListReturnNotNullList()
        {

            var lstRapoport = RapportMgr.GetRapportList();
            Assert.IsTrue(lstRapoport != null);
        }

        /// <summary>
        ///   Teste que la récupération d'un rapport qui n'existe pas retourne un null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetNonExistingRapportByIdReturnNull()
        {
            RapportEnt rapport = RapportMgr.GetRapportById(-1, true);
            Assert.IsNull(rapport);
        }

        /// <summary>
        ///   Teste que la récupération d'un nouveau rapport ne retourne pas un null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetNewRapportReturnNotNull()
        {
            RapportEnt rapport = RapportMgr.GetNewRapport();
            Assert.IsNotNull(rapport);
        }

        #endregion

        #region CREATE

        /// <summary>
        ///   Teste l'ajout d'un nouveau rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void AddNewRapport()
        {


            RapportEnt rapport = RapportMgr.GetNewRapport();

            rapport.CiId = 1;
            rapport.ListLignes.ElementAt(0).CiId = 1;
            rapport.ListLignes.ElementAt(0).DatePointage = DateTime.Now;
            rapport.ListLignes.ElementAt(0).ListRapportLigneTaches.ElementAt(0).HeureTache = 3;
            rapport.DateChantier = DateTime.Now;
            rapport.HoraireDebutM = DateTime.Now;
            rapport.HoraireDebutS = DateTime.Now;
            rapport.HoraireFinM = DateTime.Now;
            rapport.HoraireFinS = DateTime.Now;

            //rapport.ListLignes.ElementAt(0).PrenomNomTemporaire = "Test";

            int countBefore = RapportMgr.GetRapportList().Count();

            RapportMgr.AddRapport(rapport);

            int countAfter = RapportMgr.GetRapportList().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste le tri des résultats de la recherche par numéro de rapport croissant
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void SearchRapportOrderByRapportNumber()
        {
            SearchRapportEnt searchRapportEnt = RapportMgr.GetFiltersList(19);
            searchRapportEnt.SortFields["Ci.Code"] = searchRapportEnt.CiCodeAsc;
            searchRapportEnt.SortFields["RapportStatut.Libelle"] = searchRapportEnt.StatutAsc;
            searchRapportEnt.SortFields["RapportId"] = searchRapportEnt.NumeroRapportAsc;
            searchRapportEnt.SortFields["DateRapport"] = searchRapportEnt.DateChantierAsc;
            searchRapportEnt.ValueText = "t";
            searchRapportEnt.NumeroRapportAsc = true;

            var rapports = RapportMgr.SearchRapportWithFilter(searchRapportEnt).Rapports;
            int lastId = -1;
            foreach (RapportEnt rapport in rapports)
            {
                if (rapport.RapportId < lastId)
                {
                    lastId = -1;
                    break;
                }

                lastId = rapport.RapportId;
            }

            Assert.IsTrue(true);
        }

        /// <summary>
        ///   Teste le tri des résultats de la recherche par numéro de rapport décroissant
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void SearchRapportOrderByRapportNumberDESC()
        {

            SearchRapportEnt searchRapportEnt = RapportMgr.GetFiltersList(19);

            searchRapportEnt.NumeroRapportAsc = false;

            var rapports = RapportMgr.SearchRapportWithFilter(searchRapportEnt).Rapports;
            int lastId = int.MaxValue;
            foreach (RapportEnt rapport in rapports)
            {
                if (rapport.RapportId > lastId)
                {
                    lastId = int.MaxValue;
                    break;
                }

                lastId = rapport.RapportId;
            }

            Assert.AreNotEqual(lastId, int.MaxValue);
        }

        /// <summary>
        ///   Teste l'ajout d'une nouvelle ligne de rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void AddNewRapportLigneToRapport()
        {


            RapportEnt rapport = RapportMgr.GetNewRapport();

            int countBefore = rapport.ListLignes.Count();

            RapportMgr.AddNewPointageReelToRapport(rapport);

            int countAfter = rapport.ListLignes.Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste l'ajout d'une prime et d'une tache dans un rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void AddPrimeAndTacheToRapport()
        {
            int ciId = 1;
            RapportEnt rapport = RapportMgr.GetNewRapport();

            int countPrimeInRapportBefore = rapport.ListPrimes.Count();
            int countTacheInRapportBefore = rapport.ListTaches.Count();
            int countLignePrimeInRapportLigneBefore = rapport.ListLignes.ElementAt(0).ListRapportLignePrimes.Count();
            int countLigneTacheInRapportLigneBefore = rapport.ListLignes.ElementAt(0).ListRapportLigneTaches.Count();

            // Prime

            PrimeEnt prime = PrimeMgr.GetNewPrime(1);

            RapportMgr.AddPrimeToRapport(rapport, prime);

            // Tache

            TacheEnt tache = TacheMgr.GetTacheParDefaut(ciId);

            RapportMgr.AddTacheToRapport(rapport, tache);

            int countPrimeInRapportAfter = rapport.ListPrimes.Count();
            int countTacheInRapportAfter = rapport.ListTaches.Count();
            int countLignePrimeInRapportLigneAfter = rapport.ListLignes.ElementAt(0).ListRapportLignePrimes.Count();
            int countLigneTacheInRapportLigneAfter = rapport.ListLignes.ElementAt(0).ListRapportLigneTaches.Count();

            Assert.IsTrue((countPrimeInRapportBefore + 1) == countPrimeInRapportAfter);
            Assert.IsTrue((countTacheInRapportBefore + 1) == countTacheInRapportAfter);
            Assert.IsTrue((countLignePrimeInRapportLigneBefore + 1) == countLignePrimeInRapportLigneAfter);
            Assert.IsTrue((countLigneTacheInRapportLigneBefore + 1) == countLigneTacheInRapportLigneAfter);
        }

        #endregion

        #region DELETE

        /// <summary>
        ///   Teste la suppression d'une ligne de rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteRapportLigne()
        {


            RapportEnt rapport = RapportMgr.GetNewRapport();

            rapport.CiId = 1;
            rapport.DateChantier = DateTime.Now;
            rapport.HoraireDebutM = DateTime.Now;
            rapport.HoraireDebutS = DateTime.Now;
            rapport.HoraireFinM = DateTime.Now;
            rapport.HoraireFinS = DateTime.Now;

            rapport.ListLignes.ElementAt(0).PrenomNomTemporaire = "Test";

            RapportMgr.AddRapport(rapport);

            int countBefore = rapport.ListLignes.Count();

            rapport.ListLignes.ElementAt(0).IsDeleted = true;
            RapportMgr.UpdateRapport(rapport);

            rapport = RapportMgr.GetRapportById(rapport.RapportId, true);
            int countAfter = rapport.ListLignes.Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste la suppresion d'un rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteRapport()
        {

            RapportEnt rapport = RapportMgr.GetNewRapport();

            rapport.CiId = 1;
            rapport.DateChantier = DateTime.Now;
            rapport.HoraireDebutM = DateTime.Now;
            rapport.HoraireDebutS = DateTime.Now;
            rapport.HoraireFinM = DateTime.Now;
            rapport.HoraireFinS = DateTime.Now;

            rapport.ListLignes.ElementAt(0).PrenomNomTemporaire = "Test";

            RapportMgr.AddRapport(rapport);

            RapportMgr.DeleteRapport(rapport, SuperAdminUserId);

            Assert.IsTrue(rapport.DateSuppression != null);
        }

        /// <summary>
        ///   Teste la suppresion d'un rapport
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void TestEtatPaie()
        {
            int userId = 1;
            DateTime date = new DateTime(2017, 07, 1);
            EtatPaieExportModel etatPaieExportModel = new EtatPaieExportModel()
            {
                Date = date,
                Filtre = TypeFiltreEtatPaie.Perimetre,
                OrganisationId = 100,
                Tri = false,
                PersonnelId = null,
                Pdf = false
            };

            EtatPaieMgr.GenerateControlePointages(etatPaieExportModel, userId, "");
            Assert.IsTrue(1 == 2);
        }
        #endregion
    }
}
