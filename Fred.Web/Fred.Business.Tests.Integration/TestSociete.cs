using System.Linq;
using Fred.DataAccess.Common;
using Fred.Entities.Societe;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestSociete : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des sociétés n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetSocieteListReturnNotNullList()
        {
            var societes = SocieteMgr.GetSocieteList();
            Assert.IsTrue(societes != null);
        }

        /// <summary>
        ///   Teste la création d'une nouvelle société
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void AddNewSociete()
        {
            var repo = new DbRepository<SocieteEnt>(FredContext);

            SocieteEnt societe = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (création)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = true,
                Externe = true,
                GroupeId = 1
            };

            int countBefore = repo.Query().Get().AsNoTracking().Count();

            var societeAdded = SocieteMgr.AddSocieteAsync(societe);

            int countAfter = repo.Query().Get().AsNoTracking().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            SocieteMgr.DeleteSocieteById(societeAdded.Result.SocieteId);
        }

        /// <summary>
        ///   Teste la création d'une nouvelle société inactive => elle ne doit pas être visible
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void AddNewSocieteInactiveNotVisible()
        {
            var repo = new DbRepository<SocieteEnt>(FredContext);

            SocieteEnt societe = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (création)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = false,
                Externe = true,
                GroupeId = 1
            };

            int countBefore = repo.Query().Filter(s => s.Active).Get().AsNoTracking().Count();

            SocieteMgr.AddSocieteAsync(societe);

            int countAfter = repo.Query().Filter(s => s.Active).Get().AsNoTracking().Count();

            Assert.IsTrue(countBefore == countAfter);
        }

        /// <summary>
        ///   Teste la création d'une nouvelle société active  => elle doit être visible
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void AddNewSocieteActiveVisible()
        {
            var repo = new DbRepository<SocieteEnt>(FredContext);

            SocieteEnt societe = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (création)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = true,
                Externe = true,
                GroupeId = 1
            };

            int countBefore = repo.Query().Get().AsNoTracking().Count();

            SocieteMgr.AddSocieteAsync(societe);

            int countAfter = repo.Query().Get().AsNoTracking().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la mise à jour d'une société active en inactive  => elle ne doit plus être visible
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void UpdateExistingSocieteAsInactiveNotVisible()
        {

            var repo = new DbRepository<SocieteEnt>(FredContext);

            SocieteEnt societe = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (suppression logique)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = true,
                Externe = true,
                GroupeId = 1
            };

            SocieteMgr.AddSocieteAsync(societe);

            int countBefore = repo.Query().Filter(s => s.Active).Get().AsNoTracking().Count();

            societe.Active = false;
            SocieteMgr.UpdateSocieteAsync(societe);

            int countAfter = repo.Query().Filter(s => s.Active).Get().AsNoTracking().Count();

            Assert.IsTrue((countBefore - 1) == countAfter);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void UpdateExistingSociete()
        {


            SocieteEnt societeBefore = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (modification)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = true,
                Externe = true,
                GroupeId = 1
            };

            int societeId = SocieteMgr.AddSocieteAsync(societeBefore).Result.SocieteId;

            string libBefore = societeBefore.Libelle;
            societeBefore.Libelle = "Societe de test après modification";

            SocieteMgr.UpdateSocieteAsync(societeBefore);

            SocieteEnt societeAfter = SocieteMgr.GetSocieteById(societeId);

            Assert.AreNotEqual(libBefore, societeAfter.Libelle);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void DeleteExistingSociete()
        {
            var repo = new DbRepository<SocieteEnt>(FredContext);

            SocieteEnt societe = new SocieteEnt
            {
                Code = "TS",
                CodeSocietePaye = "TST",
                CodeSocieteComptable = "TST01",
                Libelle = "Société de test (suppression)",
                Adresse = "Test",
                Ville = "Test",
                CodePostal = "00000",
                SIREN = "1234567891011",
                Active = true,
                Externe = true,
                GroupeId = 1
            };

            int countBefore = repo.Query().Get().AsNoTracking().Count();
            SocieteMgr.DeleteSocieteById(societe.SocieteId);
            int countAfter = repo.Query().Get().AsNoTracking().Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste recherche des sociétés
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetSocieteListReturnAtLeastOneRecord()
        {
            var societes = SocieteMgr.GetSocieteList().ToList();
            Assert.IsTrue(societes.Count > 0);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingSocieteReturnNull()
        {
            SocieteEnt societe = SocieteMgr.GetSocieteById(-1);
            Assert.IsNull(societe);
        }
    }
}
