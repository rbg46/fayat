using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.Models;

namespace Fred.Framework
{
    public class CacheManager : ICacheManager
    {
        private readonly ILogManager logMgr;
        private readonly ObjectCache cache;

        public CacheManager(ILogManager logMgr)
        {
            this.logMgr = logMgr;

            cache = MemoryCache.Default;
        }

        public T GetOrCreate<T>(string cacheKey, Func<T> refreshFunc, TimeSpan slidingExpiration)
        {
            try
            {
                var cacheObj = GetFromCache<T>(cacheKey);
                if (cacheObj != null)
                    return cacheObj;

                cacheObj = refreshFunc();
                if (cacheObj == null)
                    return default;

                AddToCache(cacheKey, cacheObj, slidingExpiration);

                return cacheObj;
            }
            catch (Exception exception)
            {
                logMgr.TraceException(exception.Message, exception);
                throw;
            }
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> refreshFunc, TimeSpan slidingExpiration)
        {
            try
            {
                var cacheObj = GetFromCache<T>(cacheKey);
                if (cacheObj != null)
                    return cacheObj;

                cacheObj = await refreshFunc();
                if (cacheObj == null)
                    return default;

                AddToCache(cacheKey, cacheObj, slidingExpiration);

                return cacheObj;
            }
            catch (Exception exception)
            {
                logMgr.TraceException(exception.Message, exception);
                throw;
            }
        }

        private T GetFromCache<T>(string cacheKey)
        {
            if (!cache.Contains(cacheKey))
                return default;

            return (T)((CacheItemWithPolicy)cache.Get(cacheKey)).CacheItem;
        }

        private void AddToCache<T>(string cacheKey, T cacheObj, TimeSpan slidingExpiration)
        {
            var policy = new CacheItemPolicy { SlidingExpiration = slidingExpiration };
            var cacheItemWithPolicy = new CacheItemWithPolicy
            {
                CacheItem = cacheObj,
                CacheItemPolicy = policy,
                ExpirationDate = DateTime.Now.AddSeconds(slidingExpiration.TotalSeconds),
                Key = cacheKey
            };

            cache.Add(cacheKey, cacheItemWithPolicy, policy);
        }

        public void Remove(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        public List<CacheItemWithPolicy> GetAll()
        {
            List<CacheItemWithPolicy> cacheList = new List<CacheItemWithPolicy>();
            try
            {
                for (int i = 0; i < cache.GetCount(); i++)
                {
                    var currentCacheItem = cache.ElementAt(i);
                    CacheItemWithPolicy itemWithPolicy = cache.Get(currentCacheItem.Key) as CacheItemWithPolicy;

                    if (itemWithPolicy != null)
                    {
                        cacheList.Add(itemWithPolicy);
                    }
                    else
                    {
                        CacheItemWithPolicy cacheItem = new CacheItemWithPolicy
                        {
                            Key = currentCacheItem.Key,
                            CacheItem = currentCacheItem.Value,
                        };
                        cacheList.Add(cacheItem);
                    }
                }
            }
            catch (Exception e)
            {
                this.logMgr?.TraceException(e.Message, e);
                throw new FredBusinessException(e.Message, e);
            }

            return cacheList;
        }
    }
}
