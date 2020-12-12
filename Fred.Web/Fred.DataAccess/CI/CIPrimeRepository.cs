using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.CI
{
    /// <summary>
    ///   Référentiel de données pour les CIPrime
    /// </summary>
    public class CIPrimeRepository : FredRepository<CIPrimeEnt>, ICIPrimeRepository
    {
        private readonly FredDbContext context;

        public CIPrimeRepository(FredDbContext context)
            : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public IEnumerable<CIPrimeEnt> GetCIPrimePrivated(int? societeId, DateTime lastModification)
        {
            var result = from c in context.CIPrimes
                         join s in context.Primes on c.PrimeId equals s.PrimeId
                         where s.SocieteId == societeId
                         select c;

            // Synchornisation delta.
            if (lastModification != default(DateTime))
            {
                result.Where(t => t.DateModification >= lastModification);
            }

            return result.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<CIPrimeEnt> GetCIPrimeSync()
        {
            return context.CIPrimes.AsNoTracking();
        }

        /// <inheritdoc />
        public async Task<int> GetCIPrimeIdAsync(int primeId)
        {
            return await context.CIPrimes
                .Where(s => s.PrimeId == primeId)
                .Select(s => s.CiPrimeId)
                .FirstOrDefaultAsync();
        }
    }
}
