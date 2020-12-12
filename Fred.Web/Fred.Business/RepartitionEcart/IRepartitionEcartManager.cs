using Fred.Business.Models.RepartitionEcart;
using Fred.Entities.RepartitionEcart;
using Fred.Framework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.RepartitionEcart
{
    /// <summary>
    /// Manager des RepartitionEcartEnt
    /// </summary>
    public interface IRepartitionEcartManager : IManager<RepartitionEcartEnt>
    {
        /// <summary>
        /// Recupere les RepartitionEcart pour un ci et une date comptable.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">Date comptable de la clôture</param>
        /// <returns>RepartitionEcartWrapper</returns>
        Task<RepartitionEcartWrapper> GetByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Créer les OD d'ecarts et cloture les od. Créer aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">Date comptable de la clôture</param>
        /// <returns>Result de string</returns>
        Task<Result<string>> ClotureAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Créer les OD d'ecarts et cloture les od. Créer aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciIds">Liste de CI Id</param>
        /// <param name="dateComptable">Date comptable de la clôture</param>
        /// <returns>Result de string</returns>
        Task<Result<string>> ClotureAsync(List<int> ciIds, DateTime dateComptable);

        /// <summary>
        /// Supprime les OD d'ecarts et cloture les od. Supprime aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">Date comptable de la clôture</param>
        /// <returns>Result de string</returns>
        Task<Result<string>> DeClotureAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Supprime les OD d'ecarts et cloture les od. Supprime aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciIds">Liste de CI Id</param>
        /// <param name="dateComptable">Date comptable de la clôture</param>
        /// <returns>Result de string</returns>
        Task<Result<string>> DeClotureAsync(List<int> ciIds, DateTime dateComptable);
    }
}
