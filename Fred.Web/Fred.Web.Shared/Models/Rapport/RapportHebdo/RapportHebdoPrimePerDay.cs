using System;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Rapport hebdo prime pointage par jour
    /// </summary>
    public class RapportHebdoPrimePerDay
    {
        /// <summary>
        /// Obtient ou definit Rapport identifier
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Obtient ou definit Rapport ligne identifier
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        /// Obtient ou definit jour du semaine
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Obtient ou definit heure majoration
        /// </summary>
        public double? HeurePrime { get; set; }

        /// <summary>
        /// Obtient ou definit heure majoration ancien value
        /// </summary>
        public double? HeurePrimeOldValue { get; set; }

        /// <summary>
        /// Obtient ou definit if prime is affectd
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Obtient ou definit si la ligne est crée
        /// </summary>
        public bool IsCreated { get; set; }

        /// <summary>
        /// Obtient ou definit si laigne est modifié
        /// </summary>
        public bool IsUpdated { get; set; }
        
        /// <summary>
        /// retourne true si le rapport est verrouille
        /// </summary>
        public bool PersonnelVerrouille { get; set; }

        /// <summary>
        /// retourne true si le rapport est valide
        /// </summary>
        public bool? RapportValide { get; set; }

        /// <summary>
        /// retourne la date du pointage de la journee 
        /// </summary>
        public DateTime datePointage { get; set; }

    }
}
