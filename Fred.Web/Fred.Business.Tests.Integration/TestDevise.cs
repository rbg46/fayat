using System;
using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestDevise : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des devises n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetDeviseListReturnNotNullList()
        {
            var devises = DeviseMgr.GetList();
            Assert.IsTrue(devises != null);
        }

        /// <summary>
        ///   Teste que la liste des devises n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetAllSinceDate()
        {
            int oldCount = DeviseMgr.GetAll().Count();
            DateTime date = DateTime.Now;
            DeviseEnt devise = new DeviseEnt
            {
                IsoCode = GuidEx.CreateBase64Guid().Left(3),
                CodeHtml = "&",
                Libelle = "Brexit",
                IsoNombre = "69",
                Symbole = "?",
                CodePaysIso = "??",
                DateCreation = date,
                IsDeleted = false
            };
            DeviseMgr.Add(devise);
            int newCount = DeviseMgr.GetAll(date).Count();
            Assert.AreEqual(1, newCount - oldCount);

            //Test cleanup
            DeviseMgr.DeleteById(devise);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewDevise()
        {
            DeviseEnt devise = new DeviseEnt
            {
                IsoCode = "TI1",
                CodeHtml = "&",
                Libelle = "Brexit",
                IsoNombre = "69",
                Symbole = "?",
                CodePaysIso = "??",
                IsDeleted = false
            };

            int countRepoBefore = DeviseMgr.GetList().Count();
            DeviseMgr.Add(devise);
            int countRepoAfter = DeviseMgr.GetList().Count();

            Assert.IsTrue((countRepoBefore + 1) == countRepoAfter);

            //Test cleanup
            DeviseMgr.DeleteById(devise);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateExistingDevise()
        {
            DeviseEnt devise = new DeviseEnt
            {
                IsoCode = "TI2",
                CodeHtml = "&",
                Libelle = "Brexit",
                IsoNombre = "69",
                Symbole = "?",
                CodePaysIso = "??",
                IsDeleted = false
            };

            int deviseId = DeviseMgr.Add(devise);
            string libBefore = devise.Libelle;
            devise.Libelle = "Libellé devise après modification";
            DeviseMgr.Update(devise);
            DeviseEnt deviseAfter = DeviseMgr.GetById(deviseId);

            Assert.AreNotEqual(libBefore, deviseAfter.Libelle);

            //Test cleanup
            DeviseMgr.DeleteById(devise);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteExistingDevise()
        {
            DeviseEnt devise = new DeviseEnt
            {
                IsoCode = "TI3",
                CodeHtml = "&",
                Libelle = "Brexit",
                IsoNombre = "69",
                Symbole = "?",
                CodePaysIso = "??"
            };

            int deviseId = DeviseMgr.Add(devise);
            int countRepoBefore = DeviseMgr.GetList().Count();
            Assert.IsTrue(DeviseMgr.DeleteById(devise));
            int countRepoAfter = DeviseMgr.GetList().Count();

            Assert.IsTrue((countRepoBefore - 1) == countRepoAfter);
        }
    }
}
