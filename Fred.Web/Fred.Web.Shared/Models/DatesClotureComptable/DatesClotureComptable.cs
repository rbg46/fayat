using System;
using Fred.Web.Models.CI;

namespace Fred.Web.Models.DatesClotureComptable
{
    public class DatesClotureComptableModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique de la cloture comptable.
        /// </summary>
        public int DatesClotureComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la CI.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la CI.
        /// </summary>
        public CIModel CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de saisie des clotures comptables.
        /// </summary>
        public DateTime? DateArretSaisie { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de transfert des clotures comptables.
        /// </summary>
        public DateTime? DateTransfertFAR { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la cloture comptable.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit l'année.
        /// </summary>
        public int Annee { get; set; }

        /// <summary>
        /// Obtient ou définit le mois.
        /// </summary>
        public int Mois { get; set; }

        /// <summary>
        ///  Obtient ou définit la periode.
        /// </summary>
        public int Periode { get; set; }
    }
}
