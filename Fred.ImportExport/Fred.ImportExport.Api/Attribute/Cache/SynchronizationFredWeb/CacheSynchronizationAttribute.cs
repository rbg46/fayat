using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Fred.Framework;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.Services;

namespace Fred.ImportExport.Api.Attribute.Cache.SynchronizationFredWeb
{
    /// <summary>
    /// Filtre asp.net web api qui supprime le cache sur fred web et fred ie a partir d'une clé de cache
    /// </summary>
    public class CacheSynchronizationAttribute : ActionFilterAttribute
    {
        private readonly string cacheKey;
        private readonly ICacheSynchronizationService cacheSynchronizationService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cacheKey">La cle de cache a supprimé de fred web et fred ie </param>
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

        /// <summary>Occurs before the action method is invoked.</summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            cacheSynchronizationService.RemoveOnFredIe(cacheKey);
        }

        /// <summary>Occurs after the action method is invoked.</summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            cacheSynchronizationService.RemoveOnFredIe(cacheKey);
            cacheSynchronizationService.RemoveOnFredWeb(cacheKey);
        }

    }
}
