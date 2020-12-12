using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Common
{
    /// <summary>
    ///   Représente le contexte d'accès à la couche de données.
    /// </summary>
    public static class DataAccessContext
    {
        /// <summary>
        ///   Constante de préfixe permettant l'accès au DbContext depuis le HTTP Context
        /// </summary>
        private const string Prefix = "DataAccessContext_";

        /// <summary>
        ///   Constante de suffixe permettant l'accès au DbContext depuis le HTTP Context
        /// </summary>
        private const string FredContext = "FredContext";

        /// <summary>
        ///   Constante de clé (constituée des deux clés préfixe + suffixe) permettant l'accès au DbContext depuis le HTTP Context
        /// </summary>
        private const string CacheKey = Prefix + FredContext;

        /// <summary>
        ///   Locker permettant le bon fonctionnement du singleton
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        ///   Renvoie l'instance en cours du DbContext utilisé pour accéder à la base de données.
        ///   Si aucune instance n'a été créée dans le contexte en cours, une nouvelle sera
        ///   implémentée.
        /// </summary>
        /// <typeparam name="T">Le type de contexte</typeparam>
        /// <returns>Une nouvelle instance ou l'instance préalabelement chargée.</returns>
        public static T GetCurrentDb<T>() where T : DbContext, new()
        {
            T ctx = ContextHelper.GetData(CacheKey) as T;
            if (ctx == null)
            {
                lock (Locker)
                {
                    ctx = ContextHelper.GetData(CacheKey) as T;
                    if (ctx == null)
                    {
                        lock (Locker)
                        {
                            ctx = new T();
                            ContextHelper.SetData(CacheKey, ctx);
                        }
                    }
                }
            }

            return ctx;
        }
    }
}