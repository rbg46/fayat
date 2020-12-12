using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire.Helpers;
using Fred.Business.Budget.Helpers;
using Fred.Business.CI;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;

namespace Fred.Business.Tests.Budget.ControleBudgetaire.DataSourceBuilder.Cumul
{
    [TestClass]
    public class DataSourceBuilderValeursCumuleesDepenses
    {
        private readonly Mock<IControleBudgetaireManager> controleBudgetaireManager = new Mock<IControleBudgetaireManager>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IDepenseServiceMediator> depenseServiceMediator = new Mock<IDepenseServiceMediator>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();

        private readonly ICollection<DepenseExhibition> depenses = new List<DepenseExhibition>();
        private AbstractControleBudgetaireDataSourceBuilder dataSourceBuilder;


        [TestInitialize]
        public void Init()
        {
            ciManager.Setup(cim => cim.GetSocieteByCIId(It.IsAny<int>(), false))
                .Returns(GetFakeSociete);

            budgetManager.Setup(x => x.GetDateMiseEnApplicationBudgetSurCi(It.IsAny<int>()))
                .Returns(() => DateTime.Now);

            avancementManager.Setup(am => am.GetLastAvancementAvantPeriodes(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<AvancementEnt>());

            dataSourceBuilder = new ValeursCumuleesDataSourceBuilder(controleBudgetaireManager.Object, depenseServiceMediator.Object, budgetManager.Object, budgetT4Manager.Object, ciManager.Object, avancementManager.Object);


        }

        [TestMethod]
        public async Task TestBuildDataSourcePasDepensesAsync()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeBudgetT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeBudgetT4 });

            int periodeCourante = 201811;
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeBudgetT4.BudgetId, periodeCourante).ConfigureAwait(false);
            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);
            Assert.AreEqual(0, source.Valeurs.Single()["MontantDepense"]);

            var quantiteDepense = source.Valeurs.Single()["QuantiteDepense"] as RessourceUnite;
            Assert.AreEqual(0, quantiteDepense.Quantite);
            Assert.IsNull(quantiteDepense.Unite);
        }


        [TestMethod]
        public async Task TestBuildDataSourceJusteUneDepense()
        {
            var fakeRessource = GetFakeRessource();
            var fakeTache4 = GetFakeTache4();
            var fakeUnite = GetFakeUnite();
            var depense = new DepenseExhibition()
            {
                Ressource = fakeRessource,
                RessourceId = fakeRessource.RessourceId,
                Tache = fakeTache4.Parent,
                TacheId = fakeTache4.ParentId.Value,
                MontantHT = 1200,
                Quantite = 20,
                PUHT = 60,
                Unite = fakeUnite,
                UniteId = fakeUnite.UniteId
            };

            depenses.Add(depense);

            depenseServiceMediator.Setup(edm => edm.GetAllDepenseExternetWithTacheAndRessourceAsync(It.IsAny<SearchDepense>()))
                .Returns(Task.FromResult<IEnumerable<DepenseExhibition>>(depenses));

            var fakeBudgetT4 = GetFakeBudgetT4();
            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeBudgetT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeBudgetT4 });

            //La période n'a pas d'importance mais doit être correcte
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeBudgetT4.BudgetId, 201801).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);

            Assert.AreEqual(depense.MontantHT, source.Valeurs.Single()["MontantDepense"]);
            var quantiteDepense = source.Valeurs.Single()["QuantiteDepense"] as RessourceUnite;
            Assert.AreEqual(depense.Quantite, quantiteDepense.Quantite);
            Assert.AreEqual(depense.Unite.Code, quantiteDepense.Unite);
        }

        [TestMethod]
        public async Task TestBuildDataSourceSurDepenseNonBudgeteAsync()
        {
            var fakeRessource = GetFakeRessource();
            var fakeTache4 = GetFakeTache4();
            var fakeUnite = GetFakeUnite();
            var depense = new DepenseExhibition()
            {
                Ressource = fakeRessource,
                RessourceId = fakeRessource.RessourceId,
                Tache = fakeTache4.Parent,
                TacheId = fakeTache4.ParentId.Value,
                MontantHT = 1200,
                Quantite = 20,
                PUHT = 60,
                Unite = fakeUnite,
                UniteId = fakeUnite.UniteId
            };

            depenses.Add(depense);

            depenseServiceMediator.Setup(edm => edm.GetAllDepenseExternetWithTacheAndRessourceAsync(It.IsAny<SearchDepense>()))
                .Returns(Task.FromResult<IEnumerable<DepenseExhibition>>(depenses));

            //On reset la fonction GetByBudgetId qui a été initialisé dans la fonction Init ici car on ne veut pas 
            //De T4 budgété 
            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(It.IsAny<int>(), true))
                .Returns(new List<BudgetT4Ent>() { });

            //La période n'a pas d'importance mais doit être correcte
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, 1, 201801).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);
            Assert.IsNull(source.Valeurs.Single()["MontantBudget"]);

            //Le PU (et la quantité) est calculé comme étant la moyenne des PU de tous les sous détails du T4
            Assert.IsNull(source.Valeurs.Single()["PuBudget"]);
            Assert.IsNull(source.Valeurs.Single()["QuantiteBudget"]);

            Assert.AreEqual(0, source.Valeurs.Single()["MontantAjustement"]);

            //Le PFA Mois précédent est particulier, car s'il n'existe pas alors rien ne doit être affiché (pas même 0)
            Assert.IsNull(source.Valeurs.Single()["PfaMoisPrecedent"]);
            Assert.IsNull(source.Valeurs.Single()["MontantDad"]);

            Assert.AreEqual(depense.MontantHT, source.Valeurs.Single()["MontantDepense"]);
            var quantiteDepense = source.Valeurs.Single()["QuantiteDepense"] as RessourceUnite;
            Assert.AreEqual(depense.Quantite, quantiteDepense.Quantite);
            Assert.AreEqual(depense.Unite.Code, quantiteDepense.Unite);

        }

        [TestMethod]
        public async Task TestBuildDataSourceUneDepensePartageParDeuxT4Async()
        {

            //La dépense est comptée au niveau du T3, dans le cas de ce test
            //Le T3 possède deux T4, lorsqu'on construit la source de données la dépense ne doit en aucun cas être comptée
            //deux fois (une pour chaque T4) la première ligne correspondant au premier T4 trouvé contiendra la dépense
            //et la deuxième ligne ne contiendra que des 0

            var fakeRessource = GetFakeRessource();
            var fakeTache4 = GetFakeTache4();
            var depense = new DepenseExhibition()
            {
                Ressource = fakeRessource,
                RessourceId = fakeRessource.RessourceId,
                Tache = fakeTache4.Parent,
                TacheId = fakeTache4.ParentId.Value,
                MontantHT = 1200,
                Quantite = 20,
                PUHT = 60
            };

            depenses.Add(depense);
            depenseServiceMediator.Setup(edm => edm.GetAllDepenseExternetWithTacheAndRessourceAsync(It.IsAny<SearchDepense>()))
                .Returns(Task.FromResult<IEnumerable<DepenseExhibition>>(depenses));


            var Tache4_2 = new TacheEnt
            {
                Niveau = 4,
                TacheId = 4,
                CI = fakeTache4.CI,
                CiId = fakeTache4.CiId,
                Code = "Fake T4 2",
                Libelle = "Fake Tache 4 2",
                ParentId = fakeTache4.TacheId,
                Parent = fakeTache4
            };


            var fakeBudgetT4 = GetFakeBudgetT4();
            var fakeUnite = GetFakeUnite();
            var nouveauT4 = new BudgetT4Ent()
            {
                QuantiteARealiser = 1,
                MontantT4 = 100,
                PU = 100,
                T4 = Tache4_2,
                T4Id = Tache4_2.TacheId,
                Budget = fakeBudgetT4.Budget,
                BudgetId = fakeBudgetT4.BudgetId,
                BudgetT4Id = 1,
                BudgetSousDetails = new List<BudgetSousDetailEnt>()
                {
                    new BudgetSousDetailEnt()
                    {
                        Quantite = 10,
                        PU = 10,
                        Montant = 100,
                        Ressource = fakeRessource,
                        RessourceId = fakeRessource.RessourceId,
                        Unite = fakeUnite,
                        UniteId = fakeUnite.UniteId,
                        BudgetSousDetailId = 1
                    }
                }
            };

            nouveauT4.BudgetSousDetails.First().BudgetT4 = nouveauT4;
            nouveauT4.BudgetSousDetails.First().BudgetT4Id = nouveauT4.BudgetT4Id;

            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeBudgetT4.BudgetId, true))
                    .Returns(new List<BudgetT4Ent>() { fakeBudgetT4, nouveauT4 });

            //La période n'a pas d'importance mais doit être correcte
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeBudgetT4.BudgetId, 201801).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(2, source.Valeurs.Count);

            //La première ligne contient la dépense
            Assert.AreEqual(depense.MontantHT, source.Valeurs.First()["MontantDepense"]);
            var quantiteDepense = source.Valeurs.First()["QuantiteDepense"] as RessourceUnite;
            Assert.AreEqual(depense.Quantite, quantiteDepense.Quantite);
            Assert.IsNull(quantiteDepense.Unite);

            //La deuxième ne contient que des 0
            Assert.AreEqual(0, source.Valeurs.ElementAt(1)["MontantDepense"]);
            quantiteDepense = source.Valeurs.ElementAt(1)["QuantiteDepense"] as RessourceUnite;
            Assert.AreEqual(0, quantiteDepense.Quantite);
            Assert.IsNull(quantiteDepense.Unite);
        }
    }
}
