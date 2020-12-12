using Fred.Business.Budget.Avancement;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.AvancementEtat;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;

namespace Fred.Business.Tests.Budget.Avancement
{
    [TestClass]
    public class AddOrUpdateAvancementTest
    {
        private readonly Mock<IAvancementEtatManager> avancementEtatManager = new Mock<IAvancementEtatManager>();
        private readonly Mock<IAvancementTacheManager> avancementTacheManager = new Mock<IAvancementTacheManager>();
        private readonly Mock<IAvancementWorkflowManager> avancementWorkflowManager = new Mock<IAvancementWorkflowManager>();
        private readonly Mock<IAvancementRepository> avancementRepository = new Mock<IAvancementRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
        private BudgetSousDetailEnt budgetSousDetail;
        private AvancementEnt insertedOrUpdatedAvancement;
        private AvancementEnt expectedAvancement;

        [TestInitialize]
        public void Init()
        {
            avancementEtatManager.Setup(aem => aem.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeAvancementEtatByCode(code));

            avancementRepository.Setup(ar => ar.Insert(It.IsAny<AvancementEnt>()))
                .Callback<AvancementEnt>(av => insertedOrUpdatedAvancement = av);

            avancementRepository.Setup(ar => ar.Update(It.IsAny<AvancementEnt>()))
                .Callback<AvancementEnt>(av => insertedOrUpdatedAvancement = av);

            var fakeRessource = GetFakeRessource();
            var fakeUnite = GetFakeUnite();

            budgetSousDetail = new BudgetSousDetailEnt()
            {
                Quantite = 10,
                PU = 10,
                Montant = 100,
                Ressource = fakeRessource,
                RessourceId = fakeRessource.RessourceId,
                Unite = fakeUnite,
                UniteId = fakeUnite.UniteId,
                BudgetSousDetailId = 1
            };

            var fakeCi = GetFakeCi();
            var fakeAvancementEtatEnregistre = GetFakeAvancementEtatEnregistre();
            expectedAvancement = new AvancementEnt
            {
                AvancementEtatId = fakeAvancementEtatEnregistre.AvancementEtatId,
                BudgetSousDetailId = budgetSousDetail.BudgetSousDetailId,
                PourcentageSousDetailAvance = 50,
                QuantiteSousDetailAvancee = 5,
                DAD = 50,
                Periode = 201811,
                CiId = fakeCi.CiId,
                DeviseId = 48
            };
        }


        [TestMethod]
        public void AddAvancementPourcentageTest()
        {
            var t4AvancementSaveModel = new AvancementTache4SaveModel
            {
                AvancementPourcent = expectedAvancement.PourcentageSousDetailAvance,
                DAD = expectedAvancement.DAD,
                TypeAvancement = (int)TypeAvancementBudget.Pourcentage
            };


            var avancementManager = new AvancementManager(uow.Object, avancementRepository.Object, avancementEtatManager.Object, avancementWorkflowManager.Object, avancementTacheManager.Object);
            avancementManager.AddOrUpdateAvancement(budgetSousDetail, t4AvancementSaveModel, expectedAvancement.Periode, expectedAvancement.CiId, expectedAvancement.DeviseId, (int)TypeAvancementBudget.Pourcentage, 1);

            Assert.AreEqual(expectedAvancement.PourcentageSousDetailAvance, insertedOrUpdatedAvancement.PourcentageSousDetailAvance);
            Assert.IsNull(insertedOrUpdatedAvancement.QuantiteSousDetailAvancee);
            Assert.AreEqual(expectedAvancement.DAD, insertedOrUpdatedAvancement.DAD);
            Assert.AreEqual(expectedAvancement.Periode, insertedOrUpdatedAvancement.Periode);
            Assert.AreEqual(expectedAvancement.AvancementEtatId, insertedOrUpdatedAvancement.AvancementEtatId);
            Assert.AreEqual(expectedAvancement.CiId, insertedOrUpdatedAvancement.CiId);
            Assert.AreEqual(expectedAvancement.DeviseId, insertedOrUpdatedAvancement.DeviseId);
            Assert.AreEqual(expectedAvancement.BudgetSousDetailId, insertedOrUpdatedAvancement.BudgetSousDetailId);
        }

        [TestMethod]
        public void UpdateAvancementPourcentageTest()
        {
            var t4AvancementSaveModel = new AvancementTache4SaveModel
            {
                AvancementPourcent = expectedAvancement.PourcentageSousDetailAvance,
                DAD = expectedAvancement.DAD,
                TypeAvancement = (int)TypeAvancementBudget.Pourcentage
            };


            var fakeAvancementEtatEnregistre = GetFakeAvancementEtatEnregistre();
            var avancementToUpdate = new AvancementEnt
            {
                AvancementEtatId = fakeAvancementEtatEnregistre.AvancementEtatId,
                BudgetSousDetailId = budgetSousDetail.BudgetSousDetailId,
                BudgetSousDetail = budgetSousDetail,
                PourcentageSousDetailAvance = 0,
                DAD = 0,
                Periode = 201811
            };

            avancementRepository.Setup(ar => ar.GetAvancement(budgetSousDetail.BudgetSousDetailId, expectedAvancement.Periode))
                .Returns(avancementToUpdate);

            var avancementManager = new AvancementManager(uow.Object, avancementRepository.Object, avancementEtatManager.Object, avancementWorkflowManager.Object, avancementTacheManager.Object);

            //Le CiiD n'a pas d'influence dans l'update d'un avancement
            avancementManager.AddOrUpdateAvancement(avancementToUpdate.BudgetSousDetail, t4AvancementSaveModel, avancementToUpdate.Periode, 1, 1, (int)TypeAvancementBudget.Pourcentage, 1);

            Assert.AreEqual(expectedAvancement.PourcentageSousDetailAvance, insertedOrUpdatedAvancement.PourcentageSousDetailAvance);
            Assert.IsNull(insertedOrUpdatedAvancement.QuantiteSousDetailAvancee);
            Assert.AreEqual(expectedAvancement.DAD, insertedOrUpdatedAvancement.DAD);
        }


        [TestMethod]
        public void AddAvancementQuantiteTest()
        {

            var t4AvancementSaveModel = new AvancementTache4SaveModel
            {
                AvancementQte = expectedAvancement.QuantiteSousDetailAvancee,
                Quantite = budgetSousDetail.Quantite.Value,
                DAD = expectedAvancement.DAD,
                TypeAvancement = (int)TypeAvancementBudget.Quantite

            };

            var avancementManager = new AvancementManager(uow.Object, avancementRepository.Object, avancementEtatManager.Object, avancementWorkflowManager.Object, avancementTacheManager.Object);
            avancementManager.AddOrUpdateAvancement(budgetSousDetail,
                t4AvancementSaveModel,
                expectedAvancement.Periode,
                expectedAvancement.CiId,
                expectedAvancement.DeviseId,
                (int)TypeAvancementBudget.Quantite,
                userId: 1);

            Assert.AreEqual(expectedAvancement.QuantiteSousDetailAvancee, insertedOrUpdatedAvancement.QuantiteSousDetailAvancee);
            Assert.IsNull(insertedOrUpdatedAvancement.PourcentageSousDetailAvance);
            Assert.AreEqual(expectedAvancement.DAD, insertedOrUpdatedAvancement.DAD);
            Assert.AreEqual(expectedAvancement.Periode, insertedOrUpdatedAvancement.Periode);
            Assert.AreEqual(expectedAvancement.AvancementEtatId, insertedOrUpdatedAvancement.AvancementEtatId);
            Assert.AreEqual(expectedAvancement.CiId, insertedOrUpdatedAvancement.CiId);
            Assert.AreEqual(expectedAvancement.DeviseId, insertedOrUpdatedAvancement.DeviseId);
            Assert.AreEqual(expectedAvancement.BudgetSousDetailId, insertedOrUpdatedAvancement.BudgetSousDetailId);
        }

        [TestMethod]
        public void UpdateAvancementQuantiteTest()
        {

            var t4AvancementSaveModel = new AvancementTache4SaveModel
            {
                AvancementQte = expectedAvancement.QuantiteSousDetailAvancee,
                Quantite = budgetSousDetail.Quantite.Value,
                DAD = expectedAvancement.DAD,
                TypeAvancement = (int)TypeAvancementBudget.Quantite

            };

            var fakeAvancementEtatEnregistre = GetFakeAvancementEtatEnregistre();
            var avancementToUpdate = new AvancementEnt
            {
                AvancementEtatId = fakeAvancementEtatEnregistre.AvancementEtatId,
                BudgetSousDetailId = budgetSousDetail.BudgetSousDetailId,
                BudgetSousDetail = budgetSousDetail,
                QuantiteSousDetailAvancee = 0,
                DAD = 0,
                Periode = 201811
            };

            avancementRepository.Setup(ar => ar.GetAvancement(budgetSousDetail.BudgetSousDetailId, avancementToUpdate.Periode))
                .Returns(avancementToUpdate);

            var avancementManager = new AvancementManager(uow.Object, avancementRepository.Object, avancementEtatManager.Object, avancementWorkflowManager.Object, avancementTacheManager.Object);
            avancementManager.AddOrUpdateAvancement(avancementToUpdate.BudgetSousDetail, t4AvancementSaveModel, avancementToUpdate.Periode, 1, 1, (int)TypeAvancementBudget.Quantite, 1);

            Assert.AreEqual(expectedAvancement.QuantiteSousDetailAvancee, insertedOrUpdatedAvancement.QuantiteSousDetailAvancee);
            Assert.IsNull(insertedOrUpdatedAvancement.PourcentageSousDetailAvance);
            Assert.AreEqual(expectedAvancement.DAD, insertedOrUpdatedAvancement.DAD);
        }
    }
}
