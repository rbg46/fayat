using System.Collections.Generic;
using static Fred.Entities.Rapport.PointageMensuelPersonEnt;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// definit un object d'une liste d'heure d'absence avec son personnel
    /// </summary>
    public class LigneAbsence
    {
        /// <summary>
        /// definit la class des heures d'absence
        /// </summary>
        public List<Absences> HeureAbsence { get; set; }

        /// <summary>
        /// Identifiant de la personne 
        /// </summary>
        public int PersonnelId { get; set; }
    }
}
