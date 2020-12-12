using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Common.Tests.Helper;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class ReferentialTests : FredBaseTest
    {
        [TestMethod]
        [TestCategory("DBDepend")]
        public void CheckValidityCIBeforeImportation()
        {
            CIEnt ciEnt = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                EtablissementComptableId = 1,
                DateOuverture = new DateTime(2016, 1, 1)
            };

            Assert.IsTrue(EtabComptableMgr.GetEtablissementComptableById((int)ciEnt.EtablissementComptableId) != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCIPrimeList()
        {
            var ciPrimes = PrimeMgr.GetCIPrimeList();
            Assert.IsTrue(ciPrimes != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public async Task GetPrimesIdsByCiId()
        {
            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "GetCIPrimeList",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;

            CIEnt ci = new CIEnt
            {
                DateOuverture = DateTime.Now,
                EtablissementComptableId = 2,
                Adresse = "aaaaa",
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "test"
            };
            int ciId = CIMgr.AddCI(ci).CiId;

            PrimeMgr.AddCIPrimes(primeId, ciId);
            IEnumerable<int> ciPrime = PrimeMgr.GetPrimesIdsByCiId(ciId);
            Assert.IsTrue(ciPrime != null && ciPrime.Any());

            //Test cleanup
            PrimeMgr.DeleteCIPrimeById(primeId, ciId);
            await PrimeMgr.DeletePrimeAsync(primeId);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public async Task AddCIPrimes()
        {
            int nbPrimesBefore = PrimeMgr.GetCIPrimeList().Count();

            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "toto",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };

            PrimeEnt newPrime = await PrimeMgr.AddPrimeAsync(prime);

            int primeId = newPrime.PrimeId;

            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false,
                EnteteLivraison = "M. Dupont",
                AdresseLivraison = "00 avenue Jacquie Sardou",
                CodePostalLivraison = "69690",
                VilleLivraison = "Vladivostok"
            };
            int ciId = CIMgr.AddCI(ci).CiId;

            PrimeMgr.AddCIPrimes(primeId, ciId);
            int nbPrimesAfter = PrimeMgr.GetCIPrimeList().Count();
            Assert.IsTrue(nbPrimesAfter > nbPrimesBefore);

            //Test cleanup
            PrimeMgr.DeleteCIPrimeById(primeId, ciId);
            await PrimeMgr.DeletePrimeAsync(primeId);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public async Task DeleteCIPrimesByIdsAsync()
        {
            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "toto",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;

            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false,
                EnteteLivraison = "M. Dupont",
                AdresseLivraison = "00 avenue Jacquie Sardou",
                CodePostalLivraison = "69690",
                VilleLivraison = "Vladivostok"
            };
            int ciId = CIMgr.AddCI(ci).CiId;

            PrimeMgr.AddCIPrimes(primeId, ciId);
            int nbPrimesBefore = PrimeMgr.GetCIPrimeList().Count();
            PrimeMgr.DeleteCIPrimeById(primeId, ciId);
            int nbPrimesAfter = PrimeMgr.GetCIPrimeList().Count();
            Assert.IsTrue(nbPrimesAfter < nbPrimesBefore);

            //Test cleanup
            CIMgr.DeleteCIById(ciId);
            await PrimeMgr.DeletePrimeAsync(primeId);
        }
    }
}
