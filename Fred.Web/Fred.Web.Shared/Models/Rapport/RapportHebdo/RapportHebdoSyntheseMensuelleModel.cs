using Fred.Web.Models.Personnel;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Synthese mensuelle du rapport hebdo model pour les ETAM/IAC
    /// </summary>
    public class RapportHebdoSyntheseMensuelleModel
    {
        /// <summary>
        /// Obtient ou definit personnel Model
        /// </summary>
        public PersonnelLightModel Personnel { get; set; }

        /// <summary>
        /// Obtient ou definit Statut des rapports
        /// </summary>
        public string PointageStatut { get; set; }

        /// <summary>
        /// Obtient ou definit nombre des jours pointés
        /// </summary>
        public double NbreJoursPointes { get; set; }

        /// <summary>
        /// Obtient ou definit nombre des jours d'absence
        /// </summary>
        public double NbreAbsences { get; set; }

        /// <summary>
        /// Obtient ou definit nombre des primes
        /// </summary>
        public int NbrePrimes { get; set; }

        /// <summary>
        /// Obtient ou définit le statut su personnel (ETAM , IAC, Ouvrier ..)
        /// </summary>
        public string PersonnelStatut { get; set; }
    }
}
