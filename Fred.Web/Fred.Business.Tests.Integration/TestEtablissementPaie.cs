using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestEtablissementPaie : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des établissements de paie retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetEtablissementPaieList()
        {

            var etablissements = EtabPaieMgr.GetEtablissementPaieList();
            Assert.IsTrue(etablissements != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void AgencesDeRattachement()
        {

            EtablissementPaieEnt etablissement = new EtablissementPaieEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                SocieteId = 1,
                Libelle = "EtabPaieTU",
                IsAgenceRattachement = false,
                HorsRegion = true,
                Actif = true,
                Adresse = "35 avenue des Oranges Bleues",
                Latitude = 0,
                Longitude = 0
            };
            int id = EtabPaieMgr.AddEtablissementPaie(etablissement);
            var etablissements = EtabPaieMgr.AgencesDeRattachement(id);
            var etabsId = etablissements.Select(e => e.EtablissementPaieId).ToList();

            Assert.IsTrue(etablissements != null && !etabsId.Contains(id));
        }

        /// <summary>
        ///   Teste La récupération d'un établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetEtablissementPaieById()
        {

            EtablissementPaieEnt etablissement = new EtablissementPaieEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                SocieteId = 1,
                Libelle = "EtabPaieTU",
                IsAgenceRattachement = false,
                HorsRegion = true,
                Actif = true,
                Adresse = "35 avenue des Oranges Bleues",
                Latitude = 0,
                Longitude = 0
            };
            int id = EtabPaieMgr.AddEtablissementPaie(etablissement);

            EtablissementPaieEnt etablissement2 = EtabPaieMgr.GetEtablissementPaieById(id);
            Assert.IsTrue(etablissement2 != null);
        }

        /// <summary>
        ///   Teste l'ajout un nouvel établissement de paie.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void AddEtablissementPaie()
        {


            int nbEtablissementsBefore = EtabPaieMgr.GetEtablissementPaieList().Count();

            EtablissementPaieEnt etablissement = new EtablissementPaieEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                SocieteId = 1,
                Libelle = "EtabPaieTU",
                IsAgenceRattachement = false,
                HorsRegion = true,
                Actif = true,
                Adresse = "35 avenue des Oranges Bleues",
                Latitude = 0,
                Longitude = 0
            };
            int id = EtabPaieMgr.AddEtablissementPaie(etablissement);

            int nbEtablissementsAfter = EtabPaieMgr.GetEtablissementPaieList().Count();

            Assert.IsTrue(nbEtablissementsAfter == (nbEtablissementsBefore + 1));
        }

        /// <summary>
        ///   Teste la sauvegarde les modifications d'un établissement de paie.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void UpdateEtablissementPaie()
        {

            EtablissementPaieEnt etablissement = new EtablissementPaieEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                SocieteId = 1,
                Libelle = "EtabPaieTU",
                IsAgenceRattachement = false,
                HorsRegion = true,
                Actif = true,
                Adresse = "35 avenue des Oranges Bleues",
                Latitude = 0,
                Longitude = 0
            };
            int id = EtabPaieMgr.AddEtablissementPaie(etablissement);
            string libelleBefore = EtabPaieMgr.GetEtablissementPaieById(id).Libelle;
            etablissement.Libelle = "UpdatedEtabPaieTU";
            EtabPaieMgr.UpdateEtablissementPaie(etablissement);
            string libelleAfter = EtabPaieMgr.GetEtablissementPaieById(id).Libelle;
            Assert.IsTrue(libelleBefore != libelleAfter);
        }

        /// <summary>
        ///   Teste la suppression un établissement de paie.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteEtablissementPaieById()
        {

            EtablissementPaieEnt etablissement = new EtablissementPaieEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                SocieteId = 1,
                Libelle = "EtabPaieTU",
                IsAgenceRattachement = false,
                HorsRegion = true,
                Actif = true,
                Adresse = "35 avenue des Oranges Bleues",
                Latitude = 0,
                Longitude = 0
            };
            int id = EtabPaieMgr.AddEtablissementPaie(etablissement);
            int nbEtablissementsBefore = EtabPaieMgr.GetEtablissementPaieList().Count();
            Assert.IsTrue(EtabPaieMgr.DeleteEtablissementPaieById(etablissement));
            int nbEtablissementsAfter = EtabPaieMgr.GetEtablissementPaieList().Count();
            Assert.IsTrue(nbEtablissementsAfter == (nbEtablissementsBefore - 1));
        }
    }
}