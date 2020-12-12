using Fred.Entities.Organisation;
using System;

namespace Fred.Entities.CI
{
    /// <summary>
    /// Représente une ci
    /// </summary>
    [Serializable]
    public class CILightEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ci.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une ci.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une affaire.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'affaire
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Organisation
        /// </summary>
        public OrganisationLightEnt Organisation { get; set; }
    }
}
