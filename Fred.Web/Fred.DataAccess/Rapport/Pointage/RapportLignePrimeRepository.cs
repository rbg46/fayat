using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Framework;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   Référentiel de données pour les lignes de prime.
    /// </summary>
    public class RapportLignePrimeRepository : FredRepository<RapportLignePrimeEnt>, IRapportLignePrimeRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RapportLignePrimeRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public RapportLignePrimeRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Find rapport ligne prime by rapport ligne id and prime id
        /// </summary>
        /// <param name="rapportLigneId">Rapport ligne identifier</param>
        /// <param name="primeId">Prime identifier</param>
        /// <returns>Rapport ligne prime</returns>
        public RapportLignePrimeEnt FindPrime(int rapportLigneId, int primeId)
        {
            return this.Context.RapportLignePrimes.FirstOrDefault(x => x.RapportLigneId == rapportLigneId && x.PrimeId == primeId);
        }
    }
}