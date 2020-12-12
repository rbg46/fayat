using System;
using Fred.Entities.Journal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestJournal : FredBaseTest
    {
        private static JournalEnt journal;

        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
            journal = new JournalEnt
            {
                SocieteId = 1,
                Code = "journalTest",
                Libelle = "mon journal de test",
                DateCloture = DateTime.Now.AddYears(1),
                DateCreation = DateTime.Now,
                AuteurCreationId = 19,
                ImportFacture = true
            };
        }

        #region Journal

        /// <summary>
        ///   Teste que la liste des journaux retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetJournalList()
        {
            var resu = JournalRepository.GetAllJournal();

            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste l'ajout d'un journal
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void AddJournal()
        {
            int resu = JournalRepository.AddJournal(journal).JournalId;
            Assert.IsTrue(resu > 0);
        }

        /// <summary>
        ///   Teste la récupération d'un journal par l'id de la societe
        /// </summary>
        [TestMethod]
        [TestCategory("DBDependNotMaintained")]

        public void GetJournalByIdSociete()
        {
            var resu = JournalRepository.GetJournaux(1);

            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste la récupération d'un journal par son code societe
        /// </summary>
        [TestMethod]
        [TestCategory("DBDependNotMaintained")]

        public void GetJournalByCodeSociete()
        {
            var resu = JournalRepository.GetLogImportBySocieteCode("RB");

            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste la récupération d'un journal par son code
        /// </summary>
        [TestMethod]
        [TestCategory("DBDependNotMaintained")]
        [Ignore]
        public void GetJournalByCode()
        {
            JournalEnt resu = JournalRepository.GetJournalByCode("journalTest");

            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste la récupération d'un journal par son id
        /// </summary>
        [TestMethod]
        [TestCategory("DBDependNotMaintained")]

        public void GetJournalById()
        {
            JournalEnt resu = JournalRepository.GetLogImportById(2);

            Assert.IsTrue(resu != null);
        }

        #endregion
    }
}
