using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget;
using Fred.Business.Budget.BudgetManager;
using Fred.Common.Tests;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;

namespace Fred.Business.Tests.Budget.Liste
{
    [TestClass]
    public class CopieBudgetTest : BaseTu<BudgetCopieManager>
    {
        private readonly BudgetEnt fakeBudgetEnApplication = GetFakeBudgetEnApplication();
        private BudgetEnt budgetInsere = null;
        private int newBudgetId;
        private const int utilisateurconnecteId = 0;

        [TestInitialize]
        public void Init()
        {
            //Il est normalement déjà garanti que la version doit avoir cette valeur mais dans le doute on la respécifie pour garantir le futur Assert sur la version
            fakeBudgetEnApplication.Version = "1.0";
            fakeBudgetEnApplication.BudgetT4s = new List<BudgetT4Ent>()
            {
                GetFakeBudgetT4()
            };
            var budgetT4Manager = GetMocked<IBudgetT4Manager>();
            budgetT4Manager.Setup(bt4m => bt4m.GetByBudgetId(fakeBudgetEnApplication.BudgetId, true))
                .Returns(fakeBudgetEnApplication.BudgetT4s);

            var budgetEtatManager = GetMocked<IBudgetEtatManager>();
            budgetEtatManager.Setup(bem => bem.GetByCode(It.IsAny<string>()))
                .Returns<string>(param => GetFakeBudgetEtatByCode(param));

            newBudgetId = fakeBudgetEnApplication.BudgetId + 1;

            var budgetRepository = GetMocked<IBudgetRepository>();
            budgetRepository.Setup(br => br.Insert(It.IsAny<BudgetEnt>()))
                .Returns<BudgetEnt>(b =>
               {
                   b.BudgetId = newBudgetId;
                   budgetInsere = b;
                   return b;
               });

            budgetRepository.Setup(br => br.GetBudget(fakeBudgetEnApplication.BudgetId, true))
                .Returns(fakeBudgetEnApplication);

            //Ici on ne retourne pas directement l'object, car il est null lors de l'appel à la fonction Setup
            //On retourne plutôt un "getter" de cet object qui sera initialisé par la fonction Insert mockée plus haut
            budgetRepository.Setup(br => br.GetBudget(newBudgetId, false))
                .Returns(() => budgetInsere);

            budgetRepository.Setup(br => br.GetBudgetMaxVersion(fakeBudgetEnApplication.BudgetId))
                .Returns("1.0");

            var uow = GetMocked<IUnitOfWork>();
            SubstituteConstructorArgument<IUnitOfWork>(uow.Object);
        }


        [TestMethod]
        public void CopieBudgetDansCiTest()
        {
            var budgetCopie = Actual.CopierBudgetDansMemeCiAsync(fakeBudgetEnApplication.BudgetId, utilisateurconnecteId, false).Result;

            CheckBudgetCopie(budgetCopie);
            CheckWorkflowCopie(budgetCopie);
            CheckT4Copie(budgetCopie);
            CheckSousDetailBudgetCopie(budgetCopie);
        }

        private void CheckBudgetCopie(BudgetEnt budgetCopie)
        {
            //Verification du budget
            var fakeEtatBrouillon = GetFakeBudgetEtatBrouillon();
            Assert.AreEqual(fakeEtatBrouillon.BudgetEtatId, budgetCopie.BudgetEtatId);
            Assert.AreEqual(1, budgetCopie.Workflows.Count);
            Assert.AreEqual(utilisateurconnecteId, budgetCopie.Workflows.Single().AuteurId);
            Assert.IsNull(budgetCopie.Workflows.Single().EtatInitialId);
            Assert.AreEqual(fakeEtatBrouillon.BudgetEtatId, budgetCopie.Workflows.Single().EtatCibleId);
        }

        private void CheckWorkflowCopie(BudgetEnt budgetCopie)
        {
            var today = DateTime.Today.Day;
            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;
            Assert.AreEqual(today, budgetCopie.Workflows.Single().Date.Day);
            Assert.AreEqual(month, budgetCopie.Workflows.Single().Date.Month);
            Assert.AreEqual(year, budgetCopie.Workflows.Single().Date.Year);
        }

        private void CheckT4Copie(BudgetEnt budgetCopie)
        {
            var budgetT4Origine = fakeBudgetEnApplication.BudgetT4s.Single();
            var budgetT4Copie = budgetCopie.BudgetT4s.First();
            Assert.AreEqual(1, budgetCopie.BudgetT4s.Count);
            Assert.AreEqual(budgetT4Origine.MontantT4, budgetT4Copie.MontantT4);
            Assert.AreEqual(budgetT4Origine.QuantiteDeBase, budgetT4Copie.QuantiteDeBase);
            Assert.AreEqual(budgetT4Origine.QuantiteARealiser, budgetT4Copie.QuantiteARealiser);
            Assert.AreEqual(budgetT4Origine.TypeAvancement, budgetT4Copie.TypeAvancement);
            Assert.AreEqual(budgetT4Origine.PU, budgetT4Copie.PU);
            Assert.AreEqual(budgetT4Origine.T4Id, budgetT4Copie.T4Id);
            Assert.AreEqual(budgetT4Origine.UniteId, budgetT4Copie.UniteId);


        }

        private void CheckSousDetailBudgetCopie(BudgetEnt budgetCopie)
        {
            var budgetSdOrigine = fakeBudgetEnApplication.BudgetT4s.Single().BudgetSousDetails.Single();
            var budgetSdCopie = budgetCopie.BudgetT4s.First().BudgetSousDetails.First();
            Assert.AreEqual(1, budgetCopie.BudgetT4s.Count);
            Assert.AreEqual(budgetSdOrigine.Montant, budgetSdCopie.Montant);
            Assert.AreEqual(budgetSdOrigine.PU, budgetSdCopie.PU);
            Assert.AreEqual(budgetSdOrigine.Quantite, budgetSdCopie.Quantite);
            Assert.AreEqual(budgetSdOrigine.RessourceId, budgetSdCopie.RessourceId);
            Assert.AreEqual(budgetSdOrigine.UniteId, budgetSdCopie.UniteId);
            Assert.AreEqual(budgetSdOrigine.Commentaire, budgetSdCopie.Commentaire);
        }

    }

}
