using System.Linq;
using Fred.DataAccess.Common;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestTache : FredBaseTest
    {
        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddNewTache()
        {
            TacheEnt tache = new TacheEnt
            {
                Code = GenerateString(6),
                Libelle = "Tache YDY",
                Niveau = 1,
                CiId = 1
            };

            var epo = new DbRepository<TacheEnt>(FredContext);
            int countBefore = epo.Query().Get().Count();

            TacheMgr.AddTache(tache);

            int countAfter = epo.Query().Get().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateExistingTache()
        {
            TacheEnt tacheBefore = new TacheEnt
            {
                Code = GenerateString(6),
                Libelle = "Tache YDY",
                Niveau = 1,
                CiId = 1
            };

            int tacheId = TacheMgr.AddTache(tacheBefore);

            string libBefore = tacheBefore.Libelle;
            tacheBefore.Libelle = "Test Tache";

            TacheMgr.UpdateTache(tacheBefore);

            TacheEnt tacheAfter = TacheMgr.GetTacheById(tacheId);

            Assert.AreNotEqual(libBefore, tacheAfter.Libelle);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void DeleteExistingTache()
        {
            TacheEnt tache = new TacheEnt
            {
                Code = GenerateString(6),
                Libelle = "TestDeleteTache",
                Niveau = 1,
                CiId = 1
            };

            int tacheId = TacheMgr.AddTache(tache);
            TacheMgr.DeleteTacheById(tacheId);

            TacheEnt tacheDeleted = TacheMgr.FindById(tache.TacheId);

            Assert.IsTrue(tacheDeleted.DateSuppression != null);
        }

        /// <summary>
        ///   Teste recherche des tâches
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetTacheListReturnAtLeastOneRecord()
        {
            var taches = TacheMgr.GetTacheList().ToList();
            Assert.IsTrue(taches.Count > 0);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingTacheReturnNull()
        {
            TacheEnt tache = TacheMgr.GetTacheById(-1);
            Assert.IsNull(tache);
        }

        /// <summary>
        ///   Teste que la liste des tâches n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetTacheListReturnNotNullList()
        {
            var taches = TacheMgr.GetTacheList();
            Assert.IsTrue(taches != null);
        }

        /// <summary>
        ///   Teste que la tâche par défaut existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetTacheParDefaut()
        {

            int ciId = 1;
            TacheEnt tacheParDefaut = TacheMgr.GetTacheParDefaut(ciId);

            Assert.IsTrue(tacheParDefaut != null && tacheParDefaut.TacheParDefaut);
        }

        /// <summary>
        ///   Teste la génération du nouveau code d'une tâche
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GetNextTaskCode()
        {
            TacheEnt task = TacheMgr.GetTacheById(4);
            string nextCode = TacheMgr.GetNextTaskCode(task);
            Assert.IsTrue(nextCode == "PREPA05");
        }

        #region Taches

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetTacheById()
        {

            TacheEnt tache = TacheMgr.GetTacheById(1);
            int id = tache.TacheId;
            Assert.AreEqual(id, 1);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddTache()
        {


            CIEnt ci = new DbRepository<CIEnt>(FredContext).Query().Get().First();
            TacheEnt tache = new TacheEnt
            {
                Code = GenerateString(10),
                Libelle = "Tache de Test",
                Active = true,
                CiId = ci.CiId,
                Niveau = 1
            };

            var repo = TacheRepository;
            int countBefore = repo.Query().Filter(t => t.CiId == ci.CiId && !t.DateSuppression.HasValue).Get().Count();

            int id = TacheMgr.AddTache(tache);

            int countAfter = repo.Query().Filter(t => t.CiId == ci.CiId && !t.DateSuppression.HasValue).Get().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateTache()
        {
            TacheEnt tache = new TacheEnt
            {
                Code = GenerateString(6),
                Libelle = "Tache de Test",
                Active = true,
                CiId = 1,
                Niveau = 1
            };
            string libelleBefore = "Test fonctionnel";

            int id = TacheMgr.AddTache(tache);
            TacheEnt tacheTest = TacheMgr.GetTacheById(id);
            tacheTest.Libelle = libelleBefore;
            TacheMgr.UpdateTache(tacheTest);
            string libelleAfter = TacheMgr.GetTacheById(id).Libelle;

            Assert.IsTrue(libelleBefore == libelleAfter);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void DeleteTache()
        {
            CIEnt ci = new DbRepository<CIEnt>(FredContext).Query().Get().First();
            TacheEnt tache = new TacheEnt
            {
                Code = GenerateString(10),
                Libelle = "Tache de Test",
                Active = true,
                CiId = ci.CiId,
                Niveau = 1
            };

            int tacheId = TacheMgr.AddTache(tache);
            var repo = TacheRepository;
            int countBefore = repo.Query().Filter(t => t.CiId == ci.CiId && !t.DateSuppression.HasValue).Get().Count();
            TacheMgr.DeleteTacheById(tacheId);
            int countAfter = repo.Query().Filter(t => t.CiId == ci.CiId && !t.DateSuppression.HasValue).Get().Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        #endregion
    }
}
