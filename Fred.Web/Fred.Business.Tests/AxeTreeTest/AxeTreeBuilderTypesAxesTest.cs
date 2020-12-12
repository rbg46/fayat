using Fred.Business.Budget.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.AxeTreeTest
{
    [TestClass]
    public class AxeTreeBuilderTypesAxesTest
    {
        [TestMethod]
        public void IsNiveauSuperieurForDefaultDetailsTest()
        {
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T1, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T2, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T3, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Ressource, AxePrincipal.TacheRessource));


            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T1, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T2, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T3, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.RessourceTache));
            Assert.IsTrue(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Ressource, AxePrincipal.RessourceTache));

            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T1, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T2, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T3, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Ressource, AxePrincipal.TacheOnly));

            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T1, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T2, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.T3, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(AxeTypes.Ressource, AxePrincipal.RessourceOnly));
        }

        [TestMethod]
        public void IsAxeTacheTest()
        {
            Assert.IsTrue(AxeTreeBuilder.IsAxeTache(AxeTypes.T1));
            Assert.IsTrue(AxeTreeBuilder.IsAxeTache(AxeTypes.T2));
            Assert.IsTrue(AxeTreeBuilder.IsAxeTache(AxeTypes.T3));
            Assert.IsFalse(AxeTreeBuilder.IsAxeTache(AxeTypes.Chapitre));
            Assert.IsFalse(AxeTreeBuilder.IsAxeTache(AxeTypes.SousChapitre));
            Assert.IsFalse(AxeTreeBuilder.IsAxeTache(AxeTypes.Ressource));
        }

        [TestMethod]
        public void IsAxeRessourceTest()
        {
            Assert.IsFalse(AxeTreeBuilder.IsAxeRessource(AxeTypes.T1));
            Assert.IsFalse(AxeTreeBuilder.IsAxeRessource(AxeTypes.T2));
            Assert.IsFalse(AxeTreeBuilder.IsAxeRessource(AxeTypes.T3));
            Assert.IsTrue(AxeTreeBuilder.IsAxeRessource(AxeTypes.Chapitre));
            Assert.IsTrue(AxeTreeBuilder.IsAxeRessource(AxeTypes.SousChapitre));
            Assert.IsTrue(AxeTreeBuilder.IsAxeRessource(AxeTypes.Ressource));
        }

        [TestMethod]
        public void IsLeafForDefaultDetailsTest()
        {
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T1, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T2, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T3, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.TacheRessource));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.TacheRessource));
            Assert.IsTrue(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Ressource, AxePrincipal.TacheRessource));

            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T1, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T2, AxePrincipal.RessourceTache));
            Assert.IsTrue(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T3, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.RessourceTache));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Ressource, AxePrincipal.RessourceTache));

            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T1, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T2, AxePrincipal.TacheOnly));
            Assert.IsTrue(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T3, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.TacheOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Ressource, AxePrincipal.TacheOnly));

            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T1, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T2, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.T3, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Chapitre, AxePrincipal.RessourceOnly));
            Assert.IsFalse(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.SousChapitre, AxePrincipal.RessourceOnly));
            Assert.IsTrue(AxeTreeBuilder.IsLeafForDefaultDetails(AxeTypes.Ressource, AxePrincipal.RessourceOnly));
        }
    }
}
