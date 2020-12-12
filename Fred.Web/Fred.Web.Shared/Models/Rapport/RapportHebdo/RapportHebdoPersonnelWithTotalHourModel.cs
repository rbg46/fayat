using System.Collections.Generic;
namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Model contenant un personnel avec sa liste de pointage
    /// </summary>
    public class RapportHebdoPersonnelWithTotalHourModel
    {

        /// <summary>
        /// identifiant du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// List des Heures normales de ce personnel
        /// </summary>
        public List<PersonnelTotalHourByDayModel> ListTotalHours { get; set; }

        /// <summary>
        /// Code statut des pointages
        /// </summary>
        public string PointageStatutCode { get; set; }

        /// <summary>
        /// Libelle statut des pointages
        /// </summary>
        public string PointageStatutLibelle { get; set; }

    }
}
