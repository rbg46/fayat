using Fred.Business.Budget.Avancement;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using static Fred.Business.Tests.ModelBuilder.AvancementEtat;

namespace Fred.Business.Tests.Budget.RecalageBudgetaire
{
    [TestClass]
    public class AvancementManagerRecalageTest
    {
        private readonly Mock<IAvancementEtatManager> avancementEtatManager = new Mock<IAvancementEtatManager>();
        private readonly Mock<IAvancementWorkflowManager> avancementWorkflowManager = new Mock<IAvancementWorkflowManager>();
        private readonly Mock<IAvancementTacheManager> avancementTacheManager = new Mock<IAvancementTacheManager>();
        private readonly Mock<IAvancementRepository> avancementRepository = new Mock<IAvancementRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
        private AvancementManager avancementManager;
        private const int expectedCiId = 100;
        private const int expectedPeriode = 201811;
        private const int expectedDeviseId = 48;
        private AvancementEnt avancementInsere;

        [TestInitialize]
        public void Init()
        {
            avancementRepository.Setup(ar => ar.Insert(It.IsAny<AvancementEnt>()))
                .Callback<AvancementEnt>(param => avancementInsere = param);

            avancementEtatManager.Setup(aem => aem.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeAvancementEtatByCode(code));

            avancementManager = new AvancementManager(uow.Object, avancementRepository.Object, avancementEtatManager.Object, avancementWorkflowManager.Object, avancementTacheManager.Object);
        }

        [TestMethod]
        public void CreationAvancementT4RevTest()
        {
            var fakeSosuDetail = new BudgetSousDetailEnt
            {
                BudgetSousDetailId = 1,
            };

            avancementManager.CreationAvancementT4Rev(fakeSosuDetail, expectedCiId, expectedDeviseId, expectedPeriode, 1);

            Assert.AreEqual(100, avancementInsere.PourcentageSousDetailAvance);
            Assert.AreEqual(0, avancementInsere.DAD);
            Assert.AreEqual(expectedCiId, avancementInsere.CiId);
            Assert.AreEqual(expectedDeviseId, avancementInsere.DeviseId);
            Assert.AreEqual(expectedPeriode, avancementInsere.Periode);
            Assert.AreEqual(fakeSosuDetail.BudgetSousDetailId, avancementInsere.BudgetSousDetailId);

            var fakeAvancementEtatEnregistre = GetFakeAvancementEtatEnregistre();
            Assert.AreEqual(fakeAvancementEtatEnregistre.AvancementEtatId, avancementInsere.AvancementEtatId);
        }
    }
}
