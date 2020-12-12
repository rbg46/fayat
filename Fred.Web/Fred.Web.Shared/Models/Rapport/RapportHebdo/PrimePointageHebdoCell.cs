using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// prime pointage rapport hebdo cell
    /// </summary>
    public class PrimePointageHebdoCell : PointageCell
    {
        /// <summary>
        /// Obtient or definit prime identifier
        /// </summary>
        public int PrimeId { get; set; }

        /// <summary>
        /// Obtient or definit prime libelle
        /// </summary>
        public string PrimeLibelle { get; set; }

        /// <summary>
        /// Obtient or definit prime code
        /// </summary>
        public string PrimeCode { get; set; }

        /// <summary>
        /// Obtient or definit prime type
        /// </summary>
        public bool IsPrimeJournaliere { get; set; }

        /// <summary>
        /// Obtient ou definit si la prime est pour l'Astreinte
        /// </summary>
        public bool? IsPrimeAstreinte { get; set; }

        /// <summary>
        /// Obtient or definit list des primes pointage par jour
        /// </summary>
        public List<RapportHebdoPrimePerDay> RapportHebdoPrimePerDayList { get; set; }
    }
}
