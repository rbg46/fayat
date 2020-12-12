using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Facturation;

namespace Fred.Common.Tests.Data.Facturation.Mock
{
    public class FacturationMock
    {
        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<FacturationEnt> GetFakeDbSet()
        {
            return new FakeDbSet<FacturationEnt>
            {
                new FacturationEnt()
                {
                    CiId = 1,
                    CommandeId = 1,
                    NumeroFactureSAP = "5106113213",
                    MontantTotalHT = 100,
                    DebitCredit = "H",
                    DeviseId = 48 // €
                }
            };
        }
    }
}
