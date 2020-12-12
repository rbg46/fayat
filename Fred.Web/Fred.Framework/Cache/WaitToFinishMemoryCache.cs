using System;
using System.Runtime.Caching;
using System.Threading;

namespace Fred.Framework.Cache
{
    /// <summary>
    /// Permet de recuperer un element du cache
    /// Un mutex stop les threads qui essaieraient d'exectuter la fonction de creation de l'item en meme temps
    /// </summary>
    public class WaitToFinishMemoryCache
    {
        private static readonly Mutex Mutex = new Mutex();

        /// <summary>
        /// Recupere la valeur du cache ou execute la fonction 'createItem'
        /// </summary>
        /// <param name="cacheKey">La cle du cache</param>
        /// <param name="createItem">la fonction de creation de l'item</param>
        /// <param name="absoluteExpiration">La date absolue d'expiration</param>
        /// <typeparam name="TItem">Type de retour du cache</typeparam>
        /// <returns>Le type de retour de la creation de l'item</returns>
        public TItem GetOrCreate<TItem>(string cacheKey, Func<TItem> createItem, TimeSpan absoluteExpiration)
        {
            TItem cacheObj = default(TItem);

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(cacheKey))
            {
                return (TItem)(cache.Get(cacheKey));
            }

            try
            {
                Mutex.WaitOne();

                if (cache.Contains(cacheKey))
                {
                    return (TItem)(cache.Get(cacheKey));
                }

                CacheItemPolicy policy = new CacheItemPolicy();

                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(absoluteExpiration);

                cacheObj = createItem();

                cache.Add(cacheKey, cacheObj, policy);

            }
            finally
            {
                Mutex.ReleaseMutex();
            }

            return cacheObj;
        }
    }
}
