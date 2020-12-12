using System;
using System.Collections.Generic;

namespace Fred.Entities.Affectation
{
    /// <summary>
    /// Calendar affectation view entity
    /// </summary>
    public class CalendarAffectationViewEnt
    {
        /// <summary>
        /// Obtient ou definit ci identifier
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou definit  date du premier jour de la semaine
        /// </summary>
        public DateTime StartDateOfTheWeek { get; set; }

        /// <summary>
        /// Obtient ou definit date du dernier jour de la semaine
        /// </summary>
        public DateTime EndDateOfTheWeek { get; set; }

        /// <summary>
        /// Obtient ou definit la valeur de gestion de l'astreint au niveau d'un ci 
        /// </summary>
        public bool IsAstreinteActive { get; set; }

        /// <summary>
        /// Obtient ou definit la liste des affectations
        /// </summary>
        public IEnumerable<AffectationViewEnt> AffectationList { get; set; }

        /// <summary>
        /// Obtient ou definit si les affectations de ce CI contient des astreintes
        /// </summary>
        public bool IsCiAffectationsHasAstreintes { get; set; }

        /// <summary>
        /// Obtient ou définit si le CI est ouvert ou non au pointage
        /// </summary>
        public bool IsDisableForPointage { get; set; }
    }
}
