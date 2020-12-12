using System.Linq;
using Fred.Entities.ReferentielFixe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestRessource : FredBaseTest
    {
        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingChapitreReturnNull()
        {
            ChapitreEnt chapitre = RefFixeMgr.GetChapitreById(-1);
            Assert.IsNull(chapitre);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingSousChapitreReturnNull()
        {

            SousChapitreEnt sousChapitre = RefFixeMgr.GetSousChapitreById(-1);
            Assert.IsNull(sousChapitre);
        }


        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingRessourceReturnNull()
        {

            RessourceEnt ressource = RefFixeMgr.GetRessourceById(-1);
            Assert.IsNull(ressource);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void AddNewChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test Add",
                GroupeId = groupeId
            };

            int countBefore = RefFixeMgr.GetChapitreListByGroupeId(groupeId).Count();

            RefFixeMgr.AddChapitre(chapitre);

            int countAfter = RefFixeMgr.GetChapitreListByGroupeId(groupeId).Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }


        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddNewSousChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitre = new SousChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "SousChapitre test Add",
                ChapitreId = chapitreId
            };

            int countBefore = RefFixeMgr.GetSousChapitreListByChapitreId(chapitreId).Count();

            RefFixeMgr.AddSousChapitre(sousChapitre);

            int countAfter = RefFixeMgr.GetSousChapitreListByChapitreId(chapitreId).Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddNewRessource()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitre = new SousChapitreEnt
            {
                Code = "SCHATEST1",
                Libelle = "SousChapitre test",
                ChapitreId = chapitreId
            };

            int sousChapitreId = RefFixeMgr.AddSousChapitre(sousChapitre).SousChapitreId;

            RessourceEnt ressource = new RessourceEnt
            {
                Code = "RESTEST1",
                Libelle = "Ressource test Add",
                SousChapitreId = sousChapitreId
            };

            int countBefore = RefFixeMgr.GetRessourceListBySousChapitreId(sousChapitreId).Count();

            var ressourceAdded = RefFixeMgr.AddRessource(ressource);

            int countAfter = RefFixeMgr.GetRessourceListBySousChapitreId(sousChapitreId).Count();

            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            RefFixeMgr.DeleteRessourceById(ressourceAdded.RessourceId);
            RefFixeMgr.DeleteSousChapitreById(sousChapitreId);
            RefFixeMgr.DeleteChapitreById(chapitreId);

        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateExistingChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitreBefore = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitreBefore).ChapitreId;

            string libBefore = chapitreBefore.Libelle;
            chapitreBefore.Libelle = "Test Chapitre";

            RefFixeMgr.UpdateChapitre(chapitreBefore);

            ChapitreEnt chapitreAfter = RefFixeMgr.GetChapitreById(chapitreId);

            Assert.AreNotEqual(libBefore, chapitreAfter.Libelle);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateExistingSousChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitreBefore = new SousChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "SousChapitre test",
                ChapitreId = chapitreId
            };

            int sousChapitreId = RefFixeMgr.AddSousChapitre(sousChapitreBefore).SousChapitreId;

            string libBefore = sousChapitreBefore.Libelle;
            sousChapitreBefore.Libelle = "Test SousChapitre";

            RefFixeMgr.UpdateSousChapitre(sousChapitreBefore);

            SousChapitreEnt sousChapitreAfter = RefFixeMgr.GetSousChapitreById(sousChapitreId);

            Assert.AreNotEqual(libBefore, sousChapitreAfter.Libelle);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateExistingRessource()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitre = new SousChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "SousChapitre test",
                ChapitreId = chapitreId
            };

            int sousChapitreId = RefFixeMgr.AddSousChapitre(sousChapitre).SousChapitreId;

            RessourceEnt ressourceBefore = new RessourceEnt
            {
                Code = "RESTEST2",
                Libelle = "Ressource test",
                SousChapitreId = sousChapitreId
            };

            int ressourceId = RefFixeMgr.AddRessource(ressourceBefore).RessourceId;

            string libBefore = ressourceBefore.Libelle;
            ressourceBefore.Libelle = "Test Ressource";

            RefFixeMgr.UpdateRessource(ressourceBefore);

            RessourceEnt ressourceAfter = RefFixeMgr.GetRessourceById(ressourceId);

            Assert.AreNotEqual(libBefore, ressourceAfter.Libelle);

            //Test cleanup
            RefFixeMgr.DeleteRessourceById(ressourceId);
            RefFixeMgr.DeleteSousChapitreById(sousChapitreId);
            RefFixeMgr.DeleteChapitreById(chapitreId);
        }

        /// <summary>
        ///   Teste recherche des ressources
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetRessourceListReturnAtLeastOneRecord()
        {

            var ressources = RefFixeMgr.GetRessourceList().ToList();
            Assert.IsTrue(ressources.Count > 0);
        }

        /// <summary>
        ///   Teste recherche des chapitres
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetChapitreListReturnAtLeastOneRecord()
        {

            var chapitres = RefFixeMgr.GetChapitreList().ToList();
            Assert.IsTrue(chapitres.Count > 0);
        }

        /// <summary>
        ///   Teste recherche des sousChapitres
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetSousChapitreListReturnAtLeastOneRecord()
        {

            var sousChapitres = RefFixeMgr.GetSousChapitreList().ToList();
            Assert.IsTrue(sousChapitres.Count > 0);
        }

        /// <summary>
        ///   Teste que la liste des chapitres n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetChapitreListReturnNotNullList()
        {

            int groupeId = 1;
            var chapitres = RefFixeMgr.GetChapitreListByGroupeId(groupeId).ToList();
            Assert.IsTrue(chapitres != null);
        }


        /// <summary>
        ///   Teste que la liste des sousChapitres n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetSousChapitreListReturnNotNullList()
        {

            var sousChapitres = RefFixeMgr.GetSousChapitreList().ToList();
            Assert.IsTrue(sousChapitres != null);
        }

        /// <summary>
        ///   Teste que la liste des ressources n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetRessourceListReturnNotNullList()
        {

            var ressources = RefFixeMgr.GetRessourceList().ToList();
            Assert.IsTrue(ressources != null);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void DeleteExistingChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;
            int countBefore = RefFixeMgr.GetChapitreListByGroupeId(groupeId).Count();
            RefFixeMgr.DeleteChapitreById(chapitreId);
            int countAfter = RefFixeMgr.GetChapitreListByGroupeId(groupeId).Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void DeleteExistingSousChapitre()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitre = new SousChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "SousChapitre test",
                ChapitreId = chapitreId
            };

            int sousChapitreId = RefFixeMgr.AddSousChapitre(sousChapitre).SousChapitreId;
            int countBefore = RefFixeMgr.GetSousChapitreListByChapitreId(chapitreId).Count();
            RefFixeMgr.DeleteSousChapitreById(sousChapitreId);
            int countAfter = RefFixeMgr.GetSousChapitreListByChapitreId(chapitreId).Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteExistingRessource()
        {

            int groupeId = 1;

            ChapitreEnt chapitre = new ChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "Chapitre test",
                GroupeId = groupeId
            };

            int chapitreId = RefFixeMgr.AddChapitre(chapitre).ChapitreId;

            SousChapitreEnt sousChapitre = new SousChapitreEnt
            {
                Code = GenerateString(8),
                Libelle = "SousChapitre test",
                ChapitreId = chapitreId
            };

            int sousChapitreId = RefFixeMgr.AddSousChapitre(sousChapitre).SousChapitreId;

            RessourceEnt ressource = new RessourceEnt
            {
                Code = GenerateString(8),
                Libelle = "Ressource test",
                SousChapitreId = sousChapitreId
            };

            int ressourceId = RefFixeMgr.AddRessource(ressource).RessourceId;
            int countBefore = RefFixeMgr.GetRessourceListBySousChapitreId(sousChapitreId).Count();
            RefFixeMgr.DeleteRessourceById(ressourceId);
            int countAfter = RefFixeMgr.GetRessourceListBySousChapitreId(sousChapitreId).Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }
    }
}
