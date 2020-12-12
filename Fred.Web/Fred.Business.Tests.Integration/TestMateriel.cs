using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestMateriel : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste de tous les matériels (actifs et inactifs) retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetMaterielListAll()
        {
            var listMateriel = MaterielMgr.GetMaterielListAll();
            Assert.IsTrue(listMateriel != null);
        }

        /// <summary>
        ///   Teste que la liste de tous des matériels actifs retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetMaterielList()
        {
            var listMateriel = MaterielMgr.GetMaterielList();
            Assert.IsTrue(listMateriel != null);
        }

        /// <summary>
        ///   Teste l'ajout d'un materiel
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void AddMateriel()
        {
            int nbMaterielBefore = MaterielMgr.GetMaterielListAll().Count();
            MaterielEnt materiel = new MaterielEnt
            {
                SocieteId = 1,
                RessourceId = 1,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test ajout",
                Actif = true
            };
            int id = MaterielMgr.AddMateriel(materiel);
            int nbMaterielAfter = MaterielMgr.GetMaterielListAll().Count();

            Assert.IsTrue(nbMaterielAfter == (nbMaterielBefore + 1));
        }

        /// <summary>
        ///   Teste la sauvegarde les modifications d'un materiel.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void UpdateMateriel()
        {

            MaterielEnt materiel = new MaterielEnt
            {
                SocieteId = 1,
                RessourceId = 1,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test Up",
                Actif = true
            };
            int id = MaterielMgr.AddMateriel(materiel);
            string libelleBefore = MaterielMgr.GetMaterielById(id).Libelle;
            materiel.Libelle = "Updated materiel";
            MaterielMgr.UpdateMateriel(materiel);
            string libelleAfter = MaterielMgr.GetMaterielById(id).Libelle;
            Assert.IsTrue(libelleBefore != libelleAfter);
        }

        /// <summary>
        ///   Teste la suppression d'un materiel
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void DeleteMaterielById()
        {
            MaterielEnt materiel = new MaterielEnt
            {
                SocieteId = 1,
                RessourceId = 1,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test Del",
                Actif = true
            };
            int id = MaterielMgr.AddMateriel(materiel);
            int nbMaterielBefore = MaterielMgr.GetMaterielListAll().Count();
            Assert.IsTrue(MaterielMgr.DeleteMaterielById(materiel));
            int nbMaterielAfter = MaterielMgr.GetMaterielListAll().Count();
            Assert.IsTrue(nbMaterielAfter < nbMaterielBefore);
        }
    }
}