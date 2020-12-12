using System.Collections.Generic;
using Fred.Entities.FeatureFlipping;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface Référentiel de données pour les features flipping
    /// </summary>
    public interface IFeatureFlippingRepository : IFredRepository<FeatureFlippingEnt>
    {
        /// <summary>
        /// Retourne la liste de tous les noms des features
        /// </summary>
        /// <returns>liste de tous les nom de features</returns>
        IEnumerable<FeatureFlippingEnt> GetFeatureFlippings();

        /// <summary>
        /// Retourne la liste de tous les noms des features
        /// </summary>
        /// <param name="code">Code de la feature</param>
        /// <returns>liste de tous les nom de features</returns>
        FeatureFlippingEnt GetFeatureFlipping(int code);

        /// <summary>
        /// Mets à jour les modifications d'une feature Flipping
        /// </summary>
        /// <param name="featureFlipping">Feature Flipping à mettre à jour</param>
        void UpdateFeatureFlipping(FeatureFlippingEnt featureFlipping);
    }
}
