using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Budget;
using Fred.Business.Budget.BudgetManager.Services;
using Fred.Business.Budget.Recette;
using Fred.Entities.Budget.Recette;
using Fred.Web.Shared.Models.Budget.Recette;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Budget.Services
{
    [TestClass]
    public class GeneralExpenseServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IBudgetManager> mockBudgetManager;
        private Mock<IAvancementRecetteLoader> mockAvancementRecetteLoader;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockBudgetManager = this.mockRepository.Create<IBudgetManager>();
            this.mockAvancementRecetteLoader = this.mockRepository.Create<IAvancementRecetteLoader>();
        }

        private GeneralExpenseService CreateService()
        {
            return new GeneralExpenseService(
                this.mockBudgetManager.Object,
                this.mockAvancementRecetteLoader.Object);
        }

        [TestMethod]
        public void GetGeneralExpenses_get_pourcentage_correctly()
        {
            // Arrange
            var service = this.CreateService();
            this.mockBudgetManager.Setup(x => x.GetIdBudgetEnApplication(It.IsAny<int>()))
                                  .Returns(1);

            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202006))
                             .Returns(GetAvancementRecetteLoadModel());
            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202005))
                            .Returns(GetAvancementRecetteLoadModelEmpty());

            // Act
            var result = service.GetGeneralExpenses(new List<int>() { 1 }, 202006);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Pourcentage.Should().Be(2.5M, because: "((50 / 100 * 70) + 140) / 70 = 2.5 => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");

        }

        [TestMethod]
        public void GetGeneralExpenses_get_budget_correctly()
        {
            // Arrange
            var service = this.CreateService();
            this.mockBudgetManager.Setup(x => x.GetIdBudgetEnApplication(It.IsAny<int>()))
                                  .Returns(1);

            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202006))
                             .Returns(GetAvancementRecetteLoadModel());
            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202005))
                        .Returns(GetAvancementRecetteLoadModelEmpty());

            // Act
            var result = service.GetGeneralExpenses(new List<int>() { 1 }, 202006);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Budget.Should().Be(175m, because: "((50 / 100 * 70) + 140) = 175 => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_recette_cumul_correctly()
        {
            // Arrange
            var service = this.CreateService();
            this.mockBudgetManager.Setup(x => x.GetIdBudgetEnApplication(It.IsAny<int>()))
                                  .Returns(1);

            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202006))
                             .Returns(GetAvancementRecetteLoadModelForRecetteCumul());
            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202005))
                        .Returns(GetAvancementRecetteLoadModelEmpty());

            // Act
            var result = service.GetGeneralExpenses(new List<int>() { 1 }, 202006);

            // Assert
            result.Should().NotBeEmpty();
            result.First().RecetteCumul.Should().Be(540m, because: "((25 / 100 * 160) + 500) = 540 => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_pfa_correctly()
        {
            // Arrange
            var service = this.CreateService();
            this.mockBudgetManager.Setup(x => x.GetIdBudgetEnApplication(It.IsAny<int>()))
                                  .Returns(1);

            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202006))
                             .Returns(GetAvancementRecetteLoadModelForFpa());
            this.mockAvancementRecetteLoader.Setup(x => x.Load(It.IsAny<int>(), 202005))
                        .Returns(GetAvancementRecetteLoadModelEmpty());

            // Act
            var result = service.GetGeneralExpenses(new List<int>() { 1 }, 202006);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Pfa.Should().Be(207m, because: "((10 / 100 * 70) + 200) = 207 => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_total_budget_correctly()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetTotalGeneralExpenses(GetGeneralExpenses());

            // Assert
            result.Should().NotBeNull();
            result.Budget.Should().Be(20.1m, because: "this is the sum of budgets => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_total_pourcentage_correctly()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetTotalGeneralExpenses(GetGeneralExpenses());

            // Assert
            result.Should().NotBeNull();
            result.Pourcentage.Should().Be(0.1005m, because: "this is the sum of budgets / the sum of prod  => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_total_recetteCumul_correctly()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetTotalGeneralExpenses(GetGeneralExpenses());

            // Assert
            result.Should().NotBeNull();
            result.RecetteCumul.Should().Be(50m, because: "this is the sum of RecetteCumul => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_total_recette_correctly()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetTotalGeneralExpenses(GetGeneralExpenses());

            // Assert
            result.Should().NotBeNull();
            result.Recette.Should().Be(10m, because: "this is the sum of RecetteCumul - sum of RecetteCumulPrevious => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }

        [TestMethod]
        public void GetGeneralExpenses_get_total_pfa_correctly()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetTotalGeneralExpenses(GetGeneralExpenses());

            // Assert
            result.Should().NotBeNull();
            result.Pfa.Should().Be(285m, because: "this is the sum of pfa  => US 14283  Calcul des frais généraux dans les éditions - RG_14283_002 ");
        }


        private List<GeneralExpense> GetGeneralExpenses()
        {
            return new List<GeneralExpense>()
            {
                new GeneralExpense()
                {
                    Prod = 100m,
                    Budget =10.1m,
                    RecetteCumul = 25,
                    RecetteCumulPrevious = 20,
                    Pfa = 200
                },
                  new GeneralExpense()
                {
                    Prod = 100m,
                    Budget =10,
                    RecetteCumul = 25,
                    RecetteCumulPrevious = 20,
                    Pfa = 85
                }
            };
        }

        private AvancementRecetteLoadModel GetAvancementRecetteLoadModel()
        {
            var budgetRecette = new BudgetRecetteEnt()
            {
                AutresRecettes = 10m,
                BudgetRecetteId = 1,
                MontantAvenants = 10m,
                MontantMarche = 10m,
                PenalitesEtRetenues = 10m,
                Revision = 10m,
                SommeAValoir = 10m,
                TravauxSupplementaires = 10m
            };
            var avancementRecette = new AvancementRecetteEnt
            {
                TauxFraisGeneraux = 50,
                AjustementFraisGeneraux = 140,
                AvancementTauxFraisGeneraux = 25,
                AvancementAjustementFraisGeneraux = 500,
                Correctif = 30
            };
            var avancementRecettePrevious = new AvancementRecetteEnt();
            var avancementRecetteModel = new AvancementRecetteLoadModel(budgetRecette, avancementRecette, avancementRecettePrevious);
            return avancementRecetteModel;
        }

        private AvancementRecetteLoadModel GetAvancementRecetteLoadModelEmpty()
        {
            var budgetRecette = new BudgetRecetteEnt();
            var avancementRecette = new AvancementRecetteEnt();
            var avancementRecettePrevious = new AvancementRecetteEnt();
            var avancementRecetteModel = new AvancementRecetteLoadModel(budgetRecette, avancementRecette, null);
            return avancementRecetteModel;
        }

        private AvancementRecetteLoadModel GetAvancementRecetteLoadModelForRecetteCumul()
        {
            var budgetRecette = new BudgetRecetteEnt();

            var avancementRecette = new AvancementRecetteEnt
            {
                AvancementTauxFraisGeneraux = 25,
                AvancementAjustementFraisGeneraux = 500,
                AutresRecettes = 20m,
                MontantAvenants = 20m,
                MontantMarche = 20m,
                PenalitesEtRetenues = 20m,
                Revision = 20m,
                SommeAValoir = 20m,
                TravauxSupplementaires = 20m,
                Correctif = 20m,
            };
            var avancementRecettePrevious = new AvancementRecetteEnt();
            var avancementRecetteModel = new AvancementRecetteLoadModel(budgetRecette, avancementRecette, avancementRecettePrevious);
            return avancementRecetteModel;
        }


        private AvancementRecetteLoadModel GetAvancementRecetteLoadModelForFpa()
        {
            var budgetRecette = new BudgetRecetteEnt();
            var avancementRecette = new AvancementRecetteEnt
            {
                TauxFraisGenerauxPFA = 10,
                AjustementFraisGenerauxPFA = 200,
                MontantMarchePFA = 10,
                MontantAvenantsPFA = 10,
                SommeAValoirPFA = 10,
                TravauxSupplementairesPFA = 10,
                RevisionPFA = 10,
                AutresRecettesPFA = 10,
                PenalitesEtRetenuesPFA = 10
            };
            var avancementRecettePrevious = new AvancementRecetteEnt();
            var avancementRecetteModel = new AvancementRecetteLoadModel(budgetRecette, avancementRecette, avancementRecettePrevious);
            return avancementRecetteModel;
        }
    }
}
