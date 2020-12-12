using System;
using Fred.Framework;
using Fred.ImportExport.DataAccess.ExternalService.Cache.SynchronizationFredWeb;

namespace Fred.ImportExport.Business.Cache.SynchronizationFredWeb.Services
{
    /// <summary>
    /// Service de synchronisation du cache entre fred web et fred ie
    /// </summary>
    public class CacheSynchronizationService : ICacheSynchronizationService
    {
        private readonly IDisableFredWebCacheRepository disableFredWebCacheRepository;
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cacheManager">Le cache manager</param>
        /// <param name="disableFredWebCacheRepository">Le repo pour acceder a fred web et annuler son cache</param>      
        public CacheSynchronizationService(ICacheManager cacheManager)
        {
            if (cacheManager == null)
            {
                throw new ArgumentNullException(nameof(cacheManager));
            }

            this.cacheManager = cacheManager;

            this.disableFredWebCacheRepository = new DisableFredWebCacheRepository();
        }

        public void RemoveOnFredIe(string cacheKey)
        {

            try
            {
                this.cacheManager.Remove(cacheKey);
                NLog.LogManager.GetCurrentClassLogger().Info("Remise a zero du cache des organisations sur fredIe");
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Remise a zero du cache des organisations sur fredIe");
            }
        }

        /// <summary>
        /// Remove le cache de fred web 
        /// </summary>
        /// <param name="cacheKey">La clé du cache a supprimer</param>
        public void RemoveOnFredWeb(string cacheKey)
        {
            try
            {
                this.disableFredWebCacheRepository.RequestFredWebToDisableCache(cacheKey);
                NLog.LogManager.GetCurrentClassLogger().Info("Remise a zero du cache des organisations sur fredweb");
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, "Remise a zero du cache des organisations sur fredweb");
            }
        }

    }
}
