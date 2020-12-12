using System.Threading.Tasks;

namespace Fred.ImportExport.DataAccess.ExternalService.Cache.SynchronizationFredWeb
{
    /// <summary>
    /// Supprime le cache de fred web
    /// </summary>
    public interface IDisableFredWebCacheRepository
    {
        /// <summary>
        /// Appelle fred web pour desactiver le cache
        /// </summary>
        /// <param name="cacheKey">La clé du cache dont on veut supprimer de fred web</param>
        Task RequestFredWebToDisableCache(string cacheKey);
    }
}
