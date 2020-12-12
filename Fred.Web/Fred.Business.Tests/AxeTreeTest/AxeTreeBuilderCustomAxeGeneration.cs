using System;
using System.Linq;
using Fred.Business.Budget.Helpers;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;


namespace Fred.Business.Tests.AxeTreeTest
{
    [TestClass]
    public class AxeTreeBuilderCustomAxeGeneration
    {
        private AxeTreeDataSource sources = new AxeTreeDataSource();
        private decimal expectedAverage;
        private int expectedSum;

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
        }



        [TestMethod]
        public void GenerateTacheRessourceT1ChapitreTree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.TacheRessource,null, new AxeTypes[] { AxeTypes.T1, AxeTypes.Chapitre } );
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : T1-Chapitre
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeT1 = tree.Single();
            var axeChapitre = axeT1.SousAxe.Single();

            //NOTE : La fonction DisplayInLeaves n'affiche la valeur que lorsqu'on est sur une feuille pour la structure par défaut de l'axe principal choisi
            //Dans notre cas on a une structure personnalisée (T1-Chapitre) donc pas de feuille

            Assert.AreEqual(expectedAverage, axeT1.Valeurs["Average"]);
            Assert.IsNull(axeT1.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT1.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeChapitre.Valeurs["Average"]);
            Assert.IsNull(axeChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeChapitre.Valeurs["Sum"]);
        }

        [TestMethod]
        public void GenerateTacheRessourceChapitreT1Tree()
        {
            var axe = new AxeTreeBuilder(sources, AxePrincipal.RessourceTache,null, new AxeTypes[] { AxeTypes.Chapitre, AxeTypes.T1 } );
            var tree = axe.GenerateTree();

            //Dans le cas du test, on doit avoir une structure d'arbre "linéaire" 
            //c'est à dire que l'arbre doit être sous cette forme : Chapitre-T1
            //Chaque noeud de l'arbre n'a qu'un seul et unique enfant
            var axeChapitre = tree.Single();
            var axeT1 = axeChapitre.SousAxe.Single();

            //NOTE : La fonction DisplayInLeaves n'affiche la valeur que lorsqu'on est sur une feuille pour la structure par défaut de l'axe principal choisi
            //Dans notre cas on a une structure personnalisée (Chapitre-T1) donc pas de feuille
            Assert.AreEqual(expectedAverage, axeChapitre.Valeurs["Average"]);
            Assert.IsNull(axeChapitre.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeChapitre.Valeurs["Sum"]);

            Assert.AreEqual(expectedAverage, axeT1.Valeurs["Average"]);
            Assert.IsNull(axeT1.Valeurs["Leaves"]);
            Assert.AreEqual(expectedSum, axeT1.Valeurs["Sum"]);

        }

        [TestMethod]
        [ExpectedException(typeof(FredBusinessException))]
        public void GenerateTacheOnlyTreeWithAxePrincipalRessource()
        {
            //Lorsqu'on demande la création d'un arbre personnalisé avec uniquement des tâches et un axe principal ressource
            //On doit lancer une exception, l'axe principale ne peut pas être ressource si aucune ressource n'est présente dans l'arbre final

            var axe = new AxeTreeBuilder(sources, AxePrincipal.RessourceTache,null, new AxeTypes[] { AxeTypes.T1, AxeTypes.T2 });
            var tree = axe.GenerateTree();
        }

        [TestMethod]
        [ExpectedException(typeof(FredBusinessException))]
        public void GenerateRessourceOnlyTreeWithAxePrincipalTache()
        {
            //Lorsqu'on demande la création d'un arbre personnalisé avec uniquement des ressources et un axe principal Tache
            //On doit lancer une exception, l'axe principale ne peut pas être Tache si aucune Tache n'est présente dans l'arbre final

            var axe = new AxeTreeBuilder(sources, AxePrincipal.TacheRessource,null, new AxeTypes[] { AxeTypes.Chapitre, AxeTypes.Ressource });
            var tree = axe.GenerateTree();
        }


    }

}
