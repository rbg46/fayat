using System;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestIndemniteDeplacement : FredBaseTest
    {
        private const int PersonnelId = 1;
        private static IndemniteDeplacementEnt indemniteDeplacement;

        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
            indemniteDeplacement = new IndemniteDeplacementEnt
            {
                AuteurCreation = SuperAdminUserId,
                DateCreation = DateTime.Now,
                CiId = 1,
                CodeDeplacementId = 1,
                CodeZoneDeplacementId = 2,
                NombreKilometres = 100.56,
                PersonnelId = PersonnelId,
                SaisieManuelle = true
            };
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementListAll()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementListAll();
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementList()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementList();
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementByPersonnelListAll()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementByPersonnelListAll(3);
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementByPersonnelList()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementByPersonnelList(3);
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementByCIListAll()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementByCIListAll(1);
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementByCIList()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementByCIList(1);
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetIndemniteDeplacementByPersonnelAndCiList()
        {
            var listIndemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementByCi(3, 1);
            Assert.IsTrue(listIndemniteDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddIndemniteDeplacement()
        {
            int nbIndemniteDeplacementBefore = IndemniteDepMgr.GetIndemniteDeplacementListAll().Count();

            int id = IndemniteDepMgr.AddIndemniteDeplacement(indemniteDeplacement);
            int nbIndemniteDeplacementAfter = IndemniteDepMgr.GetIndemniteDeplacementListAll().Count();

            Assert.IsTrue(nbIndemniteDeplacementAfter == (nbIndemniteDeplacementBefore + 1));
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void UpdateIndemniteDeplacement()
        {
            IndemniteDeplacementEnt indemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementById(1);
            double distanceBefore = indemniteDeplacement.NombreKilometres;

            indemniteDeplacement.NombreKilometres = distanceBefore++;
            indemniteDeplacement.DateModification = DateTime.Now;
            indemniteDeplacement.AuteurModification = SuperAdminUserId;

            IndemniteDepMgr.UpdateIndemniteDeplacement(indemniteDeplacement);
            double distanceAfter = indemniteDeplacement.NombreKilometres;
            Assert.IsTrue(distanceBefore != distanceAfter);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void DeleteIndemniteDeplacementById()
        {
            int id = IndemniteDepMgr.AddIndemniteDeplacement(indemniteDeplacement);
            int nbIndemniteDeplacementBefore = IndemniteDepMgr.GetIndemniteDeplacementList().Count();
            IndemniteDepMgr.DeleteIndemniteDeplacementById(id);
            int nbIndemniteDeplacementAfter = IndemniteDepMgr.GetIndemniteDeplacementList().Count();
            Assert.IsTrue(nbIndemniteDeplacementAfter < nbIndemniteDeplacementBefore);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculKmVO()
        {
            IndemniteDeplacementEnt indemniteDeplacement = IndemniteDepMgr.GetIndemniteDeplacementById(1);

            Assert.IsTrue(indemniteDeplacement.NombreKilometreVOChantierRattachement.HasValue && indemniteDeplacement.NombreKilometreVODomicileChantier.HasValue &&
                          indemniteDeplacement.NombreKilometreVODomicileRattachement.HasValue);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetZoneByDistanceDownTo0AndUpTo50IsNull()
        {
            CodeZoneDeplacementEnt zone = IndemniteDepMgr.GetZoneByDistance(1, -1);
            Assert.IsNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 60);
            Assert.IsNull(zone);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetZoneByDistanceBetween0And50IsNotNull()
        {
            CodeZoneDeplacementEnt zone = IndemniteDepMgr.GetZoneByDistance(1, 0);
            Assert.IsNotNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 6);
            Assert.IsNotNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 11);
            Assert.IsNotNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 22);
            Assert.IsNotNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 35);
            Assert.IsNotNull(zone);
            zone = IndemniteDepMgr.GetZoneByDistance(1, 50);
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemniteWithEtablissementDoesntManageDeplacementIsEmpty()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);

            if (personnel.EtablissementRattachement != null)
            {
                personnel.EtablissementRattachement.GestionIndemnites = false;
            }

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacement(personnel, ci);

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement == null && indemniteDeplacement.CodeZoneDeplacement == null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemniteWithEtablissementHorsRegion()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);

            if (personnel.EtablissementRattachement != null)
            {
                personnel.EtablissementRattachement.HorsRegion = true;
            }

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacement(personnel, ci);

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement != null || indemniteDeplacement.CodeZoneDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemniteWithEtablissementRegion()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);

            if (personnel.EtablissementRattachement != null)
            {
                personnel.EtablissementRattachement.HorsRegion = false;
            }

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacement(personnel, ci);

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement != null || indemniteDeplacement.CodeZoneDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemniteWithPersonnelWithoutGPS()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacement(personnel, ci);

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement == null && indemniteDeplacement.CodeZoneDeplacement == null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemniteWithPersonnelWithoutAdresse()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacement(personnel, ci);

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement == null && indemniteDeplacement.CodeZoneDeplacement == null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemnitePrevisionnelWithPointageIPD()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);
            CodeZoneDeplacementEnt zone = IndemniteDepMgr.GetZoneByDistance(personnel.SocieteId.Value, 40);

            RapportLigneEnt pointage = new RapportLigneEnt
            {
                Ci = ci,
                CiId = ci.CiId,
                Personnel = personnel,
                PersonnelId = personnel.PersonnelId,
                CodeZoneDeplacement = zone
            };

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(pointage, new DateTime(2016, 12, 13));

            Assert.IsTrue(indemniteDeplacement.CodeDeplacement == null && indemniteDeplacement.CodeZoneDeplacement != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CalculIndemnitePrevisionnelWithPointageIGDWithIndemniteInitialiazed()
        {
            CIEnt ci = CIMgr.GetCIById(1);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);
            CodeDeplacementEnt codep = new CodeDeplacementEnt { Code = "TST", Libelle = "Test", IGD = true, Actif = true };
            CodeDeplacementEnt codeCalculeSemaine = null;
            CodeDeplacementEnt codeCalculeVendredi = null;
            RapportLigneEnt pointage = new RapportLigneEnt
            {
                Ci = ci,
                CiId = ci.CiId,
                Personnel = personnel,
                PersonnelId = personnel.PersonnelId,
                CodeDeplacement = codep
            };

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(pointage, new DateTime(2016, 12, 13));
            Assert.IsTrue(indemniteDeplacement.CodeDeplacement != null && indemniteDeplacement.CodeZoneDeplacement == null);

            codeCalculeSemaine = indemniteDeplacement.CodeDeplacement;

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(pointage, new DateTime(2016, 12, 16));
            Assert.IsTrue(indemniteDeplacement.CodeDeplacement != null && indemniteDeplacement.CodeZoneDeplacement == null);

            codeCalculeVendredi = indemniteDeplacement.CodeDeplacement;
            Assert.IsTrue(codeCalculeSemaine.Code != codeCalculeVendredi.Code);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void CalculIndemnitePrevisionnelWithPointageIGDWithoutIndemniteInitialiazed()
        {
            CIEnt ci = CIMgr.GetCIById(2);
            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(PersonnelId);
            CodeDeplacementEnt codep = new CodeDeplacementEnt { Code = "TST", Libelle = "Test", IGD = true, Actif = true };
            RapportLigneEnt pointage = new RapportLigneEnt
            {
                Ci = ci,
                CiId = ci.CiId,
                Personnel = personnel,
                PersonnelId = personnel.PersonnelId,
                CodeDeplacement = codep
            };

            indemniteDeplacement = IndemniteDepMgr.CalculIndemniteDeplacementGenerationPrevisionnelle(pointage, new DateTime(2016, 12, 13));
            Assert.IsTrue(indemniteDeplacement.CodeDeplacement == null && indemniteDeplacement.CodeZoneDeplacement == null);
        }
    }
}
