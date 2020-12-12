using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   Référentiel de données pour les pointages anticipées de prime.
    /// </summary>
    public class PointageAnticipePrimeRepository : FredRepository<PointageAnticipePrimeEnt>, IPointageAnticipePrimeRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PointageAnticipePrimeRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public PointageAnticipePrimeRepository(FredDbContext context, ILogManager logMgr) : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<int> GetPointageAnticipePrimeIdAsync(int primeId)
        {
            return await context.PointageAnticipePrimes
                .Where(s => s.PrimeId == primeId)
                .Select(s => s.PointageAnticipeId)
                .FirstOrDefaultAsync();
        }
    }
}