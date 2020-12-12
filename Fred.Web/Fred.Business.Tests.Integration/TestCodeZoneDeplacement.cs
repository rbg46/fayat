using System;
using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestCodeZoneDeplacement : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste de tous les codes zone deplacement retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeZoneDeplacementListAll()
        {
            var listCodeZoneDeplacement = CodeZoneDeplacementMgr.GetCodeZoneDeplacementListAll();
            Assert.IsTrue(listCodeZoneDeplacement != null);
        }

        /// <summary>
        ///   Teste que la liste de tous des codes zone deplacement actifs retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeZoneDeplacementList()
        {
            var listCodeZoneDeplacement = CodeZoneDeplacementMgr.GetCodeZoneDeplacementList();
            Assert.IsTrue(listCodeZoneDeplacement != null);
        }

        /// <summary>
        ///   Teste l'ajout d'un code zone deplacement
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddCodeZoneDeplacement()
        {
            int nbCodeZoneDeplacementBefore = CodeZoneDeplacementMgr.GetCodeZoneDeplacementListAll().Count();
            CodeZoneDeplacementEnt codeZoneDeplacement = new CodeZoneDeplacementEnt
            {
                AuteurCreation = SuperAdminUserId,
                DateCreation = DateTime.Now,
                SocieteId = 1,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test ajout"
            };
            int id = CodeZoneDeplacementMgr.AddCodeZoneDeplacement(codeZoneDeplacement);
            int nbCodeZoneDeplacementAfter = CodeZoneDeplacementMgr.GetCodeZoneDeplacementListAll().Count();

            Assert.IsTrue(nbCodeZoneDeplacementAfter == (nbCodeZoneDeplacementBefore + 1));

            //Test cleanup
            CodeZoneDeplacementMgr.DeleteCodeZoneDeplacementById(codeZoneDeplacement);
        }

        /// <summary>
        ///   Teste la sauvegarde les modifications d'un code zone deplacement.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateCodeZoneDeplacement()
        {
            CodeZoneDeplacementEnt codeZoneDeplacement = new CodeZoneDeplacementEnt
            {
                AuteurCreation = SuperAdminUserId,
                SocieteId = 1,
                DateCreation = DateTime.Now,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test Up"
            };
            int id = CodeZoneDeplacementMgr.AddCodeZoneDeplacement(codeZoneDeplacement);
            string libelleBefore = CodeZoneDeplacementMgr.GetCodeZoneDeplacementById(id).Libelle;
            codeZoneDeplacement.Libelle = "Updated CodeZoneDeplacement";
            codeZoneDeplacement.DateModification = DateTime.Now;
            codeZoneDeplacement.AuteurModification = SuperAdminUserId;
            CodeZoneDeplacementMgr.UpdateCodeZoneDeplacement(codeZoneDeplacement);
            string libelleAfter = CodeZoneDeplacementMgr.GetCodeZoneDeplacementById(id).Libelle;
            Assert.IsTrue(libelleBefore != libelleAfter);

            //Test cleanup
            CodeZoneDeplacementMgr.DeleteCodeZoneDeplacementById(codeZoneDeplacement);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void DeleteCodeZoneDeplacementById()
        {
            CodeZoneDeplacementEnt codeZoneDeplacement = new CodeZoneDeplacementEnt
            {
                AuteurCreation = SuperAdminUserId,
                SocieteId = 1,
                DateCreation = DateTime.Now,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test Del"
            };

            int id = CodeZoneDeplacementMgr.AddCodeZoneDeplacement(codeZoneDeplacement);
            int nbCodeZoneDeplacementBefore = CodeZoneDeplacementMgr.GetCodeZoneDeplacementList().Count();
            Assert.IsTrue(CodeZoneDeplacementMgr.DeleteCodeZoneDeplacementById(codeZoneDeplacement));
            int nbCodeZoneDeplacementAfter = CodeZoneDeplacementMgr.GetCodeZoneDeplacementList().Count();
            Assert.IsTrue(nbCodeZoneDeplacementAfter < nbCodeZoneDeplacementBefore);
        }

        /// <summary>
        ///   Teste le code zone unique pour une societe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void CodeZoneDeplacementExistForSoc()
        {
            CodeZoneDeplacementEnt codeZoneDeplacement = new CodeZoneDeplacementEnt
            {
                AuteurCreation = SuperAdminUserId,
                SocieteId = 1,
                DateCreation = DateTime.Now,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test unique"
            };

            CodeZoneDeplacementMgr.AddCodeZoneDeplacement(codeZoneDeplacement);
            bool resu = CodeZoneDeplacementMgr.IsCodeZoneDeplacementExistsBySoc(codeZoneDeplacement.Code, codeZoneDeplacement.SocieteId);
            Assert.IsTrue(resu);

            //Test cleanup
            CodeZoneDeplacementMgr.DeleteCodeZoneDeplacementById(codeZoneDeplacement);
        }
    }
}
