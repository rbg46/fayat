using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire;
using Fred.Business.DatesClotureComptable;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Entities.Constantes;

namespace Fred.Business.Tests.Budget.ControleBudgetaire
{
    [TestClass]
    public class ControleBudgetaireVersionningTest
    {
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
        private readonly Mock<IControleBudgetaireRepository> controleBudgetaireRepository = new Mock<IControleBudgetaireRepository>();
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<IDatesClotureComptableManager> datesClotureComptableManager = new Mock<IDatesClotureComptableManager>();
        private ControleBudgetaireManager controleBudgetaireManager;



        private const int PeriodeBudgetBrouillon = 201811;
        private const int PeriodeBudgetAValider = 201810;
        private const int PeriodeBudgetEnApplication = 201809;

        [TestInitialize]
        public void Init()
        {
            budgetManager.Setup(bm => bm.GetCiIdAssociatedToBudgetId(It.IsAny<int>()))
                .Returns(1);

            avancementManager.Setup(am => am.IsAvancementValide(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            datesClotureComptableManager.Setup(dccm => dccm.IsPeriodClosed(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null))
                .Returns(false);


            var fakeEtatBrouillon = GetFakeBudgetEtatBrouillon();
            var controleBudgetaireBrouillon = new ControleBudgetaireEnt()
            {
                ControleBudgetaireEtat = fakeEtatBrouillon,
                ControleBudgetaireId = fakeEtatBrouillon.BudgetEtatId
            };

            var fakeEtatAValider = GetFakeBudgetEtatAValider();
            var controleBudgetaireAValider = new ControleBudgetaireEnt()
            {
                ControleBudgetaireEtat = fakeEtatAValider,
                ControleBudgetaireId = fakeEtatAValider.BudgetEtatId
            };

            var fakeEtatEnApplication = GetFakeBudgetEtatEnApplication();
            var controleBudgetaireEnApplication = new ControleBudgetaireEnt()
            {
                ControleBudgetaireEtat = fakeEtatEnApplication,
                ControleBudgetaireId = fakeEtatEnApplication.BudgetEtatId
            };

            controleBudgetaireRepository.Setup(cbr => cbr.GetControleBudgetaireByBudgetId(It.IsAny<int>(), PeriodeBudgetBrouillon, false))
                .Returns(controleBudgetaireBrouillon);

            controleBudgetaireRepository.Setup(cbr => cbr.GetControleBudgetaireByBudgetId(It.IsAny<int>(), PeriodeBudgetAValider, false))
                .Returns(controleBudgetaireAValider);

            controleBudgetaireRepository.Setup(cbr => cbr.GetControleBudgetaireByBudgetId(It.IsAny<int>(), PeriodeBudgetEnApplication, false))
                .Returns(controleBudgetaireEnApplication);


            controleBudgetaireManager = new ControleBudgetaireManager(
                uow.Object,
                controleBudgetaireRepository.Object,
                budgetManager.Object,
                null,
                avancementManager.Object,
                null,
                null,
                null,
                datesClotureComptableManager.Object, null);
        }

        [TestMethod]
        public void PeutPasserAEtatTestVersAValider()
        {
            //Le passage à l'état A Valider ne devrait être possible qu'a partir d'un controle budgetaire à l'état brouillon
            var changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetBrouillon, EtatBudget.AValider);
            Assert.IsTrue(changementEtatModel.EtatPrecedentOkay);

            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetAValider, EtatBudget.AValider);
            Assert.IsFalse(changementEtatModel.EtatPrecedentOkay);

            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetEnApplication, EtatBudget.AValider);
            Assert.IsFalse(changementEtatModel.EtatPrecedentOkay);
        }

        [TestMethod]
        public void PeutPasserAEtatTestVersEnApplication()
        {
            //Le passage à l'état En Application ne devrait être possible qu'a partir d'un controle budgetaire à l'état A Valider
            var changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetAValider, EtatBudget.EnApplication);
            Assert.IsTrue(changementEtatModel.EtatPrecedentOkay);


            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetBrouillon, EtatBudget.EnApplication);
            Assert.IsFalse(changementEtatModel.EtatPrecedentOkay);

            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetEnApplication, EtatBudget.EnApplication);
            Assert.IsFalse(changementEtatModel.EtatPrecedentOkay);
        }

        [TestMethod]
        public void PeutPasserAEtatTestVersBrouillon()
        {
            //La dégradation à l'état brouillon n'est possible que pour un controle à l'état En Application ou brouillon
            var changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetAValider, EtatBudget.Brouillon);
            Assert.IsTrue(changementEtatModel.EtatPrecedentOkay);

            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetEnApplication, EtatBudget.Brouillon);
            Assert.IsTrue(changementEtatModel.EtatPrecedentOkay);

            changementEtatModel = controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetBrouillon, EtatBudget.Brouillon);
            Assert.IsFalse(changementEtatModel.EtatPrecedentOkay);
        }


        [TestMethod]
        [ExpectedException(typeof(FredBusinessMessageResponseException))]
        public void PeutPasserAEtatInconnu()
        {
            controleBudgetaireManager.PeutPasserAEtat(1, PeriodeBudgetBrouillon, "ETAT INCONNU");
        }
    }
}
