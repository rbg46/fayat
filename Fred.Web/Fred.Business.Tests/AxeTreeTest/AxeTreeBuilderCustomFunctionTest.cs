using System.Linq;
using FluentAssertions;
using Fred.Business.Budget.ControleBudgetaire.Helpers;
using Fred.Business.Budget.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;

namespace Fred.Business.Tests.AxeTreeTest
{
    [TestClass]
    public class AxeTreeBuilderCustomFunctionTest
    {

        private AxeTreeDataSource sources = new AxeTreeDataSource();
        private decimal expectedQuantite;
        private decimal expectedPu;
        private string expectedCode = GetFakeUnite().Code;


        [TestInitialize]
        public void Init()
        {
            sources.AddColumn("GetPu", CustomComputeColumnValueForAxe.GetPu, "GetMontant", "GetQuantite");
            sources.AddColumn("GetQuantite", CustomComputeColumnValueForAxe.GetQuantite);
            sources.AddColumn("DisplayQtePu", CustomComputeColumnValueForAxe.GetDisplayQtePuFlag);

            var row = sources.NewRow();
            row.Ressource = GetFakeRessource();
            row.Tache3 = GetFakeTache3();

            row["GetMontant"] = 4m;
            row["GetQuantite"] = new RessourceUnite
            {
                Quantite = 1m,
                Unite = expectedCode
            };

            sources.Valeurs.Add(row);

            expectedQuantite = 1m;
            expectedPu = 4m;
        }


        [TestMethod]
        public void TestCustomFunction()
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

            (axeT1.Valeurs["DisplayQtePu"] as int?).Should().Be(0);
            (axeT2.Valeurs["DisplayQtePu"] as int?).Should().Be(0);
            (axeT3.Valeurs["DisplayQtePu"] as int?).Should().Be(0);
            (axeT3.Valeurs["GetPu"] as decimal?).Should().Be(expectedPu);
            (axeT3.Valeurs["GetQuantite"] as RessourceUnite)
                .Should()
                .Match<RessourceUnite>(x => x.Quantite == expectedQuantite
                                            && x.Unite == expectedCode);
            (axeChapitre.Valeurs["DisplayQtePu"] as int?).Should().Be(0);
            (axeSousChapitre.Valeurs["DisplayQtePu"] as int?).Should().Be(0);
            (axeRessource.Valeurs["DisplayQtePu"] as int?).Should().Be(1);
            (axeRessource.Valeurs["GetPu"] as decimal?).Should().Be(expectedPu);
            (axeRessource.Valeurs["GetQuantite"] as RessourceUnite)
                .Should()
                .Match<RessourceUnite>(x => x.Quantite == expectedQuantite
                                            && x.Unite == expectedCode);
        }
    }
}
