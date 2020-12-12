using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.FeatureFlipping;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.FeatureFlipping
{
    /// <summary>
    /// Référentiel de données pour les features flipping
    /// </summary>
    public class FeatureFlippingRepository : FredRepository<FeatureFlippingEnt>, IFeatureFlippingRepository
    {
        private readonly ICacheManager cacheManager;
        private readonly ILogManager logManager;
        private readonly string featureFlippingCacheKey = "FeatureFlippingCacheKey";

        public FeatureFlippingRepository(FredDbContext context, ICacheManager cacheManager, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Retourne la liste de tous les noms des features
        /// </summary>
        /// <returns>liste de tous les nom de features</returns>
        public IEnumerable<FeatureFlippingEnt> GetFeatureFlippings()
        {
            return cacheManager.GetOrCreate(featureFlippingCacheKey, () => Context.FeatureFlippings, new TimeSpan(1, 0, 0, 0, 0));
        }

        /// <summary>
        /// Retourne la liste de tous les noms des features pouur un Id
        /// </summary>
        /// <param name="code">Code de la feature</param>
        /// <returns>liste de tous les nom de features</returns>
        public FeatureFlippingEnt GetFeatureFlipping(int code)
        {
            return GetFeatureFlippings().FirstOrDefault(x => x.Code == code);
        }

        /// <summary>
        /// Mets à jour les modifications d'une feature Flipping
        /// </summary>
        /// <param name="featureFlipping">Feature Flipping à mettre à jour</param>
        public void UpdateFeatureFlipping(FeatureFlippingEnt featureFlipping)
        {
            try
            {
                cacheManager.Remove(featureFlippingCacheKey);
                Context.Entry(featureFlipping).State = EntityState.Modified;
                cacheManager.GetOrCreate(featureFlippingCacheKey, () => GetFeatureFlippings(), new TimeSpan(1, 0, 0, 0, 0));
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }
    }
}
