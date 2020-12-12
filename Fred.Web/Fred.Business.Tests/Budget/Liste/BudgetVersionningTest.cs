using System.Collections.Generic;
using Fred.Business.Budget;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;

namespace Fred.Business.Tests.Budget
{
    [TestClass]
    public class BudgetVersionningTest
    {
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<IUtilisateurManager> utilisateurManager = new Mock<IUtilisateurManager>();
        private readonly Mock<IBudgetEtatManager> budgetEtatManager = new Mock<IBudgetEtatManager>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
        private readonly Mock<ITacheRepository> tacheRepository = new Mock<ITacheRepository>();
        private BudgetMainManagerTest budgetMainManagerTest;


        [TestInitialize]
        public void Init()
        {
            budgetManager.Setup(bm => bm.GetBudgetEnApplication(It.IsAny<int>())).Returns<BudgetEnt>(null);
            utilisateurManager.Setup(um => um.GetContextUtilisateurId()).Returns(0);

            budgetEtatManager.Setup(um => um.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeBudgetEtatByCode(code));

            budgetManager.Setup(bm => bm.BudgetChangeEtat(It.IsAny<BudgetEnt>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<BudgetEnt, int, int, string>(
                (budget, etatCibleId, utilisateurId, commentaire) =>
                {
                    budget.BudgetEtatId = etatCibleId;
                });

            budgetMainManagerTest = new BudgetMainManagerTest(
                null,
                null,
                null,
                null,
                budgetManager.Object,
                budgetEtatManager.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                utilisateurManager.Object,
                null,
                null,
                null,
                null,
                null,
                uow.Object,
                tacheRepository.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        [TestMethod]
        [Ignore]
        public void BudgetBrouillonToAValider()
        {
            var fakeEtatBrouillon = GetFakeBudgetEtatBrouillon();
            var budgetBrouillon = new BudgetEnt()
            {
                BudgetEtatId = fakeEtatBrouillon.BudgetEtatId,
                BudgetEtat = fakeEtatBrouillon,
                Version = "1.5",
                CiId = 458,
                BudgetT4s = new List<BudgetT4Ent>()
                {
                    GetFakeBudgetT4()
                }
            };
            budgetManager.Setup(bm => bm.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetBrouillon);

            var fakeEtatAValider = GetFakeBudgetEtatAValider();
            budgetMainManagerTest.ValidateDetailBudget(0);
            Assert.AreEqual("1.5", budgetBrouillon.Version);
            Assert.AreEqual(fakeEtatAValider.BudgetEtatId, budgetBrouillon.BudgetEtatId);

        }

        [TestMethod]
        [Ignore]
        public void BudgetAValiderVersEnApplication()
        {
            var fakeEtatAValider = GetFakeBudgetEtatAValider();
            var budgetAValider = new BudgetEnt()
            {
                BudgetEtatId = fakeEtatAValider.BudgetEtatId,
                BudgetEtat = fakeEtatAValider,
                Version = "1.5",
                CiId = 458,
                BudgetT4s = new List<BudgetT4Ent>()
                {
                    GetFakeBudgetT4()
                }
            };

            budgetManager.Setup(bm => bm.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetAValider);

            var fakeEtatEnApplication = GetFakeBudgetEtatEnApplication();
            budgetMainManagerTest.ValidateDetailBudget(0);
            Assert.AreEqual("2.0", budgetAValider.Version);
            Assert.AreEqual(fakeEtatEnApplication.BudgetEtatId, budgetAValider.BudgetEtatId);
        }

        [TestMethod]
        [Ignore]
        public void RetourBrouillon()
        {
            var fakeEtatAValider = GetFakeBudgetEtatAValider();

            var budgetAValider = new BudgetEnt()
            {
                BudgetEtatId = fakeEtatAValider.BudgetEtatId,
                BudgetEtat = fakeEtatAValider,
                Version = "1.5",
                CiId = 458,
                BudgetT4s = new List<BudgetT4Ent>()
                {
                    GetFakeBudgetT4()
                }
            };

            budgetManager.Setup(bm => bm.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetAValider);

            var fakeEtatBrouillon = GetFakeBudgetEtatBrouillon();
            budgetMainManagerTest.RetourBrouillon(0);
            Assert.AreEqual("1.5", budgetAValider.Version);
            Assert.AreEqual(fakeEtatBrouillon.BudgetEtatId, budgetAValider.BudgetEtatId);
        }

        [TestMethod]
        [Ignore]
        public void RetourBrouillonBudgetEnApplication()
        {
            var fakeBudgetEtatEnApplication = GetFakeBudgetEtatEnApplication();

            //Seul un budget a l'état a valider peut être remis à l'état brouillon
            var budgetEnApplication = new BudgetEnt()
            {
                BudgetEtatId = fakeBudgetEtatEnApplication.BudgetEtatId,
                BudgetEtat = fakeBudgetEtatEnApplication,
                Version = "2.0",
                CiId = 458,
                BudgetT4s = new List<BudgetT4Ent>()
                {
                    GetFakeBudgetT4()
                }
            };

            budgetManager.Setup(bm => bm.GetBudget(It.IsAny<int>(), It.IsAny<bool>())).Returns(() => budgetEnApplication);

            //Rien ne doit changer ici puisqu'un budget en application ne peut pas revenir à l'état brouillon
            var fakeEtatEnApplication = GetFakeBudgetEtatEnApplication();
            budgetMainManagerTest.RetourBrouillon(0);
            Assert.AreEqual("2.0", budgetEnApplication.Version);
            Assert.AreEqual(fakeEtatEnApplication.BudgetEtatId, budgetEnApplication.BudgetEtatId);
        }
    }
}
