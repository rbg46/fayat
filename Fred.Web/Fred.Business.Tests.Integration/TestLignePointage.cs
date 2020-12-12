using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.DatesCalendrierPaie;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.EtatPaie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestLignePointage : FredBaseTest
    {


        #region GET

        /// <summary>
        ///   Teste de la recherche des pointages
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void SearchPointageWithDefaultFilter()
        {

            SearchRapportLigneEnt searchRapportLigneEnt = PointageMgr.GetFiltersList();
            var rapports = PointageMgr.SearchPointageReelWithFilter(searchRapportLigneEnt, true);

            if (rapports == null)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(!rapports.Any());
            }
        }

        /// <summary>
        ///   Test d'utilisation de GetListePointageMensuel
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GetListePointageMensuel()
        {
            try
            {
                // Basé sur le fait que le personnelId = 1 a 7 lignes de pointages min
                // défini à l'initialisation de la base de donnée
                EtatPaieExportModel etatPaieExportModel = new EtatPaieExportModel()
                {
                    Annee = 2017,
                    Mois = 1,
                    Filtre = TypeFiltreEtatPaie.Perimetre,
                    OrganisationId = 0,
                    Tri = true,
                    PersonnelId = 1
                };
                var listPointageBase = PointageMgr
                  .GetListePointageMensuel(etatPaieExportModel)
                  .ToList();

                Assert.IsTrue(listPointageBase.Count >= 7);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        #endregion



        #region DUPLICATE

        #endregion

        #region UPDATE

        #endregion

        #region Generation pointage anticipé

        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GenererUnMoisSansPointage()
        {
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(9);
            UtilisateurEnt utilisateur = UserMgr.GetUtilisateurById(3);
            //// PointageMgr.GenerePointageAnticipe(personnel, new DateTime(2017, 12, 1), new DateTime(2018, 1, 1), utilisateur);

            SearchRapportLigneEnt search = new SearchRapportLigneEnt();
            search.DatePointageMin = new DateTime(2017, 12, 1);
            search.DatePointageMax = new DateTime(2018, 1, 1);
            search.PersonnelId = personnel.PersonnelId;

            var pointagesAnticipes = new List<PointageAnticipeEnt>();////PointageMgr.SearchPointageAnticipeWithFilter(search, true);
            Assert.IsTrue(!pointagesAnticipes.Any()
                          || !pointagesAnticipes.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Any());
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GenererDeuxMoisSansEtAvecPointage()
        {
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(1);
            UtilisateurEnt utilisateur = UserMgr.GetUtilisateurById(19);
            //// PointageMgr.GenerePointageAnticipe(personnel, new DateTime(2017, 12, 1), new DateTime(2018, 2, 1), utilisateur);

            //Ajout recherche pointage réel sur le mois
            SearchRapportLigneEnt searchReels = new SearchRapportLigneEnt();
            searchReels.DatePointageMin = new DateTime(2018, 1, 1);
            searchReels.DatePointageMax = new DateTime(2018, 2, 1);
            searchReels.PersonnelId = personnel.PersonnelId;

            var pointagesReels = PointageMgr.SearchPointageReelWithFilter(searchReels, true);

            RapportLigneEnt dernierPointageReel = null;
            int countAnticipe = 0;

            if (pointagesReels != null && pointagesReels.Any())
            {
                dernierPointageReel = pointagesReels.LastOrDefault();
            }

            SearchRapportLigneEnt searchAnticipes = new SearchRapportLigneEnt();
            searchAnticipes.DatePointageMin = new DateTime(2018, 1, 1);
            searchAnticipes.DatePointageMax = new DateTime(2018, 2, 1);
            searchAnticipes.PersonnelId = personnel.PersonnelId;

            //// var pointagesAnticipes = PointageMgr.SearchPointageAnticipeWithFilter(searchAnticipes, true);
            var pointagesAnticipes = new List<PointageAnticipeEnt>();
            //Ajout recherche pointage anticipé sur le mois
            DatesCalendrierPaieEnt calendrierPaieEnt = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(personnel.SocieteId.Value, 2018, 1);
            DateTime dateDebutGeneration = new DateTime(2018, 1, 1);
            DateTime dateFinGeneration = dateDebutGeneration.AddMonths(1);
            if (calendrierPaieEnt != null && calendrierPaieEnt.DateFinPointages.HasValue)
            {
                dateDebutGeneration = new DateTime(calendrierPaieEnt.DateFinPointages.Value.Year, calendrierPaieEnt.DateFinPointages.Value.Month, calendrierPaieEnt.DateFinPointages.Value.Day);
            }

            while (dateDebutGeneration.Date.CompareTo(dateFinGeneration.Date) < 0)
            {
                var pointagesA = pointagesAnticipes.Where(p => p.DatePointage.Date.CompareTo(dateDebutGeneration.Date) == 0);
                var pointagesR = pointagesReels.Where(p => p.DatePointage.Date.CompareTo(dateDebutGeneration.Date) == 0);


                Assert.IsTrue( //Vérification si un pointage n'a pas été généré à tord
                              (pointagesA.Any() || pointagesR.Any())
                              && !pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Any()
                              ||

                              //Vérification qu'un seul pointage a été généré
                              dernierPointageReel != null && pointagesA.Count() == 1 && !pointagesR.Any() && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1
                              ||
                              !pointagesA.Any() && (dateDebutGeneration.DayOfWeek == DayOfWeek.Saturday || dateDebutGeneration.DayOfWeek == DayOfWeek.Sunday)
                             );

                if (dernierPointageReel != null
                    && pointagesA.Count() == 1
                    && !pointagesR.Any()
                    && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1)
                {
                    PointageAnticipeEnt pointage = pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).FirstOrDefault();

                    //TODO ASSERT SUR LES ABSENCES

                    Assert.AreEqual(pointage.CiId, dernierPointageReel.CiId);
                    Assert.AreEqual(pointage.PersonnelId, dernierPointageReel.PersonnelId);
                    Assert.AreEqual(pointage.AuteurCreationId, utilisateur.UtilisateurId);
                    Assert.IsTrue(pointage.DatePointage.Date.CompareTo(dateDebutGeneration.Date) == 0);

                    Assert.AreEqual(pointage.ListePrimes.Count, dernierPointageReel.ListePrimes.Count);

                    Assert.AreEqual(pointage.CodeMajorationId, dernierPointageReel.CodeMajorationId);
                    Assert.AreEqual(pointage.HeureMajoration, dernierPointageReel.HeureMajoration);

                    IndemniteDeplacementEnt indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(dernierPointageReel, searchAnticipes.DatePointageMin.Date);
                    Assert.AreEqual(pointage.DeplacementIV, indemniteDeplacement.IVD);
                    Assert.AreEqual(pointage.CodeDeplacementId, indemniteDeplacement.CodeDeplacementId);
                    Assert.AreEqual(pointage.CodeZoneDeplacementId, indemniteDeplacement.CodeZoneDeplacementId);

                    Assert.AreEqual(pointage.HeureNormale, dernierPointageReel.HeureNormale);
                }

                dateDebutGeneration = dateDebutGeneration.AddDays(1);
            }

            if (dernierPointageReel == null)
            {
                Assert.AreEqual(countAnticipe, 0);
            }
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GenererDateClotuPaie()
        {
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(1);
            UtilisateurEnt utilisateur = UserMgr.GetUtilisateurById(19);
            //// PointageMgr.GenerePointageAnticipe(personnel, new DateTime(2018, 3, 1), new DateTime(2018, 4, 1), utilisateur);

            //Ajout recherche pointage réel sur le mois
            SearchRapportLigneEnt searchReels = new SearchRapportLigneEnt();
            searchReels.DatePointageMin = new DateTime(2018, 3, 1);
            searchReels.DatePointageMax = new DateTime(2018, 4, 1);
            searchReels.PersonnelId = personnel.PersonnelId;

            var pointagesReels = PointageMgr.SearchPointageReelWithFilter(searchReels, true);

            RapportLigneEnt dernierPointageReel = null;

            if (pointagesReels != null && pointagesReels.Any())
            {
                dernierPointageReel = pointagesReels.LastOrDefault();
            }

            SearchRapportLigneEnt searchAnticipes = new SearchRapportLigneEnt();
            searchAnticipes.DatePointageMin = new DateTime(2018, 3, 1);
            searchAnticipes.DatePointageMax = new DateTime(2018, 4, 1);
            searchAnticipes.PersonnelId = personnel.PersonnelId;

            var pointagesAnticipes = new List<PointageAnticipeEnt>(); ////PointageMgr.SearchPointageAnticipeWithFilter(searchAnticipes, true);

            //Ajout recherche pointage anticipé sur le mois
            DatesCalendrierPaieEnt calendrierPaieEnt = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(personnel.SocieteId.Value, 2018, 3);
            DateTime dateDebutGeneration = new DateTime(2018, 3, 1);
            DateTime dateFinGeneration = dateDebutGeneration.AddMonths(1);
            if (calendrierPaieEnt != null && calendrierPaieEnt.DateFinPointages.HasValue)
            {
                dateDebutGeneration = new DateTime(calendrierPaieEnt.DateFinPointages.Value.Year, calendrierPaieEnt.DateFinPointages.Value.Month, calendrierPaieEnt.DateFinPointages.Value.Day);
            }

            while (searchAnticipes.DatePointageMin.Date.CompareTo(dateFinGeneration.Date) < 0)
            {
                var pointagesA = pointagesAnticipes.Where(p => p.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);
                var pointagesR = pointagesReels.Where(p => p.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);

                Assert.IsTrue(
                              searchAnticipes.DatePointageMin.Date.CompareTo(dateDebutGeneration.Date) < 0
                              &&
                              !pointagesA.Where(p => p.DateCreation.Value.Date.CompareTo(DateTime.Now.Date) == 0).Any() || searchAnticipes.DatePointageMin.Date.CompareTo(dateDebutGeneration.Date) >= 0
                              &&
                              (dernierPointageReel != null && pointagesA.Count() == 1 && !pointagesR.Any() && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1
                               ||
                               !pointagesA.Any() && (searchAnticipes.DatePointageMin.DayOfWeek == DayOfWeek.Saturday || searchAnticipes.DatePointageMin.DayOfWeek == DayOfWeek.Sunday)
                              ) || (pointagesA.Any() || pointagesR.Any())
                              && !pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Any()
                             );

                if (dernierPointageReel != null
                    && pointagesA.Count() == 1
                    && !pointagesR.Any()
                    && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1)
                {
                    PointageAnticipeEnt pointage = pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).FirstOrDefault();

                    // TODO ASSERT SUR LES ABSENCES

                    Assert.AreEqual(pointage.CiId, dernierPointageReel.CiId);
                    Assert.AreEqual(pointage.PersonnelId, dernierPointageReel.PersonnelId);
                    Assert.AreEqual(pointage.AuteurCreationId, utilisateur.UtilisateurId);
                    Assert.IsTrue(pointage.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);

                    Assert.AreEqual(pointage.ListePrimes.Count, dernierPointageReel.ListePrimes.Count);

                    Assert.AreEqual(pointage.CodeMajorationId, dernierPointageReel.CodeMajorationId);
                    Assert.AreEqual(pointage.HeureMajoration, dernierPointageReel.HeureMajoration);

                    IndemniteDeplacementEnt indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(dernierPointageReel, searchAnticipes.DatePointageMin.Date);
                    Assert.AreEqual(pointage.DeplacementIV, indemniteDeplacement.IVD);
                    Assert.AreEqual(pointage.CodeDeplacementId, indemniteDeplacement.CodeDeplacementId);
                    Assert.AreEqual(pointage.CodeZoneDeplacementId, indemniteDeplacement.CodeZoneDeplacementId);

                    Assert.AreEqual(pointage.HeureNormale, dernierPointageReel.HeureNormale);
                }

                searchAnticipes.DatePointageMin = searchAnticipes.DatePointageMin.AddDays(1);
            }
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GenererDateClotuPaieAvecPointage()
        {
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(1);
            UtilisateurEnt utilisateur = UserMgr.GetUtilisateurById(19);
            /////PointageMgr.GenerePointageAnticipe(personnel, new DateTime(2018, 4, 1), new DateTime(2018, 5, 1), utilisateur);

            //Ajout recherche pointage réel sur le mois
            SearchRapportLigneEnt searchReels = new SearchRapportLigneEnt();
            searchReels.DatePointageMin = new DateTime(2018, 4, 1);
            searchReels.DatePointageMax = new DateTime(2018, 5, 1);
            searchReels.PersonnelId = personnel.PersonnelId;

            var pointagesReels = PointageMgr.SearchPointageReelWithFilter(searchReels, true);

            RapportLigneEnt dernierPointageReel = null;

            if (pointagesReels != null && pointagesReels.Any())
            {
                dernierPointageReel = pointagesReels.LastOrDefault();
            }

            SearchRapportLigneEnt searchAnticipes = new SearchRapportLigneEnt();
            searchAnticipes.DatePointageMin = new DateTime(2018, 4, 1);
            searchAnticipes.DatePointageMax = new DateTime(2018, 5, 1);
            searchAnticipes.PersonnelId = personnel.PersonnelId;

            var pointagesAnticipes = new List<PointageAnticipeEnt>(); ///// PointageMgr.SearchPointageAnticipeWithFilter(searchAnticipes, true);

            //Ajout recherche pointage anticipé sur le mois
            DatesCalendrierPaieEnt calendrierPaieEnt = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(personnel.SocieteId.Value, 2018, 4);
            DateTime dateDebutGeneration = new DateTime(2018, 4, 1);
            DateTime dateFinGeneration = dateDebutGeneration.AddMonths(1);
            if (calendrierPaieEnt != null && calendrierPaieEnt.DateFinPointages.HasValue)
            {
                dateDebutGeneration = new DateTime(calendrierPaieEnt.DateFinPointages.Value.Year, calendrierPaieEnt.DateFinPointages.Value.Month, calendrierPaieEnt.DateFinPointages.Value.Day);
            }

            while (searchAnticipes.DatePointageMin.Date.CompareTo(dateFinGeneration.Date) < 0)
            {
                var pointagesA = pointagesAnticipes.Where(p => p.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);
                var pointagesR = pointagesReels.Where(p => p.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);

                Assert.IsTrue(
                              searchAnticipes.DatePointageMin.Date.CompareTo(dateDebutGeneration.Date) < 0
                              &&
                              !pointagesA.Where(p => p.DateCreation.Value.Date.CompareTo(DateTime.Now.Date) == 0).Any() || searchAnticipes.DatePointageMin.Date.CompareTo(dateDebutGeneration.Date) >= 0
                              &&
                              (dernierPointageReel != null && pointagesA.Count() == 1 && !pointagesR.Any() && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1
                               ||
                               !pointagesA.Any() && (searchAnticipes.DatePointageMin.DayOfWeek == DayOfWeek.Saturday || searchAnticipes.DatePointageMin.DayOfWeek == DayOfWeek.Sunday)
                              ) || (pointagesA.Any() || pointagesR.Any())
                              && !pointagesA.Any(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0)
                             );

                if (dernierPointageReel != null
                    && pointagesA.Count() == 1
                    && !pointagesR.Any()
                    && pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).Count() == 1)
                {
                    PointageAnticipeEnt pointage = pointagesA.Where(p => p.DateCreation.Value.CompareTo(DateTime.Now.Date) == 0).FirstOrDefault();

                    //TODO ASSERT SUR LES ABSENCES

                    Assert.AreEqual(pointage.CiId, dernierPointageReel.CiId);
                    Assert.AreEqual(pointage.PersonnelId, dernierPointageReel.PersonnelId);
                    Assert.AreEqual(pointage.AuteurCreationId, utilisateur.UtilisateurId);
                    Assert.IsTrue(pointage.DatePointage.Date.CompareTo(searchAnticipes.DatePointageMin.Date) == 0);

                    Assert.AreEqual(pointage.ListePrimes.Count, dernierPointageReel.ListePrimes.Count);

                    Assert.AreEqual(pointage.CodeMajorationId, dernierPointageReel.CodeMajorationId);
                    Assert.AreEqual(pointage.HeureMajoration, dernierPointageReel.HeureMajoration);

                    IndemniteDeplacementEnt indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(dernierPointageReel, searchAnticipes.DatePointageMin.Date);
                    Assert.AreEqual(pointage.DeplacementIV, indemniteDeplacement.IVD);
                    Assert.AreEqual(pointage.CodeDeplacementId, indemniteDeplacement.CodeDeplacementId);
                    Assert.AreEqual(pointage.CodeZoneDeplacementId, indemniteDeplacement.CodeZoneDeplacementId);

                    Assert.AreEqual(pointage.HeureNormale, dernierPointageReel.HeureNormale);
                }

                searchAnticipes.DatePointageMin = searchAnticipes.DatePointageMin.AddDays(1);
            }
        }

        #endregion
    }
}
