using System;
using System.Configuration;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;

namespace Fred.ImportExport.DataAccess.ExternalService.Cache.SynchronizationFredWeb
{
    /// <summary>
    /// Supprime le cache de fred web avec un appel http
    /// </summary>
    public class DisableFredWebCacheRepository : IDisableFredWebCacheRepository
    {
        private static readonly string baseUrl = ConfigurationManager.AppSettings["FredWeb:RootUrl"];
        private static readonly string tokenUrl = string.Format(ExternalEndPoints.Authenticate_Post_Token_Fred_Web, baseUrl);
        private static readonly string loginService = ConfigurationManager.AppSettings["FredWeb:ServiceAccount:Login"];
        private static readonly string passwordService = ConfigurationManager.AppSettings["FredWeb:ServiceAccount:Password"];


        /// <summary>
        /// Appelle fred web pour desactiver le cache
        /// </summary>
        /// <param name="cacheKey">La clé du cache dont on veut supprimer de fred web</param>
        public async Task RequestFredWebToDisableCache(string cacheKey)
        {
            try
            {
                var restClient = new RestClient(loginService, passwordService, tokenUrl);
                string requestUri = string.Format(ExternalEndPoints.Delete_Fred_Web_Cache, baseUrl);

                await restClient.PostAndEnsureSuccessAsync(requestUri, new { CacheKey = cacheKey });
            }
            catch (Exception ex)
            {
                throw new FredRepositoryException(ex.Message, ex.InnerException);
            }
        }
    }
}
