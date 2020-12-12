using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Budget.Extensions;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Budget.ExtensionsTests
{
    [TestClass]
    public class AvancementExtensionTest
    {  
        [TestMethod]
        public void TestGetPourcentageAvancementQuantite()
        {
            var avancement = new AvancementEnt()
            {
                QuantiteSousDetailAvancee = 50
            };

            var t4 = new BudgetT4Ent()
            {
                TypeAvancement = (int)TypeAvancementBudget.Quantite
            };

            var quantiteSousDetailBudgete = 100;
            var pourcentageAvancement = avancement.GetPourcentageAvancementSousDetail(t4, quantiteSousDetailBudgete);

            Assert.AreEqual(50, pourcentageAvancement);
        }

        [TestMethod]
        public void TestGetPourcentageAvancementPourcentage()
        {
            var avancement = new AvancementEnt()
            {
                PourcentageSousDetailAvance = 50
            };

            var t4 = new BudgetT4Ent()
            {
                TypeAvancement = (int)TypeAvancementBudget.Pourcentage
            };

            var quantiteSousDetailBudgete = 100;
            var pourcentageAvancement = avancement.GetPourcentageAvancementSousDetail(t4, quantiteSousDetailBudgete);

            //Dans le cas d'un avancement en pourcentage, on peut simplement prendre le pourcentage saisi
            Assert.AreEqual(avancement.PourcentageSousDetailAvance, pourcentageAvancement);
        }

        [TestMethod]
        public void TestGetPourcentageIncoherent()
        {
            //Test avec un avancement devant être en pourcentage mais sans pourcentage saisi
            var avancement = new AvancementEnt();

            var t4 = new BudgetT4Ent()
            {
                TypeAvancement = (int)TypeAvancementBudget.Pourcentage
            };

            var quantiteSousDetailBudgete = 100;
            var pourcentageAvancement = avancement.GetPourcentageAvancementSousDetail(t4, quantiteSousDetailBudgete);
            Assert.AreEqual(0, pourcentageAvancement);


            t4.TypeAvancement = (int)TypeAvancementBudget.Quantite;
            pourcentageAvancement = avancement.GetPourcentageAvancementSousDetail(t4, quantiteSousDetailBudgete);
            Assert.AreEqual(0, pourcentageAvancement);
        }

    }
}
