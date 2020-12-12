using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Budget;
using Fred.Business.Budget.BudgetManager;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Budget.Liste;
using Fred.Web.Shared.Models.Budget.Liste;
using Fred.Web.Shared.Models.Budget.Recette;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Budget
{
    [TestClass]
    public class BudgetManagerTest
    {
        private BudgetEnt budgetAttendu;
        private BudgetEnt budgetSupprime;
        private BudgetEtatEnt budgetEtatAttendu;
        private BudgetManager budgetManager;
        private readonly Mock<IBudgetWorkflowManager> budgetWorkflowManager = new Mock<IBudgetWorkflowManager>();
        private readonly Mock<IBudgetEtatManager> budgetEtatManager = new Mock<IBudgetEtatManager>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IUtilisateurManager> utilisateurManager = new Mock<IUtilisateurManager>();
        private readonly Mock<IBudgetRepository> budgetRepository = new Mock<IBudgetRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        [TestInitialize]
        public void Init()
        {
            budgetAttendu = new BudgetEnt() { BudgetEtatId = 1, DateDeleteNotificationNewTask = null };

            budgetSupprime = new BudgetEnt() { BudgetEtatId = 1, DateSuppressionBudget = DateTime.Today };

            budgetEtatAttendu = new BudgetEtatEnt() { BudgetEtatId = 1 };

            budgetRepository.Setup(br => br.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetAttendu);
            budgetRepository.Setup(br => br.FindById(It.IsAny<object>())).Returns(() => budgetAttendu);
            budgetEtatManager.Setup(br => br.GetByCode(It.IsAny<string>())).Returns(budgetEtatAttendu);

            budgetManager = new BudgetManager(
                uow.Object,
                budgetRepository.Object,
                budgetEtatManager.Object,
                budgetT4Manager.Object,
                utilisateurManager.Object,
                budgetWorkflowManager.Object
            );
        }

        [TestMethod]
        public void SupprimeBudget_WithExistingId_GivesBudgetSuppressionSuccessModel()
        {
            var response = budgetManager.SupprimeBudget(34);
            response.DateSuppression.Should().BeCloseTo(DateTime.Today);
        }

        [TestMethod]
        public void SupprimeBudget_WithBudgetNull_ThrowsFredBusinessMessageResponseException()
        {
            budgetRepository.Setup(br => br.FindById(It.IsAny<object>())).Returns(new BudgetEnt());
            budgetManager.Invoking(bm => bm.SupprimeBudget(0)).Should().Throw<FredBusinessMessageResponseException>();
        }

        [TestMethod]
        public void PartageOuPrivatiseBudgetTest()
        {
            var partageNouvelleValeur = budgetManager.PartageOuPrivatiseBudget(0);
            Assert.IsTrue(partageNouvelleValeur);

            partageNouvelleValeur = budgetManager.PartageOuPrivatiseBudget(0);
            Assert.IsFalse(partageNouvelleValeur);
        }

        [TestMethod]
        public void RestaurerBudget_ReturnsTrue()
        {
            Assert.IsNotNull(budgetSupprime.DateSuppressionBudget);
            budgetRepository.Setup(br => br.FindById(It.IsAny<object>())).Returns(() => budgetSupprime);

            var budgetRestaure = budgetManager.RestaurerBudget(0);
            Assert.IsNull(budgetSupprime.DateSuppressionBudget);
            Assert.IsTrue(budgetRestaure);
        }

        [TestMethod]
        public void BudgetChangeEtat_ReturnsUpdatedBudget()
        {
            Assert.AreEqual(budgetAttendu.BudgetEtatId, 1);
            budgetManager.BudgetChangeEtat(budgetAttendu, 2, 1, "test");
            Assert.AreEqual(budgetAttendu.BudgetEtatId, 2);
        }

        [TestMethod]
        public void GetBudgetRevisions_WithUserNull_ThrowsException()
        {
            utilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(() => null);
            budgetManager.Invoking(bm => bm.GetBudgetRevisions(0)).Should().Throw<NullReferenceException>();
        }

        [TestMethod]
        public void UpdateDateDeleteNotificationNewTask_WithBudgetNotNull_ReturnsBudgetNotNull()
        {
            budgetRepository.Setup(u => u.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetAttendu);
            Assert.IsNull(budgetAttendu.DateDeleteNotificationNewTask);
            budgetManager.UpdateDateDeleteNotificationNewTask(0);
            Assert.IsNotNull(budgetAttendu.DateDeleteNotificationNewTask);
        }

        [TestMethod]
        public void UpdateDateDeleteNotificationNewTask_WithBudgetNull_ReturnsBudgetNull()
        {
            budgetRepository.Setup(u => u.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => null);
            Assert.IsNull(budgetAttendu.DateDeleteNotificationNewTask);
            budgetManager.UpdateDateDeleteNotificationNewTask(0);
            Assert.IsNull(budgetAttendu.DateDeleteNotificationNewTask);
        }

        [TestMethod]
        public void GetDateMiseEnApplicationBudgetSurCi_WithCiIdtNull_ThrowsException()
        {
            budgetRepository.Setup(u => u.GetBudgetEnApplication(It.IsAny<int>())).Returns(() => null);
            budgetManager.Invoking(bm => bm.GetDateMiseEnApplicationBudgetSurCi(0)).Should()
                .Throw<NullReferenceException>();
        }

        [TestMethod]
        public void GetDateMiseEnApplicationBudgetSurCi_WithWorkflowAssociated_ReturnsDateNotNull()
        {
            budgetAttendu.Workflows =
                new List<BudgetWorkflowEnt>() { new BudgetWorkflowEnt() { Date = DateTime.Today, EtatCibleId = 1 } };
            budgetManager.PerformEagerLoading(budgetAttendu);
            budgetRepository.Setup(u => u.GetBudgetEnApplication(It.IsAny<int>())).Returns(() => budgetAttendu);
            budgetEtatManager.Setup(u => u.GetByCode(It.IsAny<string>())).Returns(() => budgetEtatAttendu);
            var result = budgetManager.GetDateMiseEnApplicationBudgetSurCi(0);
            result.Should().BeCloseTo(DateTime.Today);
        }

        [TestMethod]
        public void GetDateMiseEnApplicationBudgetSurCi_WithAnyWorkflowAssociated_ReturnsDateNull()
        {
            budgetAttendu.Workflows =
                new List<BudgetWorkflowEnt>() { new BudgetWorkflowEnt() { Date = DateTime.Now, EtatCibleId = 2 } };
            budgetManager.PerformEagerLoading(budgetAttendu);
            budgetRepository.Setup(u => u.GetBudgetEnApplication(It.IsAny<int>())).Returns(() => budgetAttendu);
            budgetEtatManager.Setup(u => u.GetByCode(It.IsAny<string>())).Returns(() => budgetEtatAttendu);
            var result = budgetManager.GetDateMiseEnApplicationBudgetSurCi(0);
            result.Should().BeNull();
        }

        [TestMethod]
        public void SaveBudgetChangeInListView_WithListeBudgetModelNull_ThrowsNullException()
        {
            budgetManager.Invoking(bm => bm.SaveBudgetChangeInListView(null)).Should().Throw<NullReferenceException>();
        }

        [TestMethod]
        public void SaveBudgetChangeInListView_WithListeBudgetModelNotNull_ReturnsUpdatedEntity()
        {
            var listeBudget = new ListeBudgetModel()
            {
                Recettes = new BudgetRecetteModel()
                {
                    AutresRecettes = 10,
                    MontantAvenants = 11,
                    MontantMarche = 12,
                    PenalitesEtRetenues = 13,
                    Revision = 14,
                    SommeAValoir = 15,
                    TravauxSupplementaires = 16,
                },
                Commentaire = "commentaire"
            };

            var budgetWithWorkflows = budgetAttendu;
            budgetWithWorkflows.Workflows = new List<BudgetWorkflowEnt>() { new BudgetWorkflowEnt() };

            budgetRepository.Setup(u => u.GetBudget(It.IsAny<int>(), false)).Returns(() => budgetWithWorkflows);

            var result = budgetManager.SaveBudgetChangeInListView(listeBudget);

            result.Workflows.First().Commentaire.Should().BeEquivalentTo("commentaire");
            result.Recette.MontantAvenants.Should().Be(11);
            result.Recette.AutresRecettes.Should().Be(10);
            result.Recette.MontantMarche.Should().Be(12);
            result.Recette.PenalitesEtRetenues.Should().Be(13);
            result.Recette.Revision.Should().Be(14);
            result.Recette.SommeAValoir.Should().Be(15);
            result.Recette.TravauxSupplementaires.Should().Be(16
            );
        }

        [TestMethod]
        public void GetBudgetBrouillonDuBudgetEnApplication_WithMajeurVersionIsNull_ReturnsVersionMajeureIs0()
        {
            List<BudgetEnt> budgets = new List<BudgetEnt>()
            {
                new BudgetEnt()
                {
                    BudgetId = 0,
                    CiId = 0,
                    DeviseId = 0,
                    BudgetEtat = new BudgetEtatEnt() {Code = "BR"},
                    DateSuppressionBudget = null,
                    Version = "0",
                    Workflows = new List<BudgetWorkflowEnt>() { }
                },
            };
            IQueryable<BudgetEnt> query = budgets.AsQueryable<BudgetEnt>();
            budgetRepository.Setup(br => br.Get()).Returns(() => query);
            IEnumerable<BudgetVersionAuteurModel> result = budgetManager.GetBudgetBrouillonDuBudgetEnApplication(0, 0);
            result.ToList().FirstOrDefault().Version.Should().StartWith("0");
        }

        [TestMethod]
        public void GetBudgetBrouillonDuBudgetEnApplication_WithMajeurVersionIs1Dot2_ReturnsVersionMajeureIs1()
        {
            List<BudgetEnt> budgets = new List<BudgetEnt>()
            {
                new BudgetEnt()
                {
                    BudgetId = 0,
                    CiId = 0,
                    DeviseId = 0,
                    Version = "1.3",
                    BudgetEtat = new BudgetEtatEnt() {Code = "BR"},
                    DateSuppressionBudget = null,
                    Workflows = new List<BudgetWorkflowEnt>() { }
                },
                new BudgetEnt()
                {
                    BudgetId = 1,
                    CiId = 0,
                    DeviseId = 0,
                    BudgetEtat = new BudgetEtatEnt() {Code = "EA"},
                    DateSuppressionBudget = null,
                    Version = "1.0",
                    Workflows = new List<BudgetWorkflowEnt>() { }
                },
            };
            IQueryable<BudgetEnt> query = budgets.AsQueryable();
            budgetRepository.Setup(br => br.Get()).Returns(() => query);
            IEnumerable<BudgetVersionAuteurModel> result = budgetManager.GetBudgetBrouillonDuBudgetEnApplication(0, 0);
            result.ToList().FirstOrDefault().Version.Should().StartWith("1");
        }

        [TestMethod]
        public void GetBudgetsBrouillons_WithCiIdAndDeviseId_ReturnsBudgetVersionAuteurModelList()
        {
            List<BudgetEnt> budgets = new List<BudgetEnt>()
            {
                new BudgetEnt()
                {
                    BudgetId = 0,
                    CiId = 0,
                    DeviseId = 0,
                    Version = "1.3",
                    BudgetEtat = new BudgetEtatEnt() {Code = "BR"},
                    DateSuppressionBudget = null,
                    Workflows = new List<BudgetWorkflowEnt>()
                    {
                        new BudgetWorkflowEnt()
                        {
                            Auteur = new UtilisateurEnt()
                            {
                                Personnel = new PersonnelEnt() {Nom = "AuBoisDormant", Prenom = "Abel"}
                            }
                        }
                    }
                }
            };
            IQueryable<BudgetEnt> query = budgets.AsQueryable();
            budgetRepository.Setup(br => br.Get()).Returns(() => query);
            var result = budgetManager.GetBudgetsBrouillons(0, 0);
            var firstBudget = result.ToList().FirstOrDefault();

            firstBudget.Version.Should().StartWith("1.3");
            firstBudget.BudgetId.Should().Be(0);
            firstBudget.NomAuteur.Should().BeEquivalentTo("AuBoisDormant");
            firstBudget.PrenomAuteur.Should().BeEquivalentTo("Abel");

            Assert.IsInstanceOfType(result, typeof(IEnumerable<BudgetVersionAuteurModel>));
        }
    }
}
