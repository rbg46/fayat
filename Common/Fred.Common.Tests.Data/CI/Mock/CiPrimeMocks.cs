using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.Referential.Prime.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.CI;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.CI.Mock
{
    /// <summary>
    /// Element fictifs des ci primes
    /// </summary>
    public class CiPrimeMocks
    {
        private readonly List<PrimeEnt> primes = new PrimeMocks().GetFakeDbSet().ToList();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de CiPrimeEnt</returns>
        public FakeDbSet<CIPrimeEnt> GetFakeDbSet()
        {
            return new FakeDbSet<CIPrimeEnt>
            {
                new CIPrimeEnt
                {
                    CiPrimeId = 1,
                    CiId = 1,
                    PrimeId = primes[2].PrimeId, //prime privée
                    Prime = primes[2] //prime privée
                }
            };
        }
    }
}
