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
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;

namespace Fred.Business.Tests.Budget.ControleBudgetaire.DataSourceBuilder.MoisCourant
{

    /// <summary>
    /// Le fonctionnnement en mois courant change surtout au niveau de l'avancement
    /// Etant donné que les dépenses sont récupérées via le DepenseManager ce n'est pas à ce test de vérifier que le dépense manager 
    /// nous renvoit bien des dépenses au mois courant lorsqu'on lui demande
    /// </summary>
    [TestClass]
    public class DataSourceBuilderValeursMoisCourant
    {
        private readonly Mock<IControleBudgetaireManager> controleBudgetaireManager = new Mock<IControleBudgetaireManager>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IDepenseServiceMediator> depenseServiceMediator = new Mock<IDepenseServiceMediator>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();

        private AbstractControleBudgetaireDataSourceBuilder dataSourceBuilder;
        private AvancementEnt avancementOctobre;
        private AvancementEnt avancementSeptembre;
        private const int periodeOctobre = 201810;
        private const int periodeSeptembre = 201809;

        [TestInitialize]
        public void Init()
        {
            //L'état de l'avancemenet est volontairement laissé null (ce qui ne sera jamais le cas en fonctionnement normal) 
            //Car l'état de l'avancement ne doit pas avoir d'incidence sur le déroulement du programme
            avancementSeptembre = new AvancementEnt
            {
                PourcentageSousDetailAvance = 25,
                DAD = 25,
                Periode = periodeSeptembre,
                BudgetSousDetailId = 1
            };

            //L'état de l'avancemenet est volontairement laissé null (ce qui ne sera jamais le cas en fonctionnement normal) 
            //Car l'état de l'avancement ne doit pas avoir d'incidence sur le déroulement du programme
            avancementOctobre = new AvancementEnt
            {
                PourcentageSousDetailAvance = 50,
                DAD = 50,
                Periode = periodeOctobre,
                BudgetSousDetailId = 1
            };

            budgetManager.Setup(x => x.GetDateMiseEnApplicationBudgetSurCi(It.IsAny<int>()))
                .Returns(() => DateTime.Now);

            avancementManager.Setup(am => am.GetAvancements(1, periodeOctobre))
                .Returns(new List<AvancementEnt>() { avancementOctobre });


            avancementManager.Setup(am => am.GetAvancements(1, periodeSeptembre))
                .Returns(new List<AvancementEnt>() { avancementSeptembre });


            ciManager.Setup(cim => cim.GetSocieteByCIId(It.IsAny<int>(), false))
                .Returns(GetFakeSociete);

            dataSourceBuilder = new ValeursMoisCourantDataSourceBuilder(controleBudgetaireManager.Object, depenseServiceMediator.Object, budgetManager.Object, budgetT4Manager.Object, ciManager.Object, avancementManager.Object);

        }

        /// <summary>
        /// Test de la gestion de l'avancement en mois courant avec un avancement en pourcentage.
        /// Il n'est pas nécessaire de tester avec un avancement en Quantite car cette différence n'a d'impact que dans d'autres fonctions
        /// qui ne sont qu'appelées par le DataSourceBuilder. La seule chose que l'on veut tester c'est que le DataSourceBuilder cherche bien la différence
        /// entre deux avancement
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task TestBuildDataSourceAvancementAsync()
        {
            var fakeT4 = GetFakeBudgetT4();

            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeT4.BudgetId, true))
                .Returns(new List<BudgetT4Ent>() { fakeT4 });

            fakeT4.TypeAvancement = (int)TypeAvancementBudget.Pourcentage;

            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeT4.BudgetId, periodeOctobre).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);

            var expectedDad = avancementOctobre.DAD - avancementSeptembre.DAD;
            var expectedPourcentage = avancementOctobre.PourcentageSousDetailAvance - avancementSeptembre.PourcentageSousDetailAvance;

            //25 pour le montant car la différence de montant DAD entre octobre et septembre et de 25
            Assert.AreEqual(expectedDad, source.Valeurs.Single()["MontantDad"]);

            //25 pour le pourcentage de l'avancement car la différence entre le pourcentage avancement d'octobre et septembre est de 25
            Assert.AreEqual(expectedPourcentage, source.Valeurs.Single()["PourcentageAvancement"]);

        }
    }
}
