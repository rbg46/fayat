using System.Linq;
using System.Threading.Tasks;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Fred.Web.Models.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestPrime : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des primes retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetPrimesList()
        {
            var primes = PrimeMgr.GetPrimesList();
            Assert.IsTrue(primes != null);
        }

        /// <summary>
        ///   Teste que la liste des primes actives retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetActivesPrimesList()
        {

            var primes = PrimeMgr.GetPrimesList();
            Assert.IsTrue(primes != null);
        }

        /// <summary>
        ///   Teste que la liste des primes éligibles à un CI retournée n'est jamais égale à null
        /// </summary>
        /// <summary>
        ///   Teste la récupération d'une prime à partir de son identifiant unique.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public async Task GetPrimeById()
        {
            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "TUGetPrimeById",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;

            PrimeEnt primeRecuperee = await PrimeMgr.GetPrimeByIdAsync(primeId);
            Assert.IsTrue(primeRecuperee != null);
        }

        /// <summary>
        ///   Ajoute une prime
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public async Task AddPrime()
        {
            int nbPrimesBefore = PrimeMgr.GetPrimesList().Count();

            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "AddPrime",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;
            int nbPrimesAfter = PrimeMgr.GetPrimesList().Count();
            Assert.IsTrue(nbPrimesAfter > nbPrimesBefore);

            //Test cleanup
            await PrimeMgr.DeletePrimeAsync(primeId);
        }

        /// <summary>
        ///   Teste la mise à jour d'une prime dans le référentiel.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public async Task UpdatePrime()
        {
            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "UpdatePrime",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;
            string lib = (await PrimeMgr.GetPrimeByIdAsync(primeId)).Libelle;
            prime.Libelle = "Updated TUGetPrimeById";

            var primeModel = new PrimeModel
            {
                PrimeId = primeId,
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = prime.Libelle,
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            await PrimeMgr.UpdatePrimeAsync(primeModel);
            string updatedLib = (await PrimeMgr.GetPrimeByIdAsync(primeId)).Libelle;
            Assert.IsTrue(updatedLib != lib);
        }

        /// <summary>
        ///   Teste la suppression d'une prime du référentiel.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public async Task DeletePrimeById()
        {
            PrimeEnt prime = new PrimeEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "DeletePrimeById",
                Actif = true,
                Publique = true,
                SocieteId = 1
            };
            int primeId = (await PrimeMgr.AddPrimeAsync(prime)).PrimeId;
            int nbPrimesBefore = PrimeMgr.GetPrimesList().Count();
            await PrimeMgr.DeletePrimeAsync(primeId);
            int nbPrimesAfter = PrimeMgr.GetPrimesList().Count();
            Assert.IsTrue(nbPrimesAfter < nbPrimesBefore);
        }
    }
}
