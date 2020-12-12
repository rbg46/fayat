using System;
using Fred.Framework;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.Services;
using Hangfire.Common;
using Hangfire.Server;

namespace Fred.ImportExport.Business.Cache.SynchronizationFredWeb.HangfireFilter
{
    /// <summary>
    /// Filtre Hangfire qui supprime le cache sur fred web et fred ie a partir d'une clé de cache
    /// </summary>
    public class CacheSynchronizationAttribute : JobFilterAttribute, IServerFilter
    {
        private readonly ICacheSynchronizationService cacheSynchronizatorService;
        private readonly string cacheKey;

        public CacheSynchronizationAttribute(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentNullException(nameof(cacheKey));
            }

            //Pas de dependency injection dans les filtres 
            var logger = new LogManager();
            var cacheManager = new CacheManager(logger);
            this.cacheSynchronizatorService = new CacheSynchronizationService(cacheManager);
            this.cacheKey = cacheKey;
        }

        /// <summary>
        /// Called before the performance of the job.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnPerforming(PerformingContext filterContext)
        {
            try
            {
                this.cacheSynchronizatorService.RemoveOnFredIe(cacheKey);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, $"Erreur lors de la suppression du cache : {cacheKey} fred ie");
            }
        }

        /// <summary>
        /// Called after the performance of the job.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnPerformed(PerformedContext filterContext)
        {
            try
            {
                this.cacheSynchronizatorService.RemoveOnFredIe(cacheKey);
                this.cacheSynchronizatorService.RemoveOnFredWeb(cacheKey);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex, $"Erreur lors de la suppression du cache : {cacheKey} fred ie et de fredweb");
            }
        }

    }
}
