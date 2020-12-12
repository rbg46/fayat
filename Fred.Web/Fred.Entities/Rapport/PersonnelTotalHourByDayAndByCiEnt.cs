using System;

namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Entite contenant pour chaque journée le nombre d'heure et le Ci 
    /// </summary>
    public class PersonnelTotalHourByDayAndByCiEnt
    {
        /// <summary>
        /// les heures normales
        /// </summary>
        public double TotalHours { get; set; }

        /// <summary>
        /// les heures Majoration
        /// </summary>
        public double TotalMajoration { get; set; }

        /// <summary>
        /// les heures d'absence
        /// </summary>
        public double TotalAbsence { get; set; }

        /// <summary>
        /// Identifiant du Ci
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Date du Pointage Pour le Pointage Mensuel
        /// </summary>
        public DateTime DayPointageForMonth { get; set; }

        /// <summary>
        /// Code du statut de pointage
        /// </summary>
        public string PointageStatutCode { get; set; }
    }
}
