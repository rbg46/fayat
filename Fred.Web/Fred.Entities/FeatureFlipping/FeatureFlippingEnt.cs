using System;

namespace Fred.Entities.FeatureFlipping
{
    /// <summary>
    /// Représente une fonctionnalité
    /// </summary>
    public class FeatureFlippingEnt
    {
        /// <summary>
        /// Obtiens ou définit l'identifiant unique d'une Feature
        /// </summary>
        public int FeatureId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une Feature
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Obtiens ou définit le nom d'une Feature
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiens ou définit l'activation d'une Feature
        /// </summary>
        public bool IsActived { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'activation de la Feature
        /// </summary>
        public DateTime DateActivation { get; set; }

        /// <summary>
        /// Obtiens ou définit le nom de l'utilisateur qui a activé une Feature
        /// </summary>
        public string UserActivation { get; set; }
    }
}
