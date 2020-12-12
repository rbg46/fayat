using System.Collections.Generic;
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.AvancementEtat;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;

namespace Fred.Business.Tests.Budget.Avancement
{
    [TestClass]
    public class AvancementVersionningTest
    {
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IAvancementEtatManager> avancementEtatManager = new Mock<IAvancementEtatManager>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IAvancementWorkflowManager> avancementWorkflowManager = new Mock<IAvancementWorkflowManager>();
        private readonly Mock<IUtilisateurManager> utilisateurManager = new Mock<IUtilisateurManager>();
        private readonly Mock<ITacheRepository> tacheRepository = new Mock<ITacheRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        private AvancementSaveModel avancementSaveModelExpectedPeriode;
        private BudgetMainManagerTest budgetMainManagerTest;
        private const int expectedPeriode = 201801;

        [TestInitialize]
        public void Init()
        {
            avancementEtatManager.Setup(aem => aem.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeAvancementEtatByCode(code));

            var fakeBudgetT4 = GetFakeBudgetT4();


            budgetT4Manager.Setup(t4m => t4m.GetByIdWithSousDetailAndAvancement(fakeBudgetT4.BudgetId))
                .Returns(fakeBudgetT4);

            avancementSaveModelExpectedPeriode = new AvancementSaveModel
            {
                CiId = 1,
                DeviseId = 1,
                Periode = expectedPeriode,
                ListAvancementT4 = new List<AvancementTache4SaveModel>()
                {
                    new AvancementTache4SaveModel
                    {
                        AvancementPourcent = 50,
                        DAD = 50,
                        BudgetT4Id = fakeBudgetT4.BudgetT4Id,
                        TypeAvancement = (int) TypeAvancementBudget.Pourcentage
                    }
                }
            };

            budgetMainManagerTest = new BudgetMainManagerTest(
                avancementManager.Object,
                null,
                avancementEtatManager.Object,
                avancementWorkflowManager.Object,
                null,
                null,
                null,
                budgetT4Manager.Object,
                null,
                null,
                null, null,
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
        public void EnregistreToAValiderAvancementTest()
        {

            var fakeAvancementEnregistre = GetFakeAvancementEtatEnregistre();

            var avancement = new AvancementEnt
            {
                Periode = expectedPeriode,
                AvancementEtat = fakeAvancementEnregistre,
                AvancementEtatId = fakeAvancementEnregistre.AvancementEtatId
            };

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), expectedPeriode))
                .Returns(avancement);


            budgetMainManagerTest.ValidateAvancementModel(avancementSaveModelExpectedPeriode, avancement.AvancementEtat.Code);

            var fakeAvancementAValider = GetFakeAvancementAValider();
            Assert.AreEqual(fakeAvancementAValider.AvancementEtatId, avancement.AvancementEtatId);
        }

        [TestMethod]
        [Ignore]
        public void AValiderToValideTest()
        {
            var fakeAvancementAValider = GetFakeAvancementAValider();

            var avancement = new AvancementEnt
            {
                Periode = expectedPeriode,
                AvancementEtat = fakeAvancementAValider,
                AvancementEtatId = fakeAvancementAValider.AvancementEtatId
            };

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), expectedPeriode))
                .Returns(avancement);

            budgetMainManagerTest.ValidateAvancementModel(avancementSaveModelExpectedPeriode, avancement.AvancementEtat.Code);

            var fakeEtatValide = GetFakeAvancementEtatValide();
            Assert.AreEqual(fakeEtatValide.AvancementEtatId, avancement.AvancementEtatId);
        }

        [TestMethod]
        [Ignore]
        public void RetourBrouillonAvancementTest()
        {
            var fakeAvancementAValider = GetFakeAvancementAValider();

            var avancement = new AvancementEnt
            {
                Periode = expectedPeriode,
                AvancementEtat = fakeAvancementAValider,
                AvancementEtatId = fakeAvancementAValider.AvancementEtatId
            };

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), expectedPeriode))
                .Returns(avancement);

            budgetMainManagerTest.RetourBrouillonAvancement(avancementSaveModelExpectedPeriode);

            var fakeEtatEnregistre = GetFakeAvancementEtatEnregistre();
            Assert.AreEqual(fakeEtatEnregistre.AvancementEtatId, avancement.AvancementEtatId);
        }

        [TestMethod]
        [Ignore]
        public void RetourBrouillonAvancementFailTest()
        {
            var fakeAvancementValide = GetFakeAvancementEtatValide();

            var avancement = new AvancementEnt
            {
                Periode = expectedPeriode,
                AvancementEtat = fakeAvancementValide,
                AvancementEtatId = fakeAvancementValide.AvancementEtatId
            };

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), expectedPeriode))
                .Returns(avancement);

            budgetMainManagerTest.RetourBrouillonAvancement(avancementSaveModelExpectedPeriode);

            //L'etat de l'avancement ne doit pas changer si l'avancement que l'on souhaite mettre à jour n'est pas à l'état A Valider
            Assert.AreEqual(fakeAvancementValide.AvancementEtatId, avancement.AvancementEtatId);
        }
    }
}
