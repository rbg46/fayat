using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire;
using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Business.Budget.Helpers;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense.Services;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Societe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;

namespace Fred.Business.Tests.Budget.ControleBudgetaire
{
    [TestClass]
    public class GetControleBudgetaireSansDonneesTest
    {
        private ControleBudgetaireManager controleBudgetaireManager;
        private readonly Mock<IBudgetEtatManager> budgetEtatManager = new Mock<IBudgetEtatManager>();
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();
        private readonly Mock<IBudgetT4Manager> t4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IDepenseServiceMediator> depenseServiceMediator = new Mock<IDepenseServiceMediator>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IDatesClotureComptableManager> datesClotureComptableManager = new Mock<IDatesClotureComptableManager>();
        private readonly Mock<IControleBudgetaireRepository> controleBudgetaireRepository = new Mock<IControleBudgetaireRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        private BudgetEnt BudgetEnApplication = new BudgetEnt
        {
            PeriodeDebut = 201901
        };

        private SocieteEnt societe = new SocieteEnt
        {

        };

        private IEnumerable<BudgetT4Ent> budgetT4s = new List<BudgetT4Ent>()
        {

        };

        private const int ciIdBudgetEnApplication = 1;
        private const int ciIdSansBudgetEnApplication = 2;

        [TestInitialize]
        public void Init()
        {
            budgetEtatManager.Setup(um => um.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeBudgetEtatByCode(code));

            //L'important ici est de retourner une valeur autre que 0
            budgetManager.Setup(bm => bm.GetIdBudgetEnApplication(ciIdBudgetEnApplication)).Returns(1);
            budgetManager.Setup(bm => bm.GetIdBudgetEnApplication(ciIdSansBudgetEnApplication)).Returns(0);
            budgetManager.Setup(bm => bm.GetBudgetEnApplication(It.IsAny<int>())).Returns(BudgetEnApplication);
            budgetManager.Setup(bm => bm.GetDateMiseEnApplicationBudgetSurCi(It.IsAny<int>())).Returns(DateTime.Now);

            ciManager.Setup(cim => cim.GetSocieteByCIId(It.IsAny<int>(), false)).Returns(societe);
            t4Manager.Setup(t4m => t4m.GetByBudgetId(It.IsAny<int>(), It.IsAny<bool>())).Returns(budgetT4s);

            controleBudgetaireManager = new ControleBudgetaireManager(
                uow.Object,
                controleBudgetaireRepository.Object,
                budgetManager.Object,
                t4Manager.Object,
                avancementManager.Object,
                budgetEtatManager.Object,
                ciManager.Object,
                depenseServiceMediator.Object,
                datesClotureComptableManager.Object, null);
        }

        [TestMethod]
        public void GetControleBudgetaireAucunBudgetEnApplication()
        {
            budgetManager.Setup(bm => bm.GetBudgetEnApplication(It.IsAny<int>())).Returns(() => null);
            //Arrange
            var filtre = new ControleBudgetaireLoadModel()
            {
                AxePrincipal = AxePrincipal.TacheRessource,
                CiId = ciIdSansBudgetEnApplication,
                DeviseId = 48,
                PeriodeComptable = 201810,
                Cumul = true
            };

            //Action
            Task<ControleBudgetaireModel> taskControleBudgetaireModel = controleBudgetaireManager.GetControleBudgetaireAsync(filtre);
            taskControleBudgetaireModel.Exception.Message.Should().Contain("Aucun budget en application pour ce CI");
        }

        [TestMethod]
        public async Task GetControleBudgetaireBudgetSansDonneesAsync()
        {
            var filtre = new ControleBudgetaireLoadModel()
            {
                AxePrincipal = AxePrincipal.TacheRessource,
                CiId = ciIdBudgetEnApplication,
                DeviseId = 48,
                PeriodeComptable = 201811,
                Cumul = true
            };

            //NOTE : les assertions suivantes sont très dépendantes des données de tests décrites plus haut
            var tree = await controleBudgetaireManager.GetControleBudgetaireAsync(filtre).ConfigureAwait(false);
            Assert.AreEqual(0, tree.Tree.Count());
            Assert.IsFalse(tree.AvancementValide);
            Assert.IsFalse(tree.PeriodeCloturee);
            Assert.AreEqual(filtre.PeriodeComptable, tree.Periode);
            Assert.IsFalse(tree.Locked);
            var fakeEtatBrouillon = GetFakeBudgetEtatBrouillon();
            Assert.AreEqual(fakeEtatBrouillon.Code, tree.CodeEtat);
        }

        [TestMethod]
        public async Task GetControleBudgetaireSansDonneesAvancementEtPeriodeClotureAsync()
        {
            avancementManager.Setup(am => am.IsAvancementValide(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            datesClotureComptableManager.Setup(dccm => dccm.IsPeriodClosed(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null))
                .Returns(true);


            var filtre = new ControleBudgetaireLoadModel()
            {
                AxePrincipal = AxePrincipal.TacheRessource,
                CiId = ciIdBudgetEnApplication,
                DeviseId = 48,
                PeriodeComptable = 201811,
                Cumul = true
            };

            //NOTE : les assertions suivantes sont très dépendantes des données de tests décrites plus haut
            var tree = await controleBudgetaireManager.GetControleBudgetaireAsync(filtre).ConfigureAwait(false);
            Assert.IsTrue(tree.AvancementValide);
            Assert.IsTrue(tree.PeriodeCloturee);
        }



    }
}
