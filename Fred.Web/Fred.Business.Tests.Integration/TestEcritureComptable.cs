using System.Diagnostics;
using System.Linq;
using Fred.Business.DatesClotureComptable;
using Fred.DataAccess.DatesClotureComptable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestEcritureComptable : FredBaseTest
    {
        [TestMethod]
        //A ENLEVER Le tag Ignore avant de le lancer en local
        [Ignore]
        public void TestMontantOnEcritureComptableIsEqualToTheSumOfCumuls()
        {
            var ecritures = (from e in this.FredContext.EcritureComptables
                             let ecs = e.EcritureComptableCumul.ToList()
                             let count = ecs.Count()
                             select new { ecriture = e, e.Montant, ecs }).ToList();

            var ecrituresWithSumNotEqual = ecritures.Where(x => x.ecs.Count > 0)
                                        .Select(x => new { x.ecriture, x.Montant, sum = x.ecs.Sum(y => y.Montant) })
                                        .Where(x => x.Montant != x.sum)
                                        .ToList();


            var datesClotureComptableRepository = new DatesClotureComptableRepository(this.FredContext, null);
            var datesClotureComptableManager = new DatesClotureComptableManager(this.Uow, datesClotureComptableRepository, this.UserMgr);


            int countError = 0;
            foreach (var ecritureWithSumNotEqual in ecrituresWithSumNotEqual)
            {
                var dateComptable = ecritureWithSumNotEqual.ecriture.DateComptable;
                var year = dateComptable.Value.Year;
                var month = dateComptable.Value.Month;
                var ciId = ecritureWithSumNotEqual.ecriture.CiId;

                var periodClosed = datesClotureComptableManager.IsPeriodClosed(ciId, year, month);
                if (periodClosed)
                {
                    countError += 1;
                    Debug.WriteLine("Not include EcritureComptableId = " + ecritureWithSumNotEqual.ecriture.EcritureComptableId);
                }
            }

            Assert.IsTrue(countError == 0, "The sum of Montant of ecritureComptableCumul must be equal of Montant of ecritureComptable in open period");

        }

    }
}
