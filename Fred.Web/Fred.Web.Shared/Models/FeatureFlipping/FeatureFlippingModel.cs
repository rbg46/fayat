using System;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.FeatureFlipping
{
    public class FeatureFlippingModel
    {
        /// <summary>
        /// Obtiens ou définit le FeatureId
        /// </summary>
        [Required]
        public int FeatureId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une Feature (Unique)
        /// </summary>
        [Required]
        public int Code { get; set; }

        /// <summary>
        /// Obtiens ou définit le nom de la Feature (Unique)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiens ou définit l'état de la Feature
        /// </summary>
        public bool IsActived { get; set; }

        /// <summary>
        /// Obtiens ou définit la date d'activation de la feature
        /// </summary>
        public DateTime DateActivation { get; set; }

        /// <summary>
        /// Obtiens ou définit l'utilisateur qui active la feature
        /// </summary>
        public string UserActivation { get; set; }
    }
}
