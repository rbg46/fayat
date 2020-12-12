
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire.Helpers;
using Fred.Business.CI;
using Fred.Business.Depense.Services;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;

namespace Fred.Business.Tests.Budget.ControleBudgetaire.DataSourceBuilder.Cumul
{

    /// <summary>
    /// Cette classe Teste les fonctionnalités propres a la gestion en cumulée
    /// C'est à dire les dépenses et l'avancement.
    /// Pour tester les valeurs budgétées, les PFA et les Ajustement voir le test AbstractDataSourceBuilderTest
    /// </summary>
    [TestClass]
    public class DataSourceBuilderValeursCumulesAvancementt
    {
        private readonly Mock<IControleBudgetaireManager> controleBudgetaireManager = new Mock<IControleBudgetaireManager>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IDepenseServiceMediator> depenseServiceMediator = new Mock<IDepenseServiceMediator>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();

        private ValeursCumuleesDataSourceBuilder dataSourceBuilder;


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
        public async Task TestBuildDataSourcePasAvancementAsync()
        {
            var fakeT4 = GetFakeBudgetT4();

            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeT4 });

            int periodeCourante = 201811;
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeT4.BudgetId, periodeCourante).ConfigureAwait(false);
            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);
            Assert.AreEqual(0, source.Valeurs.Single()["MontantDad"]);
            Assert.AreEqual(0, source.Valeurs.Single()["PourcentageAvancement"]);
        }


        [TestMethod]
        public async Task TestBuildDataSourceAvecAvancementPourcentageAsync()
        {
            var fakeT4 = GetFakeBudgetT4();

            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeT4 });

            //L'état de l'avancemenet est volontairement laissé null (ce qui ne sera jamais le cas en fonctionnement normal) 
            //Car l'état de l'avancement ne doit pas avoir d'incidence sur le déroulement du programme
            var budgetSousDetailId = fakeT4.BudgetSousDetails.First().BudgetSousDetailId;
            var avancementEnt = new AvancementEnt
            {
                PourcentageSousDetailAvance = 50,
                DAD = 50,
                Periode = 201809,
                BudgetSousDetailId = budgetSousDetailId
            };

            int periodeCourante = 201811;
            avancementManager.Setup(am => am.GetLastAvancementAvantPeriodes(budgetSousDetailId, periodeCourante))
                    .Returns(new List<AvancementEnt> { avancementEnt });

            fakeT4.TypeAvancement = (int)TypeAvancementBudget.Pourcentage;
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeT4.BudgetId, periodeCourante).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);

            Assert.AreEqual(avancementEnt.DAD, source.Valeurs.Single()["MontantDad"]);
            Assert.AreEqual(avancementEnt.PourcentageSousDetailAvance, source.Valeurs.Single()["PourcentageAvancement"]);
        }


        [TestMethod]
        public async Task TestBuildDataSourceAvecAvancementQuantiteAsync()
        {
            var fakeT4 = GetFakeBudgetT4();

            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeT4 });

            //L'état de l'avancemenet est volontairement laissé null (ce qui ne sera jamais le cas en fonctionnement normal) 
            //Car l'état de l'avancement ne doit pas avoir d'incidence sur le déroulement du programme
            var avancementEnt = new AvancementEnt
            {
                QuantiteSousDetailAvancee = 5m,
                DAD = 20,
                Periode = 201809,
                BudgetSousDetailId = 1
            };

            int periodeCourante = 201811;
            avancementManager.Setup(am => am.GetLastAvancementAvantPeriodes(1, periodeCourante))
                .Returns(new List<AvancementEnt> { avancementEnt });

            fakeT4.TypeAvancement = (int)TypeAvancementBudget.Quantite;

            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeT4.BudgetId, periodeCourante).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);

            //Le montant du DAD 
            Assert.AreEqual(avancementEnt.DAD, source.Valeurs.Single()["MontantDad"]);

            //Le pourcentage de l'avancement doit se calculer à partir du Sous détail
            var expectedPourcentageAvancement = avancementEnt.QuantiteSousDetailAvancee / fakeT4.BudgetSousDetails.First().Quantite * 100;
            Assert.AreEqual(expectedPourcentageAvancement, source.Valeurs.Single()["PourcentageAvancement"]);
        }
    }
}
