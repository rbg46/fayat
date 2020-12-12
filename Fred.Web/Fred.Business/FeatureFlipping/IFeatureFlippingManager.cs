using System.Collections.Generic;
using Fred.Entities.FeatureFlipping;
using Fred.Framework.FeatureFlipping;


namespace Fred.Business.FeatureFlipping
{
    /// <summary>
    ///  Interface du gestionnaire des features Flipping
    ///  Merci de lire la doc !
    /// </summary>
    public interface IFeatureFlippingManager : IManager<FeatureFlippingEnt>
    {
        /// <summary>
        /// Retourne la liste des Features
        /// </summary>
        /// <returns>Liste de feature</returns>
        IEnumerable<FeatureFlippingEnt> GetFeatureFlippings();

        /// <summary>
        /// Sauvegarde les modifications d'une Feature
        /// </summary>
        /// <param name="item">Feature à modifier</param>
        /// <returns>La feature</returns>
        FeatureFlippingEnt Update(FeatureFlippingEnt item);

        /// <summary>
        /// Indique si une feature est activée ou non
        /// </summary>
        /// <param name="code">Identifiant unique de la feature = Code</param>
        /// <returns>Retourne true si la feature est activée, sinon false</returns>
        bool IsActivated(EnumFeatureFlipping code);
    }
}
