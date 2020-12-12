using System;
using System.Web.Mvc;
using Fred.Framework;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.Services;

namespace Fred.ImportExport.Web.Attributes.SynchronizationFredWeb
{
    /// <summary>
    /// Filtre asp.net qui supprime le cache sur fred web et fred ie a partir d'une clé de cache
    /// </summary>
    public class CacheSynchronizationAttribute : ActionFilterAttribute
    {
        private readonly string cacheKey;
        private readonly ICacheSynchronizationService cacheSynchronizationService;

        public CacheSynchronizationAttribute(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentNullException(nameof(cacheKey));
            }

            //Pas de dependency injection dans les filtres        
            var logger = new LogManager();
            var cacheManager = new CacheManager(logger);
            this.cacheSynchronizationService = new CacheSynchronizationService(cacheManager);
            this.cacheKey = cacheKey;
        }

        /// <summary>Called by the ASP.NET MVC framework before the action method executes.</summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            cacheSynchronizationService.RemoveOnFredIe(cacheKey);
        }

        /// <summary>Called by the ASP.NET MVC framework after the action method executes.</summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            cacheSynchronizationService.RemoveOnFredIe(cacheKey);
            cacheSynchronizationService.RemoveOnFredWeb(cacheKey);
        }

    }
}
