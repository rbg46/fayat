using System;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Resultat d'une duplication d'un rapport
    /// </summary>
    public class RapportDuplicateModel
    {
        /// <summary>
        /// L'identifiant d'un rapport
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Date de debut de la periode
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date de fin de la periode
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
