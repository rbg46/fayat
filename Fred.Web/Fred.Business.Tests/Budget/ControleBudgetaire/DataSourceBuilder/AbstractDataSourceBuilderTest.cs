using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire.Helpers;
using Fred.Business.Budget.Helpers;
using Fred.Business.CI;
using Fred.Business.Depense.Services;
using Fred.Entities.Budget;
using Fred.Entities.Societe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;

namespace Fred.Business.Tests.Budget.ControleBudgetaire.DataSourceBuilder
{
    internal class AbstractDataSourceBuilderImplementation : AbstractControleBudgetaireDataSourceBuilder
    {
        private static Mock<IDepenseServiceMediator> mockDepenseServiceMediator = new Mock<IDepenseServiceMediator>();
        private static Mock<ICIManager> mockCIManager = new Mock<ICIManager>();
        private static Mock<IControleBudgetaireManager> mockControleBudgetaireManager = new Mock<IControleBudgetaireManager>();
        private static Mock<IBudgetManager> mockBudgetManager = new Mock<IBudgetManager>();
        private static Mock<IBudgetT4Manager> mockBudgetT4Manager = new Mock<IBudgetT4Manager>();
        private static Mock<IAvancementManager> mockAvancementManager = new Mock<IAvancementManager>();

        public AbstractDataSourceBuilderImplementation(IEnumerable<BudgetT4Ent> budgetT4s, SocieteEnt societe)
            : base(mockControleBudgetaireManager.Object, mockDepenseServiceMediator.Object, mockBudgetManager.Object, mockBudgetT4Manager.Object, mockCIManager.Object, mockAvancementManager.Object)
        {
            mockBudgetManager.Setup(x => x.GetDateMiseEnApplicationBudgetSurCi(It.IsAny<int>())).Returns(() => DateTime.Now);
            mockCIManager.Setup(x => x.GetSocieteByCIId(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => societe);
            this.budgetT4s = budgetT4s;
        }

        private readonly IEnumerable<BudgetT4Ent> budgetT4s;

        public override async Task<AxeTreeDataSource> BuildDataSourceAsync(int ciId, int budgetId, int periode)
        {
            await base.BuildDataSourceAsync(ciId, budgetId, periode).ConfigureAwait(false);

            foreach (var t4 in budgetT4s)
            {
                foreach (var sd in t4.BudgetSousDetails)
                {
                    sources.Valeurs.Add(BuildDataSourceLineForSousDetail(sd, sources));
                }
            }

            return sources;
        }
    }

    [TestClass]
    public class AbstractDataSourceBuilderTest
    {
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();

        private readonly ICollection<BudgetT4Ent> budgetT4s = new List<BudgetT4Ent>();

        [TestInitialize]
        public void Init()
        {
            ciManager.Setup(cim => cim.GetSocieteByCIId(It.IsAny<int>(), false))
                .Returns(GetFakeSociete);
        }

        [TestMethod]
        public async Task TestBuildDataSourceForSousDetailAsync()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            budgetT4s.Add(fakeBudgetT4);
            var dataSourceBuilder = new AbstractDataSourceBuilderImplementation(budgetT4s, GetFakeSociete());

            //La période n'a pas d'importance mais doit être correcte
            var source = await dataSourceBuilder.BuildDataSourceAsync(1, fakeBudgetT4.BudgetId, 201801).ConfigureAwait(false);

            //On est sensé avoir autant de ligne dans la source de données qu'on a de sous détails et de dépenses
            //Dans ce test on a qu'un sous détail
            Assert.AreEqual(1, source.Valeurs.Count);

            Assert.AreEqual(fakeBudgetT4.MontantT4, source.Valeurs.Single()["MontantBudget"]);

            //Le PU (et la quantité) est calculé comme étant la moyenne des PU de tous les sous détails du T4
            //Dans notre cas, on a qu'un seul SD
            Assert.AreEqual(fakeBudgetT4.BudgetSousDetails.First().PU, source.Valeurs.Single()["PuBudget"]);

            var quantite = source.Valeurs.Single()["QuantiteBudget"] as RessourceUnite;
            Assert.AreEqual(fakeBudgetT4.BudgetSousDetails.First().Quantite, quantite.Quantite);
            Assert.AreEqual(fakeBudgetT4.BudgetSousDetails.First().Unite.Code, quantite.Unite);

            Assert.AreEqual(0, source.Valeurs.Single()["MontantAjustement"]);
            //Le PFA Mois précédent est particulier, car s'il n'existe pas alors rien ne doit être affiché (pas même 0)
            Assert.IsNull(source.Valeurs.Single()["PfaMoisPrecedent"]);
        }
    }
}
