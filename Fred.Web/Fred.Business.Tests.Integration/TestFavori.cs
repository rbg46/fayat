using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Favori;
using Fred.Framework.Tool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestFavori : FredBaseTest
    {

        /// <summary>
        ///   Teste que la liste des favoris retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFavoriList()
        {
            var notifications = FavoriMgr.GetFavoriList();
            Assert.IsTrue(notifications != null);
        }

        /// <summary>
        ///   Teste la récupération d'une favori à partir de son identifiant unique.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetFavoriById()
        {

            FavoriEnt favori = new FavoriEnt
            {
                UtilisateurId = SuperAdminUserId,
                Search = null
            };
            int notificationId = FavoriMgr.AddFavori(favori).FavoriId;

            FavoriEnt notificationRecuperee = FavoriMgr.GetFavoriById(notificationId);
            Assert.IsTrue(notificationRecuperee != null);
        }

        /// <summary>
        ///   Teste la récupération des favori d'un utilisateur
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFavorisByIdUtilisateur()
        {


            var allUserFavoris = FavoriMgr.SearchFavoris(3);

            Assert.IsTrue(allUserFavoris != null);
        }

        /// <summary>
        ///   Teste la création d'une favori dans le référentiel.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddFavori()
        {

            int nbFavorisBefore = FavoriMgr.GetFavoriList().Count();

            var test = SerialisationTools.Serialisation("test serialisation AddFavori");

            FavoriEnt favori = new FavoriEnt
            {
                UtilisateurId = SuperAdminUserId,
                Search = test
            };

            object testStr = SerialisationTools.Deserialisation(test);
            int favoriId = FavoriMgr.AddFavori(favori).FavoriId;
            int nbFavorisAfter = FavoriMgr.GetFavoriList().Count();
            Assert.IsTrue(nbFavorisAfter > nbFavorisBefore);
        }

        /// <summary>
        ///   Teste la mise à jour d'un favori.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void UpdateFavori()
        {

            SearchCIEnt test = new SearchCIEnt();

            FavoriEnt favori = new FavoriEnt
            {
                UtilisateurId = SuperAdminUserId,
                Search = null
            };
            string libelle = favori.Libelle;
            int favoriId = FavoriMgr.AddFavori(favori).FavoriId;
            favori.Libelle = "Test update JCA";
            FavoriMgr.UpdateFavori(favori);
            string updatedLibelle = FavoriMgr.GetFavoriById(favoriId).Libelle;
            Assert.IsTrue(updatedLibelle != libelle);
        }
    }
}