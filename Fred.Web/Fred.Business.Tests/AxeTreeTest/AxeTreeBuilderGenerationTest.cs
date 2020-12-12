using System.Linq;
using Fred.Business.Budget.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;

namespace Fred.Business.Tests.AxeTreeTest
{
    [TestClass]
    public class AxeTreeBuilderGenerationTest
    {

        private AxeTreeDataSource sources = new AxeTreeDataSource();
        private decimal expectedAverage;
        private int expectedSum;
        private int expectedLeave;

        [TestInitialize]
        public void Init()
        {
            sources.AddColumn("Average", ComputeColumnValueForAxe.ComputeAverageValueOfColumn);
            sources.AddColumn("Leaves", ComputeColumnValueForAxe.DisplayOnlyInLeaves);
            sources.AddColumn("Sum", ComputeColumnValueForAxe.SumChildrenValues);

            var fakeTache3 = GetFakeTache3();
            var fakeRessource = GetFakeRessource();
            for (int i = 0; i < 10; i++)
            {
                var row = sources.NewRow();
                row.Ressource = fakeRessource;
                row.Tache3 = fakeTache3;

                row["Average"] = i + 1m;
                row["Leaves"] = 2;
                row["Sum"] = 3;

                sources.Valeurs.Add(row);
            }

            expectedAverage = 5.5m;
            expectedSum = 30;
            expectedLeave = 2;
        }

        [TestMethod]
        public void GenerateTacheRessourceDefaultTree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.TacheRessource, null);
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : T1-T2-T3-Chapitre-SousChapitre-Ressource
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeT1 = tree.Single();
            var axeT2 = axeT1.SousAxe.Single();
            var axeT3 = axeT2.SousAxe.Single();
            var axeChapitre = axeT3.SousAxe.Single();
            var axeSousChapitre = axeChapitre.SousAxe.Single();
            var axeRessource = axeSousChapitre.SousAxe.Single();

            Assert.AreEqual(expectedAverage, axeT1.Valeurs["Average"]);
            Assert.IsNull(axeT1.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT1.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT2.Valeurs["Average"]);
            Assert.IsNull(axeT2.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT2.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT3.Valeurs["Average"]);
            Assert.IsNull(axeT3.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT3.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeChapitre.Valeurs["Average"]);
            Assert.IsNull(axeChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeSousChapitre.Valeurs["Average"]);
            Assert.IsNull(axeSousChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeSousChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeRessource.Valeurs["Average"]);
            Assert.AreEqual(expectedLeave, axeRessource.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeRessource.Valeurs["Sum"]);
        }

        [TestMethod]
        public void GenerateRessourceTacheDefaultTree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.RessourceTache, null);
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : Chapitre-SousChapitre-Ressource-T1-T2-T3
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeChapitre = tree.Single();
            var axeSousChapitre = axeChapitre.SousAxe.Single();
            var axeRessource = axeSousChapitre.SousAxe.Single();
            var axeT1 = axeRessource.SousAxe.Single();
            var axeT2 = axeT1.SousAxe.Single();
            var axeT3 = axeT2.SousAxe.Single();


            Assert.AreEqual(expectedAverage, axeChapitre.Valeurs["Average"]);
            Assert.IsNull(axeChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeSousChapitre.Valeurs["Average"]);
            Assert.IsNull(axeSousChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeSousChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeRessource.Valeurs["Average"]);
            Assert.IsNull(axeRessource.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeRessource.Valeurs["Sum"]);


            Assert.AreEqual(expectedAverage, axeT1.Valeurs["Average"]);
            Assert.IsNull(axeT1.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT1.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT2.Valeurs["Average"]);
            Assert.IsNull(axeT2.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT2.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT3.Valeurs["Average"]);
            Assert.AreEqual(expectedLeave, axeT3.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT3.Valeurs["Sum"]);
        }

        [TestMethod]
        public void GenerateTacheOnlyDefaultTree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.TacheOnly, null);
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : T1-T2-T3
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeT1 = tree.Single();
            var axeT2 = axeT1.SousAxe.Single();
            var axeT3 = axeT2.SousAxe.Single();

            Assert.AreEqual(expectedAverage, axeT1.Valeurs["Average"]);
            Assert.IsNull(axeT1.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT1.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT2.Valeurs["Average"]);
            Assert.IsNull(axeT2.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT2.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT3.Valeurs["Average"]);
            Assert.AreEqual(expectedLeave,axeT3.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT3.Valeurs["Sum"]);
        }

        [TestMethod]
        public void GenerateRessourceOnlyDefaultTree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.RessourceOnly, null);
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : Chapitre-SousChapitre-Ressource
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeChapitre = tree.Single();
            var axeSousChapitre = axeChapitre.SousAxe.Single();
            var axeRessource = axeSousChapitre.SousAxe.Single();

            Assert.AreEqual(expectedAverage, axeChapitre.Valeurs["Average"]);
            Assert.IsNull(axeChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeSousChapitre.Valeurs["Average"]);
            Assert.IsNull(axeSousChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeSousChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeRessource.Valeurs["Average"]);
            Assert.AreEqual(expectedLeave, axeRessource.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeRessource.Valeurs["Sum"]);
        }
    }
}
