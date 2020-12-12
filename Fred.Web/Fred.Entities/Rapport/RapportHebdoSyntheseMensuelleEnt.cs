using Fred.Entities.Personnel;

namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Synthese mensuelle du rapport hebdo pour les ETAM/IAC
    /// </summary>
    public class RapportHebdoSyntheseMensuelleEnt
    {
        /// <summary>
        /// Obtient ou definit personnel entite
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

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
        /// Variable utilisée coté front 
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Obtient ou définit le statut su personnel (ETAM , IAC, Ouvrier ..)
        /// </summary>
        public string PersonnelStatut { get; set; }
    }
}
