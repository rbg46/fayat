using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests
{
    [TestClass]
    public class CacheManagerTests
    {
        private ICacheManager cacheMgr;

        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
        }

        /// <summary>
        ///   Initialise un test, cette méthode s'exécute avant chaque test
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this.cacheMgr = new CacheManager(new LogManager());
        }

        /// <summary>
        ///   Teste la mise en cache d'une chaîne de caractères
        /// </summary>
        [TestMethod]
        public void CacheTextReturnText()
        {
            string cacheKey = Guid.NewGuid().ToString();
            string textToCache = Guid.NewGuid().ToString();

            string textReturn = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache), TimeSpan.FromSeconds(30));

            Assert.IsTrue(string.Compare(textToCache, textReturn) == 0);
        }

        /// <summary>
        ///   Teste la mise en cache d'un object null
        /// </summary>
        [TestMethod]
        public void CacheObjectNullReturnNull()
        {
            string cacheKey = Guid.NewGuid().ToString();

            string textReturn = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnNull(), TimeSpan.FromSeconds(30));

            Assert.IsNull(textReturn);
        }

        /// <summary>
        ///   Teste la récupération en cache d'une chaîne de caractères.
        /// </summary>
        [TestMethod]
        public void CacheTextTwiceReturnTheFirstText()
        {
            string cacheKey = Guid.NewGuid().ToString();
            string textToCache1 = Guid.NewGuid().ToString();
            string textToCache2 = Guid.NewGuid().ToString();

            string textReturn1 = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache1), TimeSpan.FromSeconds(30));
            string textReturn2 = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache2), TimeSpan.FromSeconds(30));

            Assert.IsTrue(string.Compare(textToCache1, textReturn1) == 0);
        }

        /// <summary>
        ///   Teste la suppression d'une clé de cache
        /// </summary>
        [TestMethod]
        public void RemoveCacheKey()
        {
            string cacheKey = Guid.NewGuid().ToString();
            string textToCache1 = Guid.NewGuid().ToString();
            string textToCache2 = Guid.NewGuid().ToString();

            string textReturn1 = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache1), TimeSpan.FromSeconds(30));

            this.cacheMgr.Remove(cacheKey);

            string textReturn2 = this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache2), TimeSpan.FromSeconds(30));

            Assert.IsTrue(string.Compare(textToCache2, textReturn2) == 0);
        }

        /// <summary>
        ///   Teste la suppression non existante
        /// </summary>
        [TestMethod]
        public void RemoveNonExistingCacheKey()
        {
            string cacheKey = Guid.NewGuid().ToString();

            try
            {
                this.cacheMgr.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        /// <summary>
        ///   Teste la récuperation des éléments de cache
        /// </summary>
        [TestMethod]
        public void AddCacheItemThenGetAllCacheItems()
        {
            // Générer une clé de cache 
            string cacheKey = Guid.NewGuid().ToString();
            // Générer un texte à ajouter dans le cache
            string textToCache = Guid.NewGuid().ToString();
            var countBeforeAdd = this.cacheMgr.GetAll().Count;
            // Ajouter un objet dans le cache
            this.cacheMgr.GetOrCreate(cacheKey, () => ReturnTheTextInParameter(textToCache), TimeSpan.FromSeconds(30));
            var countAfterAdd = this.cacheMgr.GetAll().Count;
            // Récuperer la liste de cache 
            var cacheList = this.cacheMgr.GetAll();
            var cacheValue = cacheList.Find(c => c.Key == cacheKey);
            // Asserts 
            Assert.AreEqual(cacheValue.CacheItem, textToCache);
            Assert.AreEqual(countBeforeAdd + 1, countAfterAdd);
        }

        /// <summary>
        ///   Retourne un texte passé en paramètre
        /// </summary>
        /// <param name="text"> Texte à retourner</param>
        /// <returns> Texte en paramètre</returns>
        private string ReturnTheTextInParameter(string text)
        {
            return text;
        }

        /// <summary>
        ///   Retourne une valeur à null
        /// </summary>
        /// <returns>null</returns>
        private string ReturnNull()
        {
            return null;
        }
    }
}
