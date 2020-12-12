using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportLignePrimeBuilder : ModelDataTestBuilder<RapportLignePrimeEnt>
    {
        public RapportLignePrimeEnt Prototype()
        {
            Model.PrimeId = 1;
            Model.Prime = new PrimeEnt() { Code = "PRIME", PrimeId = 1 };
            Model.RapportLigneId = 1;
            Model.RapportLignePrimeId = 1;

            return Model;
        }
    }
}
