using System.Linq;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestCobeAbsence : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste de tous les codes d'absence retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeAbsListAll()
        {
            var listCodeAbs = CodeAbsenceMgr.GetCodeAbsListAll();
            Assert.IsTrue(listCodeAbs != null);
        }

        /// <summary>
        ///   Teste que la liste de tous des codes d'absence actifs retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeAbsList()
        {
            var listCodeAbs = CodeAbsenceMgr.GetCodeAbsList();
            Assert.IsTrue(listCodeAbs != null);
        }

        /// <summary>
        ///   Teste l'ajout d'un code d'absence
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddCodeAbsence()
        {
            int nbCodeAbsBefore = CodeAbsenceMgr.GetCodeAbsListAll().Count();
            CodeAbsenceEnt codeAbs = new CodeAbsenceEnt
            {
                SocieteId = 1,
                Code = "AD",
                Libelle = "Test ajout",
                Intemperie = false,
                TauxDecote = 20,
                NBHeuresDefautETAM = 8,
                NBHeuresMinETAM = 7,
                NBHeuresMaxETAM = 8,
                NBHeuresDefautCO = 8,
                NBHeuresMinCO = 7,
                NBHeuresMaxCO = 8,
                Actif = true
            };
            int id = CodeAbsenceMgr.AddCodeAbsence(codeAbs);
            int nbCodeAbsAfter = CodeAbsenceMgr.GetCodeAbsListAll().Count();

            Assert.IsTrue(nbCodeAbsAfter == (nbCodeAbsBefore + 1));
        }

        /// <summary>
        ///   Teste la sauvegarde les modifications d'un code d'absence.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateCodeAbsence()
        {

            CodeAbsenceEnt codeAbs = new CodeAbsenceEnt
            {
                SocieteId = 1,
                Code = "UP",
                Libelle = "Test Up",
                Intemperie = false,
                TauxDecote = 20,
                NBHeuresDefautETAM = 8,
                NBHeuresMinETAM = 7,
                NBHeuresMaxETAM = 8,
                NBHeuresDefautCO = 8,
                NBHeuresMinCO = 7,
                NBHeuresMaxCO = 8,
                Actif = true
            };
            int id = CodeAbsenceMgr.AddCodeAbsence(codeAbs);
            string libelleBefore = CodeAbsenceMgr.GetCodeAbsenceById(id).Libelle;
            codeAbs.Libelle = "Updated CodeAbs";
            CodeAbsenceMgr.UpdateCodeAbsence(codeAbs);
            string libelleAfter = CodeAbsenceMgr.GetCodeAbsenceById(id).Libelle;
            Assert.IsTrue(libelleBefore != libelleAfter);
        }

        /// <summary>
        ///   Teste la suppression d'une prime du référentiel.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void DeleteCodeAbsenceById()
        {

            CodeAbsenceEnt codeAbs = new CodeAbsenceEnt
            {
                SocieteId = 1,
                Code = "DE",
                Libelle = "Test Del",
                Intemperie = false,
                TauxDecote = 20,
                NBHeuresDefautETAM = 8,
                NBHeuresMinETAM = 7,
                NBHeuresMaxETAM = 8,
                NBHeuresDefautCO = 8,
                NBHeuresMinCO = 7,
                NBHeuresMaxCO = 8,
                Actif = true
            };
            int id = CodeAbsenceMgr.AddCodeAbsence(codeAbs);
            int nbCodeAbsBefore = CodeAbsenceMgr.GetCodeAbsListAll().Count();
            CodeAbsenceMgr.DeleteCodeAbsenceById(codeAbs);
            int nbCodeAbsAfter = CodeAbsenceMgr.GetCodeAbsListAll().Count();
            Assert.IsTrue(nbCodeAbsAfter < nbCodeAbsBefore);
        }
    }
}