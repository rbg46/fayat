using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Models;

namespace Fred.Framework
{
    /// <summary>
    ///   Gestionnaire du cache au sein de l'application.
    ///   Utilise en interne le système de cache HTTP standard
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        ///   Renvoie ou crée une entrée de cache en fonction de la clé et de la fonction de création.
        /// </summary>
        /// <typeparam name="T"> Le type d'objet à mettre en cache</typeparam>
        /// <param name="cacheKey"> La clé de cache.</param>
        /// <param name="refreshFunc"> La fonction de rafraichissement/création de la donnée.</param>
        /// <param name="slidingExpiration"> Le délai d'expiration glissante depuis la dernière consultation.</param>
        /// <returns> La donnée stockée en cache ou nouvellement créée.</returns>
        T GetOrCreate<T>(string cacheKey, Func<T> refreshFunc, TimeSpan slidingExpiration);

        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> refreshFunc, TimeSpan slidingExpiration);

        /// <summary>
        ///   Retire du cache l'entrée spécifiée.
        /// </summary>
        /// <param name="cacheKey"> La clé de cache.</param>
        void Remove(string cacheKey);

        /// <summary>
        ///   returner la liste des éléments de cache
        /// </summary>
        /// <returns> la liste des éléments de cache</returns>
        List<CacheItemWithPolicy> GetAll();
    }
}
