using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepartitionEcart;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RepartitionEcart
{
    /// <summary>
    ///  Repository des RepartitionEcartEnt
    /// </summary>
    public class RepartitionEcartRepository : FredRepository<RepartitionEcartEnt>, IRepartitionEcartRepository
    {
        public RepartitionEcartRepository(FredDbContext Context)
          : base(Context)
        { }

        /// <summary>
        /// Supprime une liste de <see cref="RepartitionEcartEnt" />
        /// </summary>
        /// <param name="repartitionEcarts">Liste de <see cref="RepartitionEcartEnt" /></param>
        public void Delete(List<RepartitionEcartEnt> repartitionEcarts)
        {
            repartitionEcarts.ForEach(x => Context.RepartitionEcarts.Attach(x));
            Context.RepartitionEcarts.RemoveRange(repartitionEcarts);
        }

        public async Task<IEnumerable<RepartitionEcartEnt>> GetListExistingRepartionsByCiIdAndPeriodeAsync(int ciId, DateTime startDate, DateTime endDate)
        {
            return await Context.RepartitionEcarts
                .Where(re => re.CiId == ciId
                             && re.DateComptable >= startDate
                             && re.DateComptable <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RepartitionEcartEnt>> GetListExistingRepartionsByCiIdsAndPeriodeAsync(List<int> ciIds, DateTime startDate, DateTime endDate)
        {
            return await Context.RepartitionEcarts
                .Where(re => ciIds.Contains(re.CiId) && re.DateComptable >= startDate && re.DateComptable <= endDate)
                .ToListAsync();
        }
    }
}
