using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Budget;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fred.Business.Budget.Extensions;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using Fred.Entities;

namespace Fred.Business.Tests.Budget.ExtensionsTests
{
    [TestClass]
    public class BudgetT4ExtensionTest
    {
        [TestMethod]
        [Ignore]
        public void TestCheckBudgetAucunT4()
        {
            var budgetT4s = new List<BudgetT4Ent>();
            var erreur = budgetT4s.CheckBudgetT4OfBudget(isTypeAvancementDynamiqueSurSociete: false);
            Assert.IsTrue(erreur.Length == 0);
        }

        [TestMethod]
        public void TestCheckBudgetAllOkay()
        {
            var budgetT4s = new List<BudgetT4Ent>()
            {
                GetFakeBudgetT4()
            };

            var erreur = budgetT4s.CheckBudgetT4OfBudget(isTypeAvancementDynamiqueSurSociete: false);
            Assert.IsTrue(erreur.Length == 0);
        }

        [TestMethod]
        public void TestBudgetSansMontant()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.MontantT4 = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }


        [TestMethod]
        public void TestBudgetSansPu()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.PU = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSansQuantiteARealiser()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.QuantiteARealiser = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSansTypeAvancement()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.TypeAvancement = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSansUnite()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.UniteId = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSansBoudgetSousDetail()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.BudgetSousDetails = new List<BudgetSousDetailEnt>();
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSousDetailSansMontant()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.BudgetSousDetails.First().Montant = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void TestBudgetSousDetailSansUnite()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.BudgetSousDetails.First().UniteId = null;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }

        [TestMethod]
        public void AssertFakeBudgetT4HasNoErrorBecauseAvancementTypeIsDynamique()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.TypeAvancement = (int)TypeAvancementBudget.Aucun;

            var budgetT4s = new List<BudgetT4Ent>()
            {
                fakeBudgetT4
            };

            var erreur = budgetT4s.CheckBudgetT4OfBudget(isTypeAvancementDynamiqueSurSociete: true);
            Assert.IsTrue(erreur.Length == 0);
        }

        [TestMethod]
        public void AssertFakeBudgetT4HasErrorBecauseAvancementTypeIsDynamique()
        {
            var fakeBudgetT4 = GetFakeBudgetT4();
            fakeBudgetT4.TypeAvancement = (int)TypeAvancementBudget.Aucun;
            AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(fakeBudgetT4);
        }


        private void AssertFakeBudgetT4HasErrorWithAvancementTypeNoDynamique(BudgetT4Ent fakeBudgetT4)
        {
            var budgetT4s = new List<BudgetT4Ent>()
            {
                fakeBudgetT4
            };

            var erreur = budgetT4s.CheckBudgetT4OfBudget(isTypeAvancementDynamiqueSurSociete: false);
            Assert.IsTrue(erreur.Length > 0);
        }
    }
}
