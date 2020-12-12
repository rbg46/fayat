using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Module;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestModule : FredBaseTest
    {
        #region Modules

        /// <summary>
        ///   Teste que la liste des modules n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetListModulesNotNull()
        {
            var modules = ModuleMgr.GetModuleList();
            Assert.IsTrue(modules != null);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddModule()
        {
            ModuleEnt module = new ModuleEnt
            {
                Libelle = "Test Module",
                Code = GuidEx.CreateBase64Guid(20),
                Fonctionnalites = new FonctionnaliteEnt[] { }
            };

            int countBefore = ModuleMgr.GetModuleList().Count();
            ModuleMgr.AddModule(module);
            int countAfter = ModuleMgr.GetModuleList().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            ModuleMgr.DeleteModuleById(module.ModuleId);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteModule()
        {
            ModuleEnt module = new ModuleEnt
            {
                Libelle = "Test Module",
                Code = GuidEx.CreateBase64Guid(20),
                Fonctionnalites = new FonctionnaliteEnt[] { }
            };

            var moduleAdded = ModuleMgr.AddModule(module);
            int countBefore = ModuleMgr.GetModuleList().Count();
            ModuleMgr.DeleteModuleById(moduleAdded.ModuleId);
            int countAfter = ModuleMgr.GetModuleList().Count();

            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste la mise à jour d'un module en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void UpdateModule()
        {
            ModuleEnt module = new ModuleEnt
            {
                Libelle = "Test Module",
                Code = GuidEx.CreateBase64Guid(20),
                Fonctionnalites = new FonctionnaliteEnt[] { }
            };
            ModuleMgr.AddModule(module);

            string libModuleAvant = module.Libelle;
            module.Libelle = "Label changé";

            ModuleMgr.UpdateModule(module);
            Assert.IsTrue(ModuleMgr.GetModuleById(module.ModuleId).Libelle != libModuleAvant);
        }

        #endregion


    }
}
