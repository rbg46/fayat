using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.ObjectifFlash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    [Ignore]
    public class TestObjectifFlash : FredBaseTest
    {
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewObjectifFlash()
        {
            ObjectifFlashEnt objectifFlash = new ObjectifFlashEnt
            {
                ObjectifFlashId = 0,
                Libelle = "Test",
                DateDebut = DateTime.Now.Date,
                DateFin = DateTime.Now.Date,
                CiId = 189,
                Ci = null,
            };
            var objectifFlashAdded = ObjectifFlashMgr.AddObjectifFlash(objectifFlash);
            Assert.IsTrue(objectifFlashAdded.ObjectifFlashId > 0);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewObjectifFlashWithTachesAndRessources()
        {
            var objectifFlashAdded = ObjectifFlashMgr.AddObjectifFlash(this.CreateNewObjectifFlashWithTachesAndRessources());
            Assert.IsTrue(objectifFlashAdded.ObjectifFlashId > 0);
            Assert.IsTrue(objectifFlashAdded.Taches.Any(x => x.ObjectifFlashTacheId != 0));
            Assert.IsTrue(objectifFlashAdded.Taches.First().Ressources.Any(x => x.ObjectifFlashTacheRessourceId != 0));
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewObjectifFlashAndUpdate()
        {
            var objectifFlashAdded = ObjectifFlashMgr.AddObjectifFlash(this.CreateNewObjectifFlashWithTachesAndRessources());
            Assert.IsTrue(objectifFlashAdded.ObjectifFlashId > 0);
            Assert.IsTrue(objectifFlashAdded.Taches.Any(x => x.ObjectifFlashTacheId != 0));
            Assert.IsTrue(objectifFlashAdded.Taches.First().Ressources.Any(x => x.ObjectifFlashTacheRessourceId != 0));

            objectifFlashAdded.Libelle = "libelle2";
            objectifFlashAdded.Taches.First().QuantiteObjectif = 9999;
            objectifFlashAdded.Taches.First().Ressources.First().QuantiteObjectif = 8888;

            var objectifFlashUpdated = ObjectifFlashMgr.UpdateObjectifFlash(objectifFlashAdded);
            Assert.IsTrue(objectifFlashUpdated.Taches.First().QuantiteObjectif == 9999);
            Assert.IsTrue(objectifFlashUpdated.Taches.First().Ressources.First().QuantiteObjectif == 8888);
        }


        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewObjectifFlashWithTachesRessourcesAndJournalisations()
        {
            var objectifFlashAdded = ObjectifFlashMgr.AddObjectifFlash(this.CreateNewObjectifFlashWithTachesAndRessources());
            objectifFlashAdded.DateDebut = DateTime.Now.Date;
            objectifFlashAdded.DateFin = objectifFlashAdded.DateDebut.AddDays(20);
            objectifFlashAdded.Taches.First().QuantiteObjectif = 50000;
            objectifFlashAdded.Taches.First().Ressources.First().QuantiteObjectif = 10000;
            var objectifFlashJournalise = ObjectifFlashMgr.GetNewJournalisation(objectifFlashAdded);
            ObjectifFlashMgr.UpdateObjectifFlash(objectifFlashJournalise);

            Assert.IsTrue(objectifFlashAdded.ObjectifFlashId > 0);
            Assert.IsTrue(objectifFlashAdded.Taches.Any(x => x.ObjectifFlashTacheId != 0));
            Assert.IsTrue(objectifFlashAdded.Taches.First().Ressources.Any(x => x.ObjectifFlashTacheRessourceId != 0));
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateObjectifFlashTacheRapportRealise()
        {
            var objectifFlashAdded = ObjectifFlashMgr.AddObjectifFlash(this.CreateNewObjectifFlashWithTachesAndRessources());
            var firstTache = objectifFlashAdded.Taches.First();
            var rapportId = 531;
            var tacheRealisations = new List<ObjectifFlashTacheRapportRealiseEnt> {
                new ObjectifFlashTacheRapportRealiseEnt
                {
                    ObjectifFlashTacheRapportRealiseId = 0,
                    ObjectifFlashTacheId = firstTache.ObjectifFlashTacheId,
                    DateRealise = DateTime.Now.Date,
                    QuantiteRealise = 100,
                    RapportId = rapportId
                },
            };
            ObjectifFlashTacheMgr.UpdateObjectifFlashTacheRapportRealise(rapportId, tacheRealisations);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetObjectifFlashTacheRapportRealise()
        {
            //this.CreateNewObjectifFlashWithTachesAndRessources()
            var objectifFlash1 = this.CreateNewObjectifFlashWithTachesAndRessources();
            objectifFlash1.DateDebut = DateTime.Now.Date;
            objectifFlash1.DateFin = DateTime.Now.Date.AddDays(1);
            objectifFlash1.Libelle = "objectifFlash1";

            var objectifFlash2 = this.CreateNewObjectifFlashWithTachesAndRessources();
            objectifFlash2.DateDebut = DateTime.Now.Date;
            objectifFlash2.DateFin = DateTime.Now.Date.AddDays(1);
            objectifFlash2.Libelle = "objectifFlash2";

            ObjectifFlashMgr.AddObjectifFlash(objectifFlash1);
            ObjectifFlashMgr.AddObjectifFlash(objectifFlash2);


            var objectifFlashs = ObjectifFlashTacheMgr.GetObjectifFlashListByDateCiIdAndTacheIds(DateTime.Now.Date, objectifFlash1.CiId.Value, 1, objectifFlash1.Taches.Select(x => x.TacheId).ToList());
            Assert.IsTrue(objectifFlashs.Count(x => x.Libelle == "objectifFlash1") > 0);
            Assert.IsTrue(objectifFlashs.Count(x => x.Libelle == "objectifFlash1") > 0);
        }

        private ObjectifFlashEnt CreateNewObjectifFlashWithTachesAndRessources()
        {
            return new ObjectifFlashEnt
            {
                ObjectifFlashId = 0,
                DateDebut = DateTime.Now.Date,
                DateFin = DateTime.Now.Date.AddDays(1),
                Libelle = "test",
                CiId = 189,
                Taches = new List<ObjectifFlashTacheEnt>()
                {
                    new ObjectifFlashTacheEnt{
                        TacheId = 17621,
                        Ressources =new List<ObjectifFlashTacheRessourceEnt>()
                        {
                            new ObjectifFlashTacheRessourceEnt{
                                RessourceId = 1
                            }
                        },
                        TacheJournalisations = null
                    }
                }
            };


        }
    }
}
