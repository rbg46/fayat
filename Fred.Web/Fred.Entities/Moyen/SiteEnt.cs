using System;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente un site
    /// </summary>
    public class SiteEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du site
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du site
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libélle du site
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le date de création du site
        /// </summary>
        public DateTime? DateCreation { get; set; }
    }
}
