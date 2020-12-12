using System;
using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.DataAccess.Common;
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestCi : FredBaseTest
    {
        #region CI

        /// <summary>
        ///   Teste que la liste des cis n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCIListReturnNotNullList()
        {
            var cis = CIMgr.GetCIList();
            Assert.IsTrue(cis != null);
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewCIClosed()
        {

            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false,
                EnteteLivraison = "M. Dupont",
                AdresseLivraison = "69 avenue des Maréchaux",
                CodePostalLivraison = "69690",
                VilleLivraison = "Vladivostok"
            };

            var repo = new DbRepository<CIEnt>(FredContext);
            int countBefore =
              repo.Query()
                  .Filter(c => c.DateFermeture == null || ci.DateFermeture.Value > DateTime.Now)
                  .Get()
                  .AsNoTracking()
                  .Count();

            CIMgr.AddCI(ci);

            int countAfter = repo.Query()
                                 .Filter(c => c.DateFermeture == null || ci.DateFermeture.Value > DateTime.Now)
                                 .Get()
                                 .AsNoTracking()
                                 .Count();

            Assert.IsTrue(countBefore == countAfter);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void UpdateExistingCI()
        {
            CIEnt ciBefore = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de modification",
                EtablissementComptableId = 1,
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                Sep = false
            };

            int ciId = CIMgr.AddCI(ciBefore).CiId;

            string libBefore = ciBefore.Libelle;
            ciBefore.Libelle = "CI de test après modification";
            ciBefore.EtablissementComptableId = 2;

            CIMgr.UpdateCI(ciBefore);

            CIEnt ciAfter = CIMgr.GetCIById(ciId);

            Assert.AreNotEqual(libBefore, ciAfter.Libelle);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewCI()
        {
            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false,
                EnteteLivraison = "M. Dupont",
                AdresseLivraison = "69 avenue des Maréchaux",
                CodePostalLivraison = "69690",
                VilleLivraison = "Vladivostok"
            };

            var repo = new DbRepository<CIEnt>(FredContext);
            int countBefore = repo.Query().Get().AsNoTracking().Count();

            CIMgr.AddCI(ci);

            int countAfter = repo.Query().Get().AsNoTracking().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste recherche des tâches
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCIListReturnAtLeastOneRecord()
        {


            var cis = CIMgr.GetCIList().ToList();
            Assert.IsTrue(cis.Count > 0);
        }

        /// <summary>
        ///   Teste la récupération de la liste des enregistrements spécifiques en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCIDevisesListByCIid()
        {

            int idCi = 1;

            var ciDevises = CIMgr.GetCIDevise(idCi);
            Assert.IsTrue(ciDevises != null);
        }
        #endregion
    }
}
