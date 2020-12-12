using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestNature : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des natures actives ou pas retournées n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNatureListAll()
        {

            var listNature = NatureMgr.GetNatureListAll();
            Assert.IsTrue(listNature != null);
        }

        /// <summary>
        ///   Teste que la liste des natures actives retournées n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNatureList()
        {
            var listnature = NatureMgr.GetNatureList();
            Assert.IsTrue(listnature != null);
        }

        /// <summary>
        ///   Teste l'ajout d'une nature
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNature()
        {
            int nbNatureBefore = NatureMgr.GetNatureListAll().Count();
            NatureEnt nature = new NatureEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test AddNature",
                SocieteId = 1,
                AuteurCreationId = 1,
                DateCreation = DateTime.Now
            };
            NatureMgr.AddNature(nature);
            int nbNatureAfter = NatureMgr.GetNatureListAll().Count();
            Assert.IsTrue(nbNatureAfter == (nbNatureBefore + 1));

            //Test cleanup
            NatureMgr.DeleteNatureById(nature);
        }

        /// <summary>
        ///   Teste la sauvegarde les modifications d'un code d'absence.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateNature()
        {
            NatureEnt nature = new NatureEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test UpdateNature libelleBefore",
                SocieteId = 1,
                AuteurCreationId = 1,
                DateCreation = DateTime.Now
            };
            NatureMgr.AddNature(nature);
            string libelleBefore = NatureMgr.GetNatureById(nature.NatureId).Libelle;
            nature.Libelle = "Test UpdateNature libelleAfter";

            List<Expression<Func<NatureEnt, object>>> fieldToUpdate = new List<Expression<Func<NatureEnt, object>>>
            {
                x => x.Code,
                x => x.Libelle,
                x => x.IsActif,
                x => x.RessourceId
            };
            NatureMgr.UpdateNature(nature, fieldToUpdate);
            string libelleAfter = NatureMgr.GetNatureById(nature.NatureId).Libelle;
            Assert.IsTrue(libelleBefore != libelleAfter);

            //Test cleanup
            NatureMgr.DeleteNatureById(nature);
        }

        /// <summary>
        ///   Teste la suppression logique d'une nature.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteNatureById()
        {
            NatureEnt nature = new NatureEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "Test DeleteNatureById",
                SocieteId = 1,
                AuteurCreationId = 1,
                DateCreation = DateTime.Now
            };
            int id = NatureMgr.AddNature(nature);
            int nbNatureBefore = NatureMgr.GetNatureList().Count();
            Assert.IsTrue(NatureMgr.DeleteNatureById(nature));
            int nbNatureAfter = NatureMgr.GetNatureList().Count();
            nature = NatureMgr.GetNatureById(id);
            Assert.IsTrue(nbNatureAfter < nbNatureBefore /*&& nature.DateSuppression != null*/);
        }
    }
}
