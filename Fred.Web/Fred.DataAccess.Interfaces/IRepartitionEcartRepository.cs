using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RepartitionEcart;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des RepartitionEcartEnt
    /// </summary>
    public interface IRepartitionEcartRepository : IRepository<RepartitionEcartEnt>
    {
        /// <summary>
        /// Supprime une liste de <see cref="RepartitionEcartEnt"/>
        /// </summary>
        /// <param name="repartitionEcarts">Liste de <see cref="RepartitionEcartEnt"/></param>
        void Delete(List<RepartitionEcartEnt> repartitionEcarts);

        Task<IEnumerable<RepartitionEcartEnt>> GetListExistingRepartionsByCiIdAndPeriodeAsync(int ciId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<RepartitionEcartEnt>> GetListExistingRepartionsByCiIdsAndPeriodeAsync(List<int> ciIds, DateTime startDate, DateTime endDate);
    }
}
