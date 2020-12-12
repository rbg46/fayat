using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using Fred.Framework;
using Fred.Web.Shared.Models.Cache;

namespace Fred.Web.API
{
    public class CacheController : ApiControllerBase
    {
        #region Private attribute

        private readonly ICacheManager cacheManager;

        #endregion

        #region Constructor

        public CacheController(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;

        }

        #endregion

        #region Public method


        /// <summary>
        /// Get list des des caches
        /// </summary>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/Cache/List/")]
        [Authorize(Roles = ApplicationRole.SuperAdmin)]
        public HttpResponseMessage GetCacheItems()
        {
            var listCache = cacheManager.GetAll();
            List<CacheModel> cacheModels = new List<CacheModel>();
            foreach (var model in listCache)
            {
                var cacheModel = new CacheModel { CacheKey = model.Key };
                if (model?.ExpirationDate != null)
                {
                    cacheModel.DateExpiration = model.ExpirationDate;
                }

                var obj = model.CacheItem;
                IEnumerable objAsEnumerable = obj as IEnumerable;
                if (obj is ValueType || obj is string)
                {
                    cacheModel.CacheValue = obj.ToString();
                }

                else if (objAsEnumerable != null)
                {
                    var result = objAsEnumerable.Cast<object>().FirstOrDefault();
                    cacheModel.CacheValue = result == null ? string.Empty : $"List : {result?.GetType().Name}";
                }
                else
                {
                    cacheModel.CacheValue = obj.GetType().Name;
                }
                cacheModels.Add(cacheModel);
            }

            return this.Get(() => cacheModels);
        }

        /// <summary>
        /// Supprimer une ligne de cache 
        /// </summary>
        /// <param name="cache">cache Key</param>
        /// <returns>Htttp response message</returns>
        [HttpPost]
        [Route("api/Cache/DeleteCache")]
        public HttpResponseMessage DeleteCache(CacheModel cache)
        {
            return this.Post(() =>
            {
                var isSuperAdmin = User.IsInRole(ApplicationRole.SuperAdmin);
                var isFredIeService = User.Identity.Name == ApplicationUser.FredIeService;
                if (isSuperAdmin || isFredIeService)
                {
                    cacheManager.Remove(cache.CacheKey);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            });
        }

        #endregion

    }

}
