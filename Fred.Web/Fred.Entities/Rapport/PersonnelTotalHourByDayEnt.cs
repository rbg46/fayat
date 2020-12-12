using System.Collections.Generic;
namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Entite contenant pour chaque jour les heures travaillés 
    /// </summary>
    public class PersonnelTotalHourByDayEnt
    {

        /// <summary>
        /// le jour dont on veut récupérer les heures normales
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// total des heures pour un ci
        /// </summary>
        public List<PersonnelTotalHourByDayAndByCiEnt> ListTotalHourByCi { get; set; }

        /// <summary>
        /// Code du statut de pointage
        /// </summary>
        public string PointageStatutCode { get; set; }

    }
}
