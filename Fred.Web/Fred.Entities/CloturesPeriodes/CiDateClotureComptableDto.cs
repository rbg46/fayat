using System;

namespace Fred.Entities.CloturesPeriodes
{
    /// <summary>
    /// CiDateClotureComptableDto
    /// </summary>
    public class CiDateClotureComptableDto
    {
        /// <summary>
        /// CiId
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// DateOuverture
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        /// DateFermeture
        /// </summary>
        public DateTime? DateFermeture { get; set; }

        /// <summary>
        /// Societe
        /// </summary>
        public SocieteDto Societe { get; set; }

        /// <summary>
        /// DatesClotureComptableId
        /// </summary>
        public int? DatesClotureComptableId { get; set; }

        /// <summary>
        /// DateTransfertFAR
        /// </summary>
        public DateTime? DateTransfertFAR { get; set; }

        /// <summary>
        /// DateCloture
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// PereId
        /// </summary>
        public int? PereId { get; set; }

        /// <summary>
        /// TypeOrganisationId
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        /// OrganisationId
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Libelle
        /// </summary>
        public string Libelle { get; set; }
    }
}
