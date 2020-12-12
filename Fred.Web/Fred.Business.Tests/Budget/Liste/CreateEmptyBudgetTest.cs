using System.Linq;
using Fred.Business.Budget;
using Fred.Business.Budget.BudgetManager;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;

namespace Fred.Business.Tests.Budget
{
    [TestClass]
    public class CreateEmptyBudgetTest
    {
        private readonly Mock<IBudgetRepository> budgetRepository = new Mock<IBudgetRepository>();
        private readonly Mock<IBudgetEtatManager> budgetEtatManager = new Mock<IBudgetEtatManager>();
        private readonly Mock<IBudgetWorkflowManager> budgetWorkflowManager = new Mock<IBudgetWorkflowManager>();
        private readonly Mock<IUtilisateurManager> utilisateurManager = new Mock<IUtilisateurManager>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();

        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
        private readonly int ciId = 458;
        private readonly int utilisateurConnecteId = 1;
        private readonly BudgetEtatEnt budgetEtatBrouillon = GetFakeBudgetEtatBrouillon();

        private BudgetEnt budgetCree;


        [TestInitialize]
        public void Init()
        {
            //Retourne n'importe quel budget passé en paramètre
            budgetRepository.Setup(br => br.Insert(It.IsAny<BudgetEnt>())).Returns<BudgetEnt>(b =>
                {
                    budgetCree = b;
                    return b;
                }
            );

            budgetRepository.Setup(br => br.GetBudget(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(() => budgetCree);

            budgetRepository.Setup(br => br.GetCiMaxVersion(It.IsAny<int>())).Returns("0.9");
            budgetEtatManager.Setup(um => um.GetByCode(It.IsAny<string>()))
                .Returns<string>(code => GetFakeBudgetEtatByCode(code));
        }

        [TestMethod]
        public void CreateEmptyBudgetSurCiTest()
        {

            var budgetManager = new BudgetManager(
                uow.Object,
                budgetRepository.Object,
                budgetEtatManager.Object,
                budgetT4Manager.Object,
                utilisateurManager.Object,
                budgetWorkflowManager.Object
                );
            var nouveauBudget = budgetManager.CreateEmptyBudgetSurCi(ciId, utilisateurConnecteId);

            //La version d'un numéro de budget s'incrémente de cette manière : 0.1, 0.2...0.9,0.10
            Assert.AreEqual("0.10", nouveauBudget.Version);
            Assert.AreEqual(ciId, nouveauBudget.CiId);


            //Un budget nouvellement crée ne doit pas être partagé à tous les utilisateurs
            Assert.IsFalse(nouveauBudget.Partage);

            //Le "Single" utilisé plus tard pourrait effectuer ce test, mais pour plus de lisibilité dans les résultats on vérifie ici
            Assert.AreEqual(1, nouveauBudget.Workflows.Count);

            //Un workflow représente une étape dans l'historique d'un budget 
            //(par exemple une nouvelle ligne est crée lors du passage du budget à l'état à Validé)
            //L'auteurId de chaque ligne représente l'auteur à l'origine de la modification (par exemple le valideur)
            //Ici l'auteurId doit être l'utilisateur connecté ayant crée le budget puisque le budget est nouveau
            Assert.AreEqual(utilisateurConnecteId, nouveauBudget.Workflows.Single().AuteurId);

            Assert.AreEqual(budgetEtatBrouillon.BudgetEtatId, nouveauBudget.BudgetEtatId);

            Assert.IsNull(nouveauBudget.PeriodeDebut);
            Assert.IsNull(nouveauBudget.PeriodeFin);
        }


    }
}
