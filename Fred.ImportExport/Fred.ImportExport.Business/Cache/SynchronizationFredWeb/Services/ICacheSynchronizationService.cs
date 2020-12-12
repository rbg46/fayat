namespace Fred.ImportExport.Business.Cache.SynchronizationFredWeb.Services
{
    /// <summary>
    /// Service de synchro du cache entre fred web et fred ie
    /// </summary>
    public interface ICacheSynchronizationService
    {
        /// <summary>
        /// Supprime le cache de fred web
        /// </summary>
        /// <param name="cacheKey">La clé du cache a supprimer</param>
        void RemoveOnFredWeb(string cacheKey);

        /// <summary>
        /// Remove le cache de fredie 
        /// </summary>
        /// <param name="cacheKey">La clé du cache a supprimer</param>
        void RemoveOnFredIe(string cacheKey);
    }
}
