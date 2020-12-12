using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Referential.StatutAbsence
{
    /// <summary>
    /// Classe repo de statut absence
    /// </summary>
    public class StatutAbsenceRepository : FredRepository<StatutAbsenceEnt>, IStatutAbsenceRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="StatutAbsenceRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public StatutAbsenceRepository(FredDbContext context)
          : base(context)
        {
        }
        /// <summary>
        ///   La liste de tous les statuts d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des statuts d'absence active</returns>
        public IEnumerable<StatutAbsenceEnt> GetStatutAbsList()
        {
            return Context.StatutAbsences.Where(s => s.IsActif);
        }
    }
}
